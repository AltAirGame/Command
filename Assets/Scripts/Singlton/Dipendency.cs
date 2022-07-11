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

        private UiManager _uiManager;

        public UiManager UiManager
        {
            get { return _uiManager; }
        }

        private Pool _pool;

        public Pool Pool
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
        private GameManger _gameManger;

        public GameManger GameManger
        {
            get { return _gameManger; }
        }

         private CommandManger _commandManger;

        public CommandManger ComandManger
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
            _dataManger = GetComponent<DataManger>();
            _gameManger = GetComponent<GameManger>();
            _commandManger = GetComponent<CommandManger>();
            _levelManger = GetComponent<ILevelManger>();
            _pool = GetComponent<Pool>();
            _uiManager = GetComponent<UiManager>();
            _dataManger = GetComponent<IData>();
            Instance = this;
        }

        public IEnumerator RunCorutine(IEnumerator rutin)
        {
            yield return StartCoroutine(rutin);
            
        }
    }
}