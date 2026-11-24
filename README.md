# SourceGenerator DLL Configuration Automator

## 概要 / Overview
マーカーアセットを配置したフォルダー以下のSourceGenerator用のdllにラベル付与とインポート設定を行う。  
This automator sets asset label and configures import settings to DLL in directory contains marker asset or in subfolders of that directory.


## 対応環境 / Requirements
2022.3.12f1以降(6000.6.10f1で動作確認済)  
2022.3.12f1 and later (developed in 6000.6.10f1)


## 使用方法 / English "How to use" is below this
SourceGenerator用のDLLがあるフォルダーにアセット作成メニューから「`SourceGenerator補助/dllフォルダーマーカー (DLL Folder Marker Asset)`」を作成すると、そのフォルダー以下のマネージドDLLにアセットラベルが付与されます。  
また、対象のフォルダーに入っているDLLに「`AvoidAutoSrcGenConfig`」ラベルが付与されている場合は処理されません。

## How to use / 日本語の「使用方法」は上にあります
Create "Marker Asset" from 「`SourceGenerator補助/dllフォルダーマーカー (DLL Folder Marker Asset)`」 at folder contains DLL for SourceGenerator.  
This automator sets asset label to dll in directory contains marker asset or in subfolders.  
If targeted DLL contains `AvoidAutoSrcGenConfig` label, this automator ignore that DLL.


## 導入方法 / English "How to introduction" is below this
1. Gitをインストールする
2. 追加したいプロジェクトを開き、Package Managerを開く
3. 以下の文字列をコピー、またはこのページ上部の「<> Code」からClone URLをコピーする

    https://github.com/HWataame/SrcGenDllConfigAutomator_Unity.git
4. 「Package Manager」ウィンドウの左上の「＋」ボタンをクリックし、「Install package from git URL...」を選択する

    ![導入方法2](https://github.com/user-attachments/assets/45fa456d-75ca-4189-b993-9819242bf05e)
5. 入力欄に手順2でコピーしたURLを貼り付け、「Install」ボタンを押す

## How to introduction / 日本語の「導入方法」は上にあります
1. Install Git to your computer.
2. Open Package Manager in the Unity project to which you want to add this feature.
3. Copy the following string, or copy the Clone URL from "<> Code" at the top of this page

    https://github.com/HWataame/SrcGenDllConfigAutomator_Unity.git
4. Click the "+" button in the "Package Manager" window and select "Install package from git URL...".

    ![導入方法2](https://github.com/user-attachments/assets/45fa456d-75ca-4189-b993-9819242bf05e)
5. Paste the URL copied in Step 2 into the input field and press the "Install" button.


## ライセンス / License
MITライセンスです / Using "MIT license"

[LISENCE](/LICENSE)
