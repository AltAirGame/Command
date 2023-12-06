using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameSystems.Core
{
    public class UiManager : MonoBehaviour, IUiManager
    {
        private event Action UIManagerIsInitialized;
        public static event Action UIManagerShowModal;
        public static event Action<string, string> UIManagerShowModalString;
        public static event Action<ModalWindowData> UIManagerShowModalData;

        
        
        
        // ... other methods ...
        [SerializeField] private MenuController MenuController;

        //-------------------
        [Header("--------------------------------PlayerInputParent---")] [SerializeField]
        private RectTransform playerInputParent;

        [Header("--------------------------------BufferParent---")] [SerializeField]
        private RectTransform BufferParrent;


        [Space(10)] private RectTransform mainBuffer;
        private RectTransform P1Buffer;
        private RectTransform P2Buffer;


        private List<GameObject> mainBufferButtons = new List<GameObject>();
        private List<GameObject> p1BufferButtons = new List<GameObject>();
        private List<GameObject> p2BufferButtons = new List<GameObject>();

        [Header("--------------------------------PlayButton---")] [SerializeField]
        private Button playButtton;

        [SerializeField] private TextMeshProUGUI playText;


        private RectTransform p1Buffer;
        private RectTransform p2Buffer;

        private IPoolService poolService;
        private IDataManagementService dataManagerService;
        private IAssetLoaderService assetLoaderService;
        private IGameManger gameManger;
        
        private void Start()
        {
            gameManger = ServiceLocator.Instance.GetService<IGameManger>();
            poolService = ServiceLocator.Instance.GetService<IPoolService>();
            dataManagerService = ServiceLocator.Instance.GetService<IDataManagementService>();
            assetLoaderService = ServiceLocator.Instance.GetService<IAssetLoaderService>();
            UIManagerIsInitialized?.Invoke();
        }

        private void OnEnable()
        {
            UIManagerIsInitialized += SubscribeEvents;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #region Event Subscriptions

        private void SubscribeEvents()
        {
            DevicePhysicalInputsManagement.QuitApplication += ShowQuittingDialogue;
            gameManger.UpdatePlayerInput += ShowPlayerInput;
            gameManger.UpdateBufferUi += ShowBufferUi;
            CommandMangmentService.UpdatePlay += UpdatePlayButton;
            CommandMangmentService.ChangePlayButtonInteractivityStatus += ChangePlayButtonInterActivityStatus;
            CommandMangmentService.AddToBuffer += AddToBufferUi;
            CommandMangmentService.RemoveAtIndexofBuffer += RemoveFromBufferUi;
            gameManger.UpdateLevelNameText += UpdateLevelnameText;
        }

        private void UnsubscribeEvents()
        {
            DevicePhysicalInputsManagement.QuitApplication -= ShowQuittingDialogue;
            gameManger.UpdateLevelNameText -= UpdateLevelnameText;
            gameManger.UpdatePlayerInput -= ShowPlayrInput;
            gameManger.UpdateBufferUi -= ShowBufferUi;
            CommandMangmentService.UpdatePlay -= UpdatePlayButton;
            CommandMangmentService.ChangePlayButtonInteractivityStatus += ChangePlayButtonInterActivityStatus;
            CommandMangmentService.AddToBuffer -= AddToBufferUi;
            CommandMangmentService.RemoveAtIndexofBuffer -= RemoveFromBufferUi;
        }

        #endregion

        #region Modal Messages

        public void ShowMessage(ModalWindowData data)
        {
            UIManagerShowModalData?.Invoke(data);
        }

        private void ShowModalMessage()
        {
            UIManagerShowModal?.Invoke();
        }

        private void ShowModalMessage(string header, string message)
        {
            UIManagerShowModalString?.Invoke(header, message);
        }

        public void ShowModalMessage(ModalWindowData data)
        {
            UIManagerShowModalData?.Invoke(data);
        }

        private void ShowQuittingDialogue()
        {
            ShowModalMessage(new ModalWindowData("Quit Game", "Are You Sure Want To Quit ?",
                " Yes!I Hate this Game", "No,My Mistake", new SlidInOut(),
                () => { Application.Quit(); }));

        }

        // ... other methods related to modal messages ...

        #endregion

        #region In-Game UI

        private void ShowPlayerInput(List<string> availableCommands)
        {
            ClearAllChildren();
            AddPlayerInput(availableCommands);

            #region Functions

            void ClearAllChildren()
            {
                foreach (var item in playerInputParent.GetComponentsInChildren<GameButton>())
                {
                    item.gameObject.SetActive(false);
                    item.transform.SetParent(
                        ServiceLocator.Instance.GetService<IPoolService>().GetGameObject().transform,
                        false);
                }
            }

            void AddPlayerInput(List<string> avilableCommand)
            {
                // Debug.Log(availableCommands.);
                foreach (var item in avilableCommand)
                {
                    

                    var buttonObject = poolService.Get("GameButton");
                    buttonObject.transform.SetParent(playerInputParent, false);
                    buttonObject.SetActive(true);
                    var command = CommandFactory.GetCommand(item);
                    Debug.Log($"command: {command.Name}");
                    var button = buttonObject.GetComponent<GameButton>();
                    button.SetListener(() => { CommandMangmentService.current.AddToCurrentBuffer(command); });

                    Debug.Log($"Button Object: {buttonObject}");
                    Debug.Log($"Pool Service: {poolService}");
                    Debug.Log($"AssetLoader Service: {assetLoaderService}");

                    assetLoaderService.LoadAddressableAsset<Sprite>($"icons/{command.Name.ToLower()}", (icon) =>
                    {
                        button.SetIcon(icon);
                        button.gameObject.name = command.Name;
                    });
                }
            }

            #endregion
        }

        private void ShowBufferUi(int buffer, int p1, int p2)
        {
            var ActiveBuffers = BufferParrent.GetComponentsInChildren<BufferUI>();
            foreach (var item in ActiveBuffers)
            {
                item.gameObject.SetActive(false);
                item.transform.SetParent(poolService.GetGameObject().transform, false);
            }

            mainBuffer = CreatBuffer(buffer, "Main", 0);
            P1Buffer = CreatBuffer(p1, "P1", 1);
            P2Buffer = CreatBuffer(p2, "P2", 2);

            RectTransform CreatBuffer(int buffer, string name, int commandBuffer)
            {
                if (buffer <= 0)
                {
                    Util.ShowMessage($"the size was 0");
                    return null;
                }

                var bufferObject = poolService.Get("Buffer");
                bufferObject.SetActive(true);
                bufferObject.transform.SetParent(BufferParrent, false);
                var tempBuffer = bufferObject.GetComponent<BufferUI>();
                tempBuffer.SetText(name);
                tempBuffer.SetSize(buffer);
                tempBuffer.SetOnClick(() => { CommandMangmentService.current.SetCurrentBuffer(commandBuffer); });
                return tempBuffer.transform.Find("Buffer").GetComponent<RectTransform>();
            }
        }

        private void AddToBufferUi(int index, ICommand command)
        {
            if (index == 0)
            {
                var buttonObject = poolService.Get("GameButton");
                mainBufferButtons.Add(buttonObject);
                buttonObject.transform.SetParent(mainBuffer, false);
                buttonObject.SetActive(true);
                var button = buttonObject.GetComponent<GameButton>();
                button.SetListener(() =>
                {
                    CommandMangmentService.current.RemoveFromBuffer(index, command,
                        button.gameObject.GetInstanceID());
                });
                var icon = Resources.Load<Sprite>(command.Name);
                button.SetIcon(icon);
                button.gameObject.name = command.Name;
            }
            else if (index == 1)
            {
                var buttonObject = poolService.Get("GameButton");
                p1BufferButtons.Add(buttonObject);
                buttonObject.transform.SetParent(P1Buffer, false);
                buttonObject.SetActive(true);
                var button = buttonObject.GetComponent<GameButton>();
                button.SetListener(() =>
                {
                    CommandMangmentService.current.RemoveFromBuffer(index, command,
                        button.gameObject.GetInstanceID());
                });
                var icon = Resources.Load<Sprite>(command.Name);
                button.SetIcon(icon);
                button.gameObject.name = command.Name;
            }

            else if (index == 2)
            {
                var buttonObject = poolService.Get("GameButton");
                p2BufferButtons.Add(buttonObject);
                buttonObject.transform.SetParent(P2Buffer, false);
                buttonObject.SetActive(true);
                var button = buttonObject.GetComponent<GameButton>();
                button.SetListener(() =>
                {
                    CommandMangmentService.current.RemoveFromBuffer(index, command,
                        button.gameObject.GetInstanceID());
                });
                var icon = Resources.Load<Sprite>(command.Name);
                button.SetIcon(icon);
                button.gameObject.name = command.Name;
            }
        }

        private void RemoveFromBufferUi(int index, int buttonindex, int Id)
        {
            if (index == 0)
            {
                var buffer = mainBufferButtons.Where(x => x.GetInstanceID() == Id).First();
                var indexofButton = mainBufferButtons.IndexOf(buffer);
                mainBufferButtons.RemoveAt(indexofButton);
                buffer.SetActive(false);
            }
            else if (index == 1)
            {
                var buffer = p1BufferButtons.Where(x => x.GetInstanceID() == Id).First();
                var indexofButton = p1BufferButtons.IndexOf(buffer);
                p1BufferButtons.RemoveAt(indexofButton);
                buffer.SetActive(false);
            }

            else if (index == 2)
            {
                var buffer = p2BufferButtons.Where(x => x.GetInstanceID() == Id).First();
                var indexofButton = p2BufferButtons.IndexOf(buffer);
                p2BufferButtons.RemoveAt(indexofButton);
                buffer.SetActive(false);
            }
        }

        private void UpdatePlayButton(Action onClick, string text)
        {
            playButtton.onClick.RemoveAllListeners();
            playButtton.onClick.AddListener(() => { onClick?.Invoke(); });
            playText.text = text;
        }

        private void ChangePlayButtonInterActivityStatus()
        {
            playButtton.interactable = !playButtton.interactable;
        }


        // ... other methods related to in-game UI ...

        #endregion

        #region Level Selection

        private void UpdateLevelnameText(string obj)
        {
            MenuController.UpdateLevelnameText(obj);
        }

        public void ShowLevels()
        {
            if (dataManagerService is null)
            {
                Debug.Log("Data manager Service is Null");
            }
            else
            {
                Debug.Log($"MenuController is {(MenuController == null ? "not " : "")}set");
                Debug.Log($"DataManagerService is {(dataManagerService == null ? "not " : "")}set");

                MenuController.ShowLevelMenu(dataManagerService.LevelData.levels);
            }
        }

        #endregion


        #region InGameUi

        private void ShowPlayrInput(List<string> avilableCommand)
        {
            ClearAllChildren();
            AddPlayerInput(avilableCommand);

            #region Functions

            void ClearAllChildren()
            {
                foreach (var item in playerInputParent.GetComponentsInChildren<GameButton>())
                {
                    item.gameObject.SetActive(false);
                    item.transform.SetParent(
                        ServiceLocator.Instance.GetService<IPoolService>().GetGameObject().transform,
                        false);
                }
            }

            void AddPlayerInput(List<string> avilableCommand)
            {
                foreach (var item in avilableCommand)
                {
                    var buttonObject = poolService.Get("GameButton");
                    buttonObject.transform.SetParent(playerInputParent, false);
                    buttonObject.SetActive(true);
                    var command = CommandFactory.GetCommand(item);
                    var button = buttonObject.GetComponent<GameButton>();
                    button.SetListener(() => { CommandMangmentService.current.AddToCurrentBuffer(command); });
                    var icon = Resources.Load<Sprite>(command.Name);
                    button.SetIcon(icon);
                    button.gameObject.name = command.Name;
                }
            }

            #endregion
        }

        #endregion
    }
}