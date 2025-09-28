using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;



namespace HierarchicalDebugTools {

    public static class ConsoleReader {



        public static ConsoleLog[] GetDebugMessages() {
            // ソース: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/LogEntries.bindings.cs

            // リフレクションを使用してUnityEditorアセンブリからLogEntriesクラスを取得
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditor.LogEntries");

            // コンソール上のログエントリ数を取得
            MethodInfo GetCount = logEntries.GetMethod("GetCount"); // このメソッドは、コンソール内のログエントリの数を取得するために使用
            int count = (int) GetCount.Invoke(null, null);

            // LogEntriesクラスから必要なメソッドとフィールドを取得
            MethodInfo GetEntryInternal = logEntries.GetMethod("GetEntryInternal"); // このメソッドは、特定のインデックスにあるログエントリを取得するために使用
            Type logEntryType = assembly.GetType("UnityEditor.LogEntry"); // これはログエントリオブジェクトの型（クラス）
            FieldInfo messageField = logEntryType.GetField("message"); // このフィールドには、ログエントリのメッセージが含まれています
            FieldInfo instanceIDField = logEntryType.GetField("instanceID"); // このフィールドには、ログエントリに関連付けられたGameObjectのインスタンスIDが含まれています。
            FieldInfo callstackIndexField = logEntryType.GetField("callstackTextStartUTF8"); // このフィールドには、ログエントリ内のコールスタックテキストの開始インデックスが含まれています。
            FieldInfo flagsField = logEntryType.GetField("mode"); // このフィールドには、ログエントリのフラグが含まれています。
            FieldInfo fileField = logEntryType.GetField("file"); // このフィールドには、ログエントリのファイル名が含まれています。
            FieldInfo lineField = logEntryType.GetField("line"); // このフィールドには、ログエントリの行番号が含まれています。
            FieldInfo columnField = logEntryType.GetField("column"); // このフィールドには、ログエントリの列番号が含まれています。

            object[] parameters = new object[2];
            ConsoleLog[] logs = new ConsoleLog[count];

            // すべてのログエントリをループ処理し、必要な情報を取得します。
            for (int i = 0; i < count; i++) {
                parameters[0] = i; // インデックス
                parameters[1] = Activator.CreateInstance(logEntryType); // ログエントリオブジェクト
                bool result = (bool) GetEntryInternal.Invoke(null, parameters);

                if (result) {
                    string fullMessage = (string) messageField.GetValue(parameters[1]);
                    int instanceID = (int) instanceIDField.GetValue(parameters[1]);
                    LogMessageFlags mode = (LogMessageFlags) flagsField.GetValue(parameters[1]);
                    int callstackTextStartIndex = (int)callstackIndexField.GetValue(parameters[1]);
                    string callstack = fullMessage[callstackTextStartIndex..].Trim(); // メッセージからコールスタックテキストを取得します。
                    string message = fullMessage[..callstackTextStartIndex]; // コールスタックテキストの前のメッセージを取得します。
                    string file = (string) fileField.GetValue(parameters[1]); // ログエントリからファイル名を取得します。
                    int line = (int) lineField.GetValue(parameters[1]); // ログエントリから行番号を取得します。
                    int column = (int) columnField.GetValue(parameters[1]); // ログエントリから列番号を取得します。

                    logs[i] = new ConsoleLog(i, instanceID, mode, message, callstack, file, line, column);
                }
            }

            return logs;
        }



        public static ConsoleLog[] FilterLogsByGameObject(ConsoleLog[] logs, GameObject targetObject) {
            List<ConsoleLog> filteredLogs = new();

            foreach (ConsoleLog log in logs) {
                UnityEngine.Object logObject = EditorUtility.InstanceIDToObject(log.InstanceID);
                if (logObject == null)
                    continue; // ログオブジェクトがnullの場合はスキップします。

                // ログオブジェクトがGameObjectで、ターゲットオブジェクトと一致する場合、フィルター済みログに追加します。
                if (logObject is GameObject && logObject.Equals(targetObject))
                    filteredLogs.Add(log);

                // ログオブジェクトがComponentで、そのGameObjectがターゲットオブジェクトと一致する場合、フィルター済みログに追加します。
                if (logObject is Component && ((Component)logObject).gameObject.Equals(targetObject))
                    filteredLogs.Add(log);
            }

            return filteredLogs.ToArray();
        }



        public static ConsoleLog[] FilterLogsByType(ConsoleLog[] logs, FilterType filterType) {
            if (filterType == FilterType.All || logs == null || logs.Length == 0)
                return logs;

            List<ConsoleLog> filteredLogs = new();

            foreach (ConsoleLog log in logs) {
                if (log.Mode.IsError() && filterType == FilterType.Error || log.Mode.IsWarning() && filterType == FilterType.Warning || log.Mode.IsInfo() && filterType == FilterType.Info) {
                    filteredLogs.Add(log);
                }
            }

            return filteredLogs.ToArray();
        }



        public static ConsoleLog[] FilterLogsBySearch(ConsoleLog[] logs, string searchString) {
            if (string.IsNullOrEmpty(searchString) || logs == null || logs.Length == 0)
                return logs;

            searchString = searchString.Trim(); // 検索文字列の先頭と末尾の空白を削除します。

            if (string.IsNullOrEmpty(searchString))
                return logs; // トリムされた検索文字列が空の場合、すべてのログを返します。

            List<ConsoleLog> filteredLogs = new();

            foreach (ConsoleLog log in logs) {
                if (log.Message.Contains(searchString, StringComparison.OrdinalIgnoreCase) || log.Callstack.Contains(searchString, StringComparison.OrdinalIgnoreCase) || log.File.Contains(searchString, StringComparison.OrdinalIgnoreCase)) {
                    filteredLogs.Add(log);
                }
            }

            return filteredLogs.ToArray();
        }



        public enum FilterType {
            All,
            Error,
            Warning,
            Info,
        }



    }

}