/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Unityのコンパイル関連の処理を提供するクラス

CompileUtil.cs
────────────────────────────────────────
バージョン: 1.0.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// Unityのコンパイル関連の処理を提供するクラス
    /// </summary>
    internal static class CompileUtil
    {
        /// <summary>
        /// 再コンパイル処理用のセッションステートのキー文字列
        /// </summary>
        private const string RecompileSessionKey = "HW.SrcGenDllLabelAutomator.CompileUtil.Recompile";


        /// <summary>
        /// 再コンパイルを要求する
        /// </summary>
        public static void RequestRecompile()
        {
            SessionState.SetBool(RecompileSessionKey, true);

            EditorApplication.update += ProcessProgressBar;
            CompilationPipeline.compilationFinished += OnCompilationFinished;

            ProcessProgressBar();
            CompilationPipeline.RequestScriptCompilation();
        }

        /// <summary>
        /// 進捗バーウィンドウを処理する
        /// </summary>
        private static void ProcessProgressBar()
        {
            if (SessionState.GetBool(RecompileSessionKey, false))
            {
                EditorUtility.DisplayProgressBar(
                    "SourceGenerator DLL Configuration Automator",
                    "Waiting Recompile",
                    0);
            }
            else
            {
                CloseProgressBar();
            }
        }

        /// <summary>
        /// リロード完了時の処理
        /// </summary>
        [DidReloadScripts]
        private static void OnReloaded()
        {
            if (SessionState.GetBool(RecompileSessionKey, false))
            {
                CloseProgressBar();
            }
        }

        /// <summary>
        /// コンパイル完了時の処理
        /// </summary>
        private static void OnCompilationFinished(object obj)
        {
            CompilationPipeline.compilationFinished -= OnCompilationFinished;

            if (!EditorApplication.isCompiling) CloseProgressBar();
        }

        /// <summary>
        /// 進捗バーウィンドウを閉じる
        /// </summary>
        private static void CloseProgressBar()
        {
            SessionState.EraseBool(RecompileSessionKey);
            EditorUtility.ClearProgressBar();

            EditorApplication.update -= ProcessProgressBar;
            CompilationPipeline.compilationFinished -= OnCompilationFinished;
        }
    }
}
