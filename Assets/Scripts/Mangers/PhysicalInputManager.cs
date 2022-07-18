using System;
using UnityEngine;

namespace MHamidi
{
    public interface IPhysicalInputManager
    { public void OnQuit()
        {
            
        }

    }

    public class PhysicalInputManager : MonoBehaviour, IPhysicalInputManager
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