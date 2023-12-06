using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace GameSystems.Core
{
    public class ServiceLocator : MonoBehaviour, IServiceLocator
    {
        public Camera camera { get; private set; }
        public GameEventManager GameEventManager;

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private GameObject _servicesParent;

        private static ServiceLocator _instance;

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ServiceLocator>() ??
                                new GameObject("ServiceLocator").AddComponent<ServiceLocator>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            _servicesParent = GameObject.Find("ServicesParent") ?? new GameObject("ServicesParent");
        }

        public void RegisterService<TInterface, TClass>(TClass service) where TInterface : class
            where TClass : MonoBehaviour, TInterface
        {
            var interfaceType = typeof(TInterface);

            if (!_services.ContainsKey(interfaceType))
            {
                _services[interfaceType] = service;
                service.transform.SetParent(_servicesParent.transform);
            }
            else
            {
                Debug.LogWarning($"Service of type {interfaceType.Name} is already registered.");
            }
        }

        public TInterface GetService<TInterface>() where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            if (_services.TryGetValue(interfaceType, out var service))
            {
                return service as TInterface;
            }

            return CreateService<TInterface>();
        }

        private TInterface CreateService<TInterface>() where TInterface : class
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type interfaceType = typeof(TInterface);

            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetInterface(interfaceType.Name) == null || type.IsAbstract) continue;
                    var localComponent=gameObject.GetComponent(type);
                    if (localComponent is null)
                    {
                        var newService = gameObject.AddComponent(type) as TInterface;
                        _services[interfaceType] = newService;
                        return newService;    break;
                            

                    }
                    else
                    {
                        return localComponent as TInterface;
                    }
                }
            }

            Debug.LogWarning($"No Class has Implemented ({interfaceType.Name}) in this Assembly");
            return null;
        }


        public Camera GetCamera()
        {
            if (camera is null)
            {
                camera = Camera.main;
            }

            return camera;
        }


        public IEnumerator RunCoroutine(IEnumerator routine)
        {
            yield return StartCoroutine(routine);
        }

        
    }
}