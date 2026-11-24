/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SourceGeneratorTargetFolderMarkerのインスペクター

SourceGeneratorTargetFolderMarkerInspector.cs
────────────────────────────────────────
バージョン: 1.1.0
2026 Wataame(HWataame)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
*/
using UnityEditor;
using UnityEngine;

namespace HW.SrcGenDllConfigurationAutomator
{
    /// <summary>
    /// SourceGeneratorTargetFolderMarkerのインスペクター
    /// </summary>
    [CustomEditor(typeof(SourceGeneratorTargetFolderMarker))]
    internal class SourceGeneratorTargetFolderMarkerInspector : UnityEditor.Editor
    {
        /// <summary>
        /// 日本語のヘッダー文字列
        /// </summary>
        private const string HowToUseHeaderJapanese = "使用方法";
        /// <summary>
        /// 日本語の情報文字列
        /// </summary>
        private const string InfoTextJapanese =
            "マーカーアセットがあるフォルダー以下にあるdllが自動ラベル付与処理の対象になります\n" +
            "(また、dllにアセットラベル「" + SrcGenDllUtil.AvoidLabel + "」を付与すると処理の対象外にできます)";

        /// <summary>
        /// 英語のヘッダー文字列
        /// </summary>
        private const string HowToUseHeaderEnglish = "How to use";
        /// <summary>
        /// 英語の情報文字列
        /// </summary>
        private const string InfoTextEnglish =
            "This Automator targets dll in directory contains marker asset or in subfolders of that directory.\n" +
            "(If DLL has \"" + SrcGenDllUtil.AvoidLabel + "\" label, the automator will ignore that DLL)";

        /// <summary>
        /// 情報文字列の表示種別
        /// </summary>
        private const MessageType InfoType = MessageType.Info;
        /// <summary>
        /// 幅の広い情報文字列の表示を使用するか
        /// </summary>
        private const bool IsWideInfo = true;


        /// <summary>
        /// ヘッダー文字列のスタイル
        /// </summary>
        private static GUIStyle HeaderStyle => EditorStyles.boldLabel;


        /// <summary>
        /// インスペクターの描画処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            var headerStyle = HeaderStyle;

            EditorGUILayout.LabelField(HowToUseHeaderJapanese, headerStyle);
            EditorGUILayout.HelpBox(InfoTextJapanese, InfoType, IsWideInfo);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField(HowToUseHeaderEnglish, headerStyle);
            EditorGUILayout.HelpBox(InfoTextEnglish, InfoType, IsWideInfo);
        }
    }
}
