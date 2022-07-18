using System;
using System.Collections.Generic;
using System.Linq;
using MHamidi;
using MHamidi.Helper;
using MHamidi.UI.UI_Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singlton;

public interface IUiManager
{
    public void ShowMessage(ModalWindowData data);

}

public class UiManager : MonoBehaviour, IUiManager
{
    [SerializeField]
    private MenuController MenuController;
    public static event Action ShowModal;
    public static event Action<string, string> ShowModalString;
    public static event Action<ModalWindowData> ShowModalData;

    //-------------------
    [Header("--------------------------------PlayerInputParent---")] [SerializeField]
    private RectTransform playerInputParent;

    [Header("--------------------------------BufferParent---")] [SerializeField]
    private RectTransform BufferParrent;

  
    [Space(10)]
    private RectTransform mainBuffer;
    private RectTransform P1Buffer;
    private RectTransform P2Buffer;

    private List<GameObject> mainBufferButtons = new List<GameObject>();
    private List<GameObject> p1BufferButtons = new List<GameObject>();
    private List<GameObject> p2BufferButtons = new List<GameObject>();

    [Header("--------------------------------PlayButton---")] [SerializeField]
    private Button playButtton;

    [SerializeField] private TextMeshProUGUI playText;

   
    //------------ 
    private void OnEnable()
    {
        PhysicalInputManager.QuitApplication += ShowQuitingDialogue;
        GameManger.UpdatePlayerInput += ShowPlayrInput;
        GameManger.UpdateBufferUi += ShowBufferUi;
        CommandManger.UpdatePlay += UpdatePlayButton;
        CommandManger.ChangePlayButtonInteractivityStatus += ChangePlayButtonInterActivityStatus;
        CommandManger.AddToBuffer += AddToBufferUi;
        CommandManger.RemoveAtIndexofBuffer += RemoveFromBufferUi;
        GameManger.UpdateLevelNameText += UpdateLevelnameText;
        
    }

   


    private void OnDisable()
    {
        
        PhysicalInputManager.QuitApplication -= ShowQuitingDialogue;
        GameManger.UpdateLevelNameText -= UpdateLevelnameText;
        GameManger.UpdatePlayerInput -= ShowPlayrInput;
        GameManger.UpdateBufferUi-= ShowBufferUi;
        CommandManger.UpdatePlay -= UpdatePlayButton;
        CommandManger.ChangePlayButtonInteractivityStatus += ChangePlayButtonInterActivityStatus;
        CommandManger.AddToBuffer -= AddToBufferUi;
        CommandManger.RemoveAtIndexofBuffer -= RemoveFromBufferUi;
    }

    //-----------
    private void ShowQuitingDialogue()
    {
        ShowModalMessage(new ModalWindowData("Quit Game", "Are You Sure Want To Quit ?",
            " Yes!I Hate this Game", "No,My Mistake", new SlidInOut(),
            () => { Application.Quit(); }));
    }

    //------------

    #region ModalMessage

    private void ShowModalMessage()
    {
        ShowModal?.Invoke();
    }

    private void ShowModalMessage(string header, string message)
    {
        ShowModalString?.Invoke(header, message);
    }

    private void ShowModalMessage(ModalWindowData data)
    {
        ShowModalData?.Invoke(data);
    }

