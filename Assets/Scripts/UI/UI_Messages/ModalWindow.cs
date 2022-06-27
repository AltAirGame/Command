using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MHamidi.UI.UI_Messages
{
    public interface IUianimation
    {
        public void Show(GameObject Subject, Action OnShow);
        public void Hide(GameObject Subject, Action OnHide);
    }

    public class SimpleHideAndShow:IUianimation
    {
        public void Show(GameObject Subject, Action OnShow)
        {
            Subject.SetActive(true);
            OnShow?.Invoke();
        }
        public void Hide(GameObject Subject, Action OnHide)
        {
            Subject.SetActive(false);
            OnHide?.Invoke();
        }
    }
    public class SlidInOut:IUianimation
    {
        public void Show(GameObject Subject, Action OnShow)
        {   
            var startPos = new Vector3(.5f * Screen.width, -1.5f * Screen.height, 0);
            var targetPos = new Vector3(.5f * Screen.width, .5f * Screen.height, 0);
            Subject.transform.position = startPos;
           
            Subject.transform.DOMove(targetPos, .5f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                
                OnShow?.Invoke();
            });

       
        }
        public void Hide(GameObject Subject, Action OnHide)
        {
            
            var startPos = Subject.transform.position;
            var targetPos =new Vector3(.5f * Screen.width, -1.5f * Screen.height, 0);
            Subject.transform.DOMove(targetPos, .5f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
               
                OnHide?.Invoke();
                Subject.SetActive(false);
            });
           
            
        }
    }

    public class ModalWindowData
    {
        public string Header;
        public string Message;
        public string OnClickOkayMessage;
        public string OnCancelMessage;
        public Action OnClickOkay;
        public Action OnCancel;
        public IUianimation UIAnimation;

        public ModalWindowData()
        {
         Header=$"Default Header";
         Message=$"Default Message";
         OnClickOkayMessage="Okay";
         OnCancelMessage=$"Cancel";
         OnClickOkay=null;
         OnCancel=null;
         UIAnimation=null;
        }
        
        public ModalWindowData(string header, string message, string onClickOkayMessage, string onCancelMessage,
            IUianimation uiAnimation, Action onClickOkay = null, Action onCancel = null)
        {
            this.Header = header;
            this.Message = message;
            this.OnClickOkayMessage = onClickOkayMessage;
            this.OnCancelMessage = onCancelMessage;
            this.UIAnimation = uiAnimation;
            this.OnClickOkay = onClickOkay;
            this.OnCancel = onCancel;
        }  
        public ModalWindowData(string header, string message,
            IUianimation uiAnimation, Action onClickOkay = null)
        {
            this.Header = header;
            this.Message = message;
        
            this.UIAnimation = uiAnimation;
            this.OnClickOkay = onClickOkay;
        }
    }

    public class ModalWindow : MonoBehaviour,IPointerClickHandler
    {
        //Data
        private ModalWindowData data;
        //View 
        [SerializeField]
        private TextMeshProUGUI headerText;
        [SerializeField]
        private TextMeshProUGUI messageText;
        [SerializeField]
        private TextMeshProUGUI onClickOkayText;
        [SerializeField]
        private TextMeshProUGUI onClickCancelText;
        [SerializeField]
        private Button OnCLickOkay;
        [SerializeField]
        private Button OnClickCancel;
        //Controller

        public void Setup(ModalWindowData data)
        {
            this.data = data;
            UpdateView();
        }
        private void UpdateView()
        {

            headerText.text = string.IsNullOrWhiteSpace(data.Header) ? string.Empty : data.Header;
            messageText.text = string.IsNullOrWhiteSpace(data.Message) ? $"Some thing Went Wrong" : data.Message;
            onClickOkayText.text = string.IsNullOrWhiteSpace(data.Header) ?$"Okay"  : data.OnClickOkayMessage;
            onClickCancelText.text = string.IsNullOrWhiteSpace(data.Header) ? $" Cancel" : data.OnCancelMessage;
            OnCLickOkay.onClick.RemoveAllListeners();
            OnClickCancel.onClick.RemoveAllListeners();
            var OnClickOkayAction = data.OnClickOkay is null
                ? () => { Hide();}
                : data.OnClickOkay;
            OnCLickOkay.onClick.AddListener(() => {OnClickOkayAction?.Invoke(); });
            var OnClickCancelAction = data.OnCancel is null
                ? () => { Hide();}
                : data.OnCancel;
            OnCLickOkay.onClick.AddListener(() => {OnClickCancelAction?.Invoke(); });
            OnClickCancel.onClick.AddListener(() => { OnClickCancelAction?.Invoke();});
            Show();
        }

        public void Show()
        {
            if (data is null||data.UIAnimation is null)
            {
                gameObject.SetActive(true);
                return;
            }
            data.UIAnimation.Show(this.gameObject,null);
        }

        private void Hide()
        {
            if (data is null||data.UIAnimation is null)
            {
                gameObject.SetActive(false);
                return;
            }
            data.UIAnimation.Hide(this.gameObject,null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Hide();
        }
    }
}