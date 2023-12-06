
using TMPro;
using UnityEngine;


namespace GameSystems.Core
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CatchLogs : MonoBehaviour
    {
        private TextMeshProUGUI text;

        private void OnEnable()
        {
            Application.logMessageReceived += UnityLogs;
            text = GetComponent<TextMeshProUGUI>();
            Util.Log += ShowText;
        }

        private void UnityLogs(string condition, string stacktrace, LogType type)
        {
            ShowText(condition);
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= UnityLogs;
            Util.Log -= ShowText;
        }

        private void ShowText(string Log)
        {
            text.text += Log;
        }
    }
}