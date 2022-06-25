using System;
using UnityEngine;

namespace MHamidi
{
    public class InputManager:MonoBehaviour
    {
        public static event Action QuitApplication;
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
                QuitApplication?.Invoke();
                
            }
            
        }
    }
}