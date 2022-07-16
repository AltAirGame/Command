using System;
using UnityEngine;

namespace Mangers
{
    public class GameEventHandler : MonoBehaviour
    {

        public static event Action<bool> Rotate;
        public static event Func<ICommand,bool> IsAvailable;
        public static event Action<ICommand> Submit;
        

        public void OnRotate(bool isRight)
        {
            Rotate?.Invoke(isRight);
        }

        public bool OnIsAvailable(ICommand command)
        {
            return IsAvailable?.Invoke(command) ?? true;
        }

        public void  OnSubmit(ICommand command)
        {
             Submit?.Invoke(command);
        }
    }
}