using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

public static class LogTrace
{
    [UnityEditor.Callbacks.OnOpenAsset(0)]
    static bool OnOpenLogTrace(int instance, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instance).name;
        if (!name.Equals("DebugCustom")) return false;

        var assembly = Assembly.GetAssembly(typeof(EditorWindow));
        if (assembly == null) return false;

        var consoleWindowType = assembly.GetType("UnityEditor.ConsoleWindow");
        if (consoleWindowType == null) return false;

        var consoleWindowField = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        if (consoleWindowField == null) return false;

        var consoleWindowInstance = consoleWindowField.GetValue(null);
        if (consoleWindowInstance == null) return false;

        if (consoleWindowInstance != (object)EditorWindow.focusedWindow) return false;

        var activeTextField = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
        if (activeTextField == null) return false;

        string activeTextValue = activeTextField.GetValue(consoleWindowInstance).ToString();
        if (string.IsNullOrEmpty(activeTextValue)) return false;

        // 첫 번째 스택 트레이스를 건너 뜀
        Match match = Regex.Match(activeTextValue, @"\(at (.+)\)");
        if (match.Success) match = match.NextMatch();

        if (match.Success)
        {
            string path = match.Groups[1].Value;
            var split = path.Split(':');
            string filePath = split[0];
            int lineNum = Convert.ToInt32(split[1]);

            string dataPath = UnityEngine.Application.dataPath.Substring(0, UnityEngine.Application.dataPath.LastIndexOf("Assets"));
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(dataPath + filePath, lineNum);
            return true;
        }
        return false;
    }
}
