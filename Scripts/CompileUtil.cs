/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Unityのコンパイル関連の処理を提供するクラス

CompileUtil.cs
────────────────────────────────────────
バージョン: 1.1.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEditor.Compilation;
#pragma warning disable IDE0063

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// Unityのコンパイル関連の処理を提供するクラス
    /// </summary>
    internal static class CompileUtil
    {
        /// <summary>
        /// ユーザーが作業しているウィンドウのハンドルを取得する
        /// </summary>
        /// <returns>ユーザーが作業しているウィンドウのハンドル</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern nint GetForegroundWindow();

        /// <summary>
        /// ウィンドウを生成したスレッドのIDとプロセスのIDを取得する
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="processId">ウィンドウを生成したプロセスのID</param>
        /// <returns>ウィンドウを生成したスレッドのID</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(nint hWnd, out uint processId);


        /// <summary>
        /// 再コンパイルを要求する
        /// </summary>
        public static void RequestRecompile()
        {
            CompilationPipeline.RequestScriptCompilation();

            var currentForegroundWindow = GetForegroundWindow();
            if (currentForegroundWindow != 0 &&
                GetWindowThreadProcessId(currentForegroundWindow, out var foregroundProcessId) != 0 &&
                foregroundProcessId == Process.GetCurrentProcess().Id)
            {
                // UnityEditorのウィンドウがアクティブである場合は
                // 一瞬で消えるターミナルを出して一瞬非アクティブにする
                // (反応性の向上策)
                using (var process = new Process())
                {
                    var psi = new ProcessStartInfo("conhost.exe")
                    {
                        UseShellExecute = false,
                        CreateNoWindow = false
                    };
                    psi.ArgumentList.Add("cmd");
                    psi.ArgumentList.Add("/c");
                    process.StartInfo = psi;

                    process.Start();
                }
            }
        }
    }
}
