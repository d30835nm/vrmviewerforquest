# VrmViewerForQuest
**※Oculus Questの実機が届いていないため、Oculus GOでしか動作確認を行っていません。**

Oculus Quest(及びGo)向けのVRMビューワーサンプルです。

・DonwloadフォルダにあるVRMを読み込みし、一覧表示可能
　PCからファイル転送、Oculusブラウザからダウンロードしたものなどを利用できます。

・DonwloadフォルダにあるVRMにお帰り頂くことが可能
　Oculus標準アプリにダウンロードフォルダのVRMを操作する機能がないためアプリ内に用意しています。

・選択したVRMの表示
　VRMの表示を行います。負荷の関係上、表示は1人、VRMの裏待機（キャッシュ）数は3人としています。ポーズ変更や表情変更機能はまだついていません。

# 動作環境
Unity 2018.4.0f1 にて確認

# 使い方
プロジェクトを開いたらエラーが大量に表示されるため、下記の作業を行ってください。

## プラットフォームの設定
OculusQuest及びOculusGoを対象としているため、
対象のプラットフォームをAndroidに切り替えてください。

## UniVRMのインポート
UniVRM v0.51.0を利用しています。
下記のリリースページから「UniVRM-0.51.0_1b36.unitypackage」をダウンロードしてインポートしてください。
[](https://github.com/vrm-c/UniVRM/releases)

## Oculus Integrationのインポート
Oculus Integration　v1.36を利用しています。
Unity Asset StoreからOculus Integrationをインポートしてください。
[](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022)

下記の2つのフォルダは最低限インポートしてください。
（他のフォルダもインポートして問題ないですが、ビルド時間に大きく影響します。）
　Oculus\Spatializer
　Oculus\VR

# ライセンスについて
MITライセンスにてご利用ください。