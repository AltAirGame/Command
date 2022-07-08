using System;
using UnityEngine;

namespace MHamidi
{
    public class PhysicalInputManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 2;
        }

        public static event Action QuitApplication;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CallQuit();
            }
        }

        private void CallQuit()
        {
            QuitApplication?.Invoke();
        }
    }
}