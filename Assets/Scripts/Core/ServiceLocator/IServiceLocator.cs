using System.Collections;
using UnityEngine;

namespace GameSystems.Core
{
    public interface IServiceLocator
    {
        public Camera camera { get; }

        void RegisterService<TInterface, TClass>(TClass service) where TInterface : class
            where TClass : MonoBehaviour, TInterface;
        Camera GetCamera();
        TInterface GetService<TInterface>()  where TInterface : class;
        IEnumerator RunCoroutine(IEnumerator routine);
    }
}