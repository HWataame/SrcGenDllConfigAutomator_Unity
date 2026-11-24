/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
マーカーアセットが配置されたフォルダー以下の
SourceGeneratorのdllを自動設定するクラス

DllConfigurator.cs
────────────────────────────────────────
バージョン: 1.1.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#pragma warning disable IDE0063

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// マーカーアセットが配置されたフォルダー以下のSourceGeneratorのdllを自動設定するクラス
    /// </summary>
    internal class DllConfigurator : AssetPostprocessor
    {
        /// <summary>
        /// SourceGeneratorのdllに付与すべきラベルの文字列
        /// </summary>
        private const string TargetLabel = "RoslynAnalyzer";


        /// <summary>
        /// プロジェクトのアセット状態が変化した時の処理
        /// </summary>
        private static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var targetDirPaths = GetTargetDirPaths();
            var dllPaths = SrcGenDllUtil.GetSrcGenDllAssetPaths();

            bool hasUpdatedDlls = false;
            for (int i = 0; i < dllPaths.Length; i++)
            {
                var dllPath = dllPaths[i];

                bool isSetLabel = false;
                for (int j = 0; j < targetDirPaths.Length; ++j)
                {
                    var targetDirPath = targetDirPaths[j];

                    if (dllPath.StartsWith(targetDirPath))
                    {
                        isSetLabel = true;
                        break;
                    }
                }

                if (AssetImporter.GetAtPath(dllPath) is not PluginImporter importer)
                {
                    continue;
                }

                var labels = new HashSet<string>(AssetDatabase.GetLabels(importer));
                bool isUpdateSettings;
                if (isSetLabel)
                {
                    isUpdateSettings = ProcessDllImportSettings(importer);

                    isUpdateSettings |= labels.Add(TargetLabel);
                }
                else
                {
                    isUpdateSettings = labels.Remove(TargetLabel);
                }

                if (isUpdateSettings)
                {
                    AssetDatabase.SetLabels(importer, labels.ToArray());
                    importer.SaveAndReimport();

                    hasUpdatedDlls = true;
                }
            }

            if (hasUpdatedDlls) CompileUtil.RequestRecompile();
        }

        /// <summary>
        /// マーカーアセットが存在する、処理対象のディレクトリを取得する
        /// </summary>
        /// <returns>処理対象のディレクトリを取得する</returns>
        private static string[] GetTargetDirPaths()
        {
            var pathEnumerable = AssetDatabase.FindAssets($"t:{nameof(SourceGeneratorTargetFolderMarker)}", new[] { "Assets" })
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Where(x => AssetDatabase.LoadAssetAtPath<Object>(x))
                .Select(x => Path.GetDirectoryName(x))
                .Distinct()
                .Select(x => x.Replace('\\', '/'))
                .OrderBy(x => x);
            var targetDirPaths = new List<string>();
            foreach (var path in pathEnumerable)
            {
                if (targetDirPaths.Count > 0)
                {
                    bool isChild = false;
                    for (int i = 0; i < targetDirPaths.Count; ++i)
                    {
                        isChild = path.StartsWith(targetDirPaths[i] + "/");
                    }

                    if (isChild) continue;
                }

                targetDirPaths.Add(path + "/");
            }

            return targetDirPaths.ToArray();
        }

        /// <summary>
        /// SourceGeneratorのdll用のインポート設定を行う
        /// </summary>
        /// <param name="importer">dllのインポーター</param>
        /// <returns>設定が変更されたか</returns>
        private static bool ProcessDllImportSettings(PluginImporter importer)
        {
            using (var serializedObject = new SerializedObject(importer))
            {
                using (var isExplicitlyReferenced = serializedObject.FindProperty("m_IsExplicitlyReferenced"))
                {
                    // エディター上では「Auto Reference」のチェックを外した状態
                    isExplicitlyReferenced.boolValue = true;
                }
                using (var validateReferences = serializedObject.FindProperty("m_ValidateReferences"))
                {
                    validateReferences.boolValue = false;
                }

                bool isUpdated = serializedObject.ApplyModifiedPropertiesWithoutUndo();
                if (isUpdated) AssetDatabase.SaveAssetIfDirty(importer);

                if (importer.GetCompatibleWithAnyPlatform())
                {
                    importer.SetCompatibleWithAnyPlatform(false);
                    isUpdated = true;
                }
                if (importer.GetCompatibleWithEditor())
                {
                    importer.SetCompatibleWithEditor(false);
                    isUpdated = true;
                }
                var buildTargets = DisableBuildTargets.Get();
                for (int i = 0; i < buildTargets.Length; ++i)
                {
                    var buildTarget = buildTargets[i];
                    if (importer.GetCompatibleWithPlatform(buildTarget))
                    {
                        importer.SetCompatibleWithPlatform(buildTarget, false);
                        isUpdated = true;
                    }
                }

                return isUpdated;
            }
        }
    }
}