    public void ShowMessage(ModalWindowData data)
    {
        ShowModalData?.Invoke(data);
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
                item.transform.SetParent(Dipendency.Instance.Pool.GetGameObject().transform, false);
            }
        }

        void AddPlayerInput(List<string> avilableCommand)
        {
            foreach (var item in avilableCommand)
            {
                var buttonObject = Dipendency.Instance.Pool.Get("GameButton");
                buttonObject.transform.SetParent(playerInputParent, false);
                buttonObject.SetActive(true);
                var command = CommandFactory.GetCommand(item);
                var button = buttonObject.GetComponent<GameButton>();
                button.SetListener(() => { CommandManger.current.AddToCurrentBuffer(command); });
                var icon = Resources.Load<Sprite>(command.Name);
                button.SetIcon(icon);
                button.gameObject.name = command.Name;
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
            item.transform.SetParent(Dipendency.Instance.Pool.GetGameObject().transform, false);
        }

        mainBuffer = CreatBuffer(buffer, "Main", 0);
        P1Buffer = CreatBuffer(p1, "P1", 1);
        P2Buffer = CreatBuffer(p2, "P2", 2);

        RectTransform CreatBuffer(int buffer, string name, int commandBuffer)
        {
            if (buffer <= 0)
            {
                Util.ShowMessag($"the size was 0");
                return null;
            }

            var bufferObject = Dipendency.Instance.Pool.Get("Buffer");
            bufferObject.SetActive(true);
            bufferObject.transform.SetParent(BufferParrent, false);
            var tempBuffer = bufferObject.GetComponent<BufferUI>();
            tempBuffer.SetText(name);
            tempBuffer.SetSize(buffer);
            tempBuffer.SetOnClick(() => { CommandManger.current.SetCurrentBuffer(commandBuffer); });
            return tempBuffer.transform.Find("Buffer").GetComponent<RectTransform>();
        }
    }

    private void AddToBufferUi(int index, ICommand command)
    {
        if (index == 0)
        {
            var buttonObject = Dipendency.Instance.Pool.Get("GameButton");
            mainBufferButtons.Add(buttonObject);
            buttonObject.transform.SetParent(mainBuffer, false);
            buttonObject.SetActive(true);
            var button = buttonObject.GetComponent<GameButton>();
            button.SetListener(() =>
            {
                CommandManger.current.RemoveFromBuffer(index, command,button.gameObject.GetInstanceID());
            });
            var icon = Resources.Load<Sprite>(command.Name);
            button.SetIcon(icon);
            button.gameObject.name = command.Name;
        }
        else if (index == 1)
        {
            var buttonObject = Dipendency.Instance.Pool.Get("GameButton");
            p1BufferButtons.Add(buttonObject);
            buttonObject.transform.SetParent(P1Buffer, false);
            buttonObject.SetActive(true);
            var button = buttonObject.GetComponent<GameButton>();
            button.SetListener(() =>
            {
                CommandManger.current.RemoveFromBuffer(index, command,button.gameObject.GetInstanceID());
            });
            var icon = Resources.Load<Sprite>(command.Name);
            button.SetIcon(icon);
            button.gameObject.name = command.Name;
        }

        else if (index == 2)
        {
            var buttonObject = Dipendency.Instance.Pool.Get("GameButton");
            p2BufferButtons.Add(buttonObject);
            buttonObject.transform.SetParent(P2Buffer, false);
            buttonObject.SetActive(true);
            var button = buttonObject.GetComponent<GameButton>();
            button.SetListener(() =>
            {
                CommandManger.current.RemoveFromBuffer(index, command,button.gameObject.GetInstanceID());
            });
            var icon = Resources.Load<Sprite>(command.Name);
            button.SetIcon(icon);
            button.gameObject.name = command.Name;
        }
    }

    private void RemoveFromBufferUi(int index, int buttonindex,int Id)
    {
        if (index == 0)
        {
            var buffer = mainBufferButtons.Where(x => x.GetInstanceID() == Id).First();
            var indexofButton=mainBufferButtons.IndexOf(buffer);
            mainBufferButtons.RemoveAt(indexofButton);
            buffer.SetActive(false);
        }
        else if (index == 1)
        {
            var buffer = p1BufferButtons.Where(x => x.GetInstanceID() == Id).First();
            var indexofButton=p1BufferButtons.IndexOf(buffer);
            p1BufferButtons.RemoveAt(indexofButton);
            buffer.SetActive(false);
        }

        else if (index == 2)
        {
            var buffer = p2BufferButtons.Where(x => x.GetInstanceID() == Id).First();
            var indexofButton=p2BufferButtons.IndexOf(buffer);
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

    #endregion
  

  
    
    
    
    #region LevelSelection
    private void UpdateLevelnameText(string obj)
    {
        MenuController.UpdateLevelnameText(obj);
    }

    public void ShowLevels()
    {
        MenuController.ShowLevelMenu(Dipendency.Instance.DataManger.gameData.levels);
    }

    #endregion
}

public class GameInGameUiController:MonoBehaviour
{
    
}