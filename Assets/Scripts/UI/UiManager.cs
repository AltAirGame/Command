using System;
using System.Collections.Generic;
using MHamidi;
using MHamidi.Helper;
using MHamidi.UI.UI_Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static event Action ShowModal;
    public static event Action<string, string> ShowModalString;
    public static event Action<ModalWindowData> ShowModalData;
    
    //-------------------
    [Header("--------------------------------PlayerInputParent---")] [SerializeField]
    private RectTransform playerInputParent;
    [Header("--------------------------------BufferParent---")] [SerializeField]
    private RectTransform BufferParrent;

    [Header("--------------------------------PlayButton---")]
    [SerializeField] private Button playButtton;
    [SerializeField] private TextMeshProUGUI playText;
    //------------ 
    private void OnEnable()
    {
        PhysicalInputManager.QuitApplication += ShowQuitingDialogue;
        GameManger.UpdatePlayerInput += ShowPlayrInput;
        GameManger.UpdateBufferUi += ShowBufferUi;
        CommandManger.UpdatePlay += UpdatePlayButton;
        CommandManger.ChangePlayButtonInteractivityStatus += ChangePlayButtonInterActivityStatus;
    }

   

    private void OnDisable()
    {
        PhysicalInputManager.QuitApplication -= ShowQuitingDialogue;
        GameManger.UpdatePlayerInput -= ShowPlayrInput;
        CommandManger.UpdatePlay -= UpdatePlayButton;
        CommandManger.ChangePlayButtonInteractivityStatus -= ChangePlayButtonInterActivityStatus;
    }
    
    //-----------
    private void ShowQuitingDialogue()
    {
        ShowModalMessage(new ModalWindowData("Quit Game", "Are You Sure Want To Quit ?",
            " Yes!I Hate this Game", "No,My Mistake", new SlidInOut(),
            () => { Application.Quit(); }));
    }

    //------------
    
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

    private void ShowPlayrInput(List<int> avilableCommand)
    {
        ClearAllChildren();
        AddPlayerInput(avilableCommand);

        #region Functions

        

     
        void ClearAllChildren()
        {
            foreach (var item in playerInputParent.GetComponentsInChildren<GameButton>())
            {
                item.gameObject.SetActive(false);
                item.transform.SetParent(Pool.Instance.transform, false);
            }
        }
        void AddPlayerInput(List<int> avilableCommand)
        {
            foreach (var item in avilableCommand)
            {
                var buttonObject = Pool.Instance.Get("GameButton");
                buttonObject.transform.SetParent(playerInputParent, false);
                buttonObject.SetActive(true);
                var command = CommandManger.current.commandLookUpTable[item];
                var button = buttonObject.GetComponent<GameButton>();
                button.SetListener(() => { CommandManger.current.AddToCurrentBuffer(command); });
                var icon = Resources.Load<Sprite>(command.name);
                button.SetIcon(icon);
                button.gameObject.name = command.name;
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
            item.transform.SetParent(Pool.Instance.transform,false);
        }

        CreatBuffer(buffer,"Main",CommandManger.current.MainCommand);
        CreatBuffer(p1,"P1",CommandManger.current.MainCommand);
        CreatBuffer(p2,"P2",CommandManger.current.p2Command);
        
        void CreatBuffer(int buffer,string name,List<ICommand> commandBuffer)
        { if (buffer<=0)
            {
                Util.ShowMessag($"the size was 0");
                return;
                
            }
            var bufferObject = Pool.Instance.Get("Buffer");
            bufferObject.SetActive(true);
           
            bufferObject.transform.SetParent(BufferParrent, false);
            var mainBuffer = bufferObject.GetComponent<BufferUI>();
            mainBuffer.SetText(name);
            mainBuffer.SetSize(buffer);
            mainBuffer.SetOnClick(() => { CommandManger.current.SetCurrentBuffer(commandBuffer); });
        }
    }

   

    private void UpdatePlayButton(Action onClick,string text)
    {
        playButtton.onClick.RemoveAllListeners();
        playButtton.onClick.AddListener(() => { onClick?.Invoke();});
        playText.text = text;
    }

    private void ChangePlayButtonInterActivityStatus()
    {
        playButtton.interactable = !playButtton.interactable;
    }

}