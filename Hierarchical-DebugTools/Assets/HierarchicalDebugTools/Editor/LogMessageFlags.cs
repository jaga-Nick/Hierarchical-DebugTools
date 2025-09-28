// ソース: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/LogEntries.bindings.cs

using System;



namespace HierarchicalDebugTools {

    [Flags]
    public enum LogMessageFlags : int {

        kNoLogMessageFlags = 0,
        kError = 1 << 0, // メッセージはエラーを表します。
        kAssert = 1 << 1, // メッセージはアサーションの失敗を表します。
        kLog = 1 << 2, // メッセージは一般的なログメッセージです。
        kFatal = 1 << 4, // メッセージは致命的なエラーを表し、プログラムは終了する必要があります。
        kAssetImportError = 1 << 6, // メッセージはアセットのインポート中に生成されたエラーを表します。
        kAssetImportWarning = 1 << 7, // メッセージはアセットのインポート中に生成された警告を表します。
        kScriptingError = 1 << 8, // メッセージはスクリプトコードによって生成されたエラーを表します。
        kScriptingWarning = 1 << 9, // メッセージはスクリプトコードによって生成された警告を表します。
        kScriptingLog = 1 << 10, // メッセージはスクリプトコードによって生成された一般的なログメッセージを表します。
        kScriptCompileError = 1 << 11, // メッセージはスクリプトコンパイラによって生成されたエラーを表します。
        kScriptCompileWarning = 1 << 12, // メッセージはスクリプトコンパイラによって生成された警告を表します。
        kStickyLog = 1 << 13, // メッセージは「スティッキー」であり、ユーザーが手動でコンソールウィンドウをクリアしても削除されません。
        kMayIgnoreLineNumber = 1 << 14, // スクリプティングランタイムは、ファイルと行の情報でログのコールスタックを注釈付けするのをスキップする必要があります。
        kReportBug = 1 << 15, // kFatalと一緒に使用すると、ログシステムがバグレポーターを起動することを示します。
        kDisplayPreviousErrorInStatusBar = 1 << 16, // このメッセージの前のメッセージは、その前にメッセージがない限り、Unityのメインウィンドウの下部に表示される必要があります。
        kScriptingException = 1 << 17, // メッセージはスクリプトコードによって生成された例外を表します。
        kDontExtractStacktrace = 1 << 18, // このメッセージではスタックトレースの抽出をスキップする必要があります。
        kScriptingAssertion = 1 << 21, // メッセージはスクリプトコードでのアサーションの失敗を表します。
        kStacktraceIsPostprocessed = 1 << 22, // スタックトレースはすでに後処理されており、さらなる処理は必要ありません。
        kIsCalledFromManaged = 1 << 23, // メッセージはマネージドコードから呼び出されています。

        FromEditor = kDontExtractStacktrace | kMayIgnoreLineNumber | kIsCalledFromManaged,

        DebugLog = kScriptingLog | FromEditor,
        DebugWarning = kScriptingWarning | FromEditor,
        DebugError = kScriptingError | FromEditor,
        DebugException = kScriptingException | FromEditor,
        DebugAssert = kScriptingAssertion | FromEditor
        
    }



    public static class LogMessageFlagsExtensions {



        public static bool IsInfo(this LogMessageFlags flags) {
            return (flags & (LogMessageFlags.kLog | LogMessageFlags.kScriptingLog)) != 0;
        }



        public static bool IsWarning(this LogMessageFlags flags) {
            return (flags & (LogMessageFlags.kScriptCompileWarning | LogMessageFlags.kScriptingWarning | LogMessageFlags.kAssetImportWarning)) != 0;
        }



        public static bool IsError(this LogMessageFlags flags) {
            return (flags & (LogMessageFlags.kFatal | LogMessageFlags.kAssert | LogMessageFlags.kError | LogMessageFlags.kScriptCompileError |
                            LogMessageFlags.kScriptingError | LogMessageFlags.kAssetImportError | LogMessageFlags.kScriptingAssertion | LogMessageFlags.kScriptingException)) != 0;
        }



    }

}