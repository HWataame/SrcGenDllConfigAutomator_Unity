/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SourceGeneratorのdll置き場フォルダーを示すマーカーアセット

SourceGeneratorTargetFolderMarker.cs
────────────────────────────────────────
バージョン: 1.1.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using System;
using UnityEngine;

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// SourceGeneratorのdll置き場フォルダーを示すマーカーアセット
    /// </summary>
    /// <remarks>このアセットが置かれたフォルダー以下を置き場として扱う</remarks>
    [Serializable]
    [CreateAssetMenu(menuName = "SourceGenerator補助/dllフォルダーマーカー (DLL Folder Marker Asset)", fileName = "_SrcGenTargetFolderMarker")]
    public class SourceGeneratorTargetFolderMarker : ScriptableObject { }
}
