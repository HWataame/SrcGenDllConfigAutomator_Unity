/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SourceGenerator用のdllへの処理を提供するクラス

SrcGenDllUtil.cs
────────────────────────────────────────
バージョン: 1.0.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;
#pragma warning disable IDE0063

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// SourceGenerator用のdllへの処理を提供するクラス
    /// </summary>
    internal static class SrcGenDllUtil
    {
        /// <summary>
        /// マーカーアセットが配置されたフォルダ以下にある無視すべきdllに付与されたラベルの文字列
        /// </summary>
        public const string AvoidLabel = "AvoidAutoSrcGenConfig";
        /// <summary>
        /// Microsoft.CodeAnalysis.CSharpのアセンブリ名
        /// </summary>
        private const string CodeAnalysisDllName = "Microsoft.CodeAnalysis.CSharp";
        /// <summary>
        /// [Generator]の型名のフルネーム
        /// </summary>
        private const string TargetAttributeName = "Microsoft.CodeAnalysis.GeneratorAttribute";
        /// <summary>
        /// ISourceGeneratorの型名のフルネーム
        /// </summary>
        private const string TargetSourceInterfaceName = "Microsoft.CodeAnalysis.ISourceGenerator";
        /// <summary>
        /// IIncrementalGeneratorの型名のフルネーム
        /// </summary>
        private const string TargetIncrementalInterfaceName = "Microsoft.CodeAnalysis.IIncrementalGenerator";


        /// <summary>
        /// PluginImporterの対象がSourceGenerator用のdllであるか判定する
        /// </summary>
        /// <param name="importer">PluginImporter</param>
        /// <returns>判定結果</returns>
        public static bool CheckContainsSourceGenerator(PluginImporter importer)
        {
            if (!importer ||
                importer.isNativePlugin)
            {
                return false;
            }

            var targetPath = importer.assetPath;
            try
            {
                var targetFullPath = Path.GetFullPath(targetPath);
                using (var assembly = AssemblyDefinition.ReadAssembly(targetFullPath))
                {
                    var asmMainModule = assembly.MainModule;
                    if (!asmMainModule.AssemblyReferences
                            .Any(x => x.Name == CodeAnalysisDllName))
                    {
                        return false;
                    }

                    Queue<TypeDefinition> typeQueue = new(asmMainModule.Types);
                    while (typeQueue.TryDequeue(out var asmType))
                    {
                        if (asmType.IsClass &&
                            !asmType.IsAbstract &&
                            asmType.HasCustomAttributes &&
                            asmType.CustomAttributes
                                .Any(x => x.AttributeType.FullName == TargetAttributeName) &&
                            asmType.HasInterfaces &&
                            asmType.Interfaces
                                .Select(x=>x.InterfaceType.FullName)
                                .Any(x => x == TargetSourceInterfaceName || x == TargetIncrementalInterfaceName))
                        {
                            return true;
                        }

                        if (asmType.HasNestedTypes)
                        {
                            foreach (var nestedType in asmType.NestedTypes)
                            {
                                typeQueue.Enqueue(nestedType);
                            }
                        }
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// オブジェクトがSourceGenerator用のdllであるか判定する
        /// </summary>
        /// <param name="target">オブジェクト</param>
        /// <returns>判定結果</returns>
        public static bool CheckContainsSourceGenerator(Object target)
        {
            if (!target ||
                !AssetDatabase.IsMainAsset(target))
            {
                return false;
            }

            var targetPath = AssetDatabase.GetAssetPath(target);
            return AssetImporter.GetAtPath(targetPath) is PluginImporter importer &&
                CheckContainsSourceGenerator(importer);
        }

        /// <summary>
        /// SourceGenerator用のDLLのパスを取得する
        /// </summary>
        /// <returns>SourceGenerator用のDLLのパス</returns>
        public static string[] GetSrcGenDllAssetPaths()
        {
            var guids = AssetDatabase.FindAssets("glob:\"Assets/**/*.dll\"");
            var paths = new List<string>();

            for (int i = 0; i < guids.Length; ++i)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (AssetDatabase.LoadAssetAtPath<Object>(path) is not Object dllAsset ||
                    !CheckContainsSourceGenerator(dllAsset) ||
                    AssetDatabase.GetLabels(dllAsset).Contains(AvoidLabel))
                {
                    continue;
                }

                paths.Add(path);
            }

            return paths.ToArray();
        }
    }
}
