using System;
using MHamidi;
using MHamidi.UI.UI_Messages;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static event Action ShowModal;
    public static event Action<string,string> ShowModalString;
    public static event Action<ModalWindowData> ShowModalData;
    
    //------------
    private void OnEnable()
    {
        InputManager.QuitApplication += ShowQuitingDialogue;
    }
    private void OnDisable()
    {
        InputManager.QuitApplication -= ShowQuitingDialogue;
    }
    //-----------
    private void ShowQuitingDialogue()
    {
        ShowModalMessage(new ModalWindowData("Quit Game","Are You Sure Want To Quit ?",
            " Yes!I Hate this Game","No,My Mistake",new SimpleHideAndShow(),
            () => {Application.Quit();}));
    }
    //------------
    private void ShowModalMessage()
    {
        ShowModal?.Invoke();
    }
    private void ShowModalMessage(string header, string message)
    {
        ShowModalString?.Invoke(header,message);
    }
    private void ShowModalMessage(ModalWindowData data)
    {
        ShowModalData?.Invoke(data);
    }
    
    
    
}