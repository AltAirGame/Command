using System;
using System.Collections;

using MHamidi;
using MHamidi.Helper;
using UnityEngine;

namespace Utils.Singlton
{
    public class Dipendency : MonoBehaviour
    {

        public float x=0;
        public float y = 0;
        public float z = 0;



        private IPhysicalInputManager _input;
        public IPhysicalInputManager InputManager {
            get
            {
                return _input;
            }
        }
        private IUiManager _uiManager;

        public IUiManager UiManager
        {
            get { return _uiManager; }
        }

        private IPool _pool;

        public IPool Pool
        {
            get{
                return _pool;
            }
        }

        private IData _dataManger;

        public IData DataManger
        {
            get
            {
                return _dataManger;
            }
        }
        private IGameManger _gameManger;

        public IGameManger GameManger
        {
            get { return _gameManger; }
        }

         private ICommandManger _commandManger;

        public ICommandManger ComandManger
        {
            get { return _commandManger; }
        }

        private ILevelManger _levelManger;

        public ILevelManger LevelManger
        {
            get { return _levelManger; }
        }

     

       
        public static Dipendency Instance;

        private Camera _camera;
        public Camera Camera {
            get
            {
                return _camera;
            }
        }

        private void OnEnable()
        {
            _camera=Camera.main;
        }

        private void Awake()
        {
            _input = GetComponent<IPhysicalInputManager>();
            _dataManger = GetComponent<IData>();
            _gameManger = GetComponent<IGameManger>();
            _commandManger = GetComponent<ICommandManger>();
            _levelManger = GetComponent<ILevelManger>();
            _pool = GetComponent<IPool>();
            _uiManager = GetComponent<IUiManager>();
            _dataManger = GetComponent<IData>();
            Instance = this;
        }

        public IEnumerator RunCorutine(IEnumerator rutin)
        {
            yield return StartCoroutine(rutin);
            
        }
    }
}