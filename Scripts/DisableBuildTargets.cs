/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
dllを無効化するビルド対象を提供するクラス

DisableBuildTargets.cs
────────────────────────────────────────
バージョン: 1.1.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using System;
using UnityEditor;

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// dllを無効化するビルド対象を提供するクラス
    /// </summary>
    internal static class DisableBuildTargets
    {
        /// <summary>
        /// dllを無効化するビルド対象
        /// </summary>
        private static readonly BuildTarget[] targets = new BuildTarget[]
        {
            BuildTarget.Android,
            BuildTarget.EmbeddedLinux,
            BuildTarget.GameCoreXboxOne,
            BuildTarget.GameCoreXboxSeries,
            BuildTarget.iOS,
            BuildTarget.Kepler,
            BuildTarget.LinuxHeadlessSimulation,
            BuildTarget.PS4,
            BuildTarget.PS5,
            BuildTarget.QNX,
            BuildTarget.StandaloneLinux64,
            BuildTarget.StandaloneOSX,
            BuildTarget.StandaloneWindows,
            BuildTarget.StandaloneWindows64,
            BuildTarget.Switch,
            BuildTarget.Switch2,
            BuildTarget.tvOS,
            BuildTarget.VisionOS,
            BuildTarget.WebGL,
            BuildTarget.WSAPlayer,
            BuildTarget.XboxOne
        };


        /// <summary>
        /// dllを無効化するビルド対象を取得する
        /// </summary>
        /// <returns>dllを無効化するビルド対象</returns>
        public static ReadOnlySpan<BuildTarget> Get()
        {
            return targets;
        }
    }
}
