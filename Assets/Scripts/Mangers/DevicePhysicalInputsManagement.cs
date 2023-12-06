using System;
using UnityEngine;

namespace GameSystems.Core
{
    public class DevicePhysicalInputsManagement : MonoBehaviour, IInputManagementService
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 2;
        }

        public static event Action QuitApplication;

        public void OnQuit()
        {
            CallQuit();
            
        }

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