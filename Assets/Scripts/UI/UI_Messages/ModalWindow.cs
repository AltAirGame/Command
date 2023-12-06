using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystems.Core
{
    public class ModalWindow : MonoBehaviour,IPointerClickHandler
    {
        //Data
        private ModalWindowData data;
        //View 
        [SerializeField] private GameObject MessageBox;
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
            // var OnClickOkayAction = data.OnClickOkay is null
            //     ? () => { Hide();}
            //     : data.OnClickOkay;
            Action OnClickOkayAction = ()=>
            {
                data.OnClickOkay?.Invoke();
                
            };

            OnCLickOkay.onClick.AddListener(() => {OnClickOkayAction?.Invoke(); });
            Action OnClickCancelAction = ()=>
            {
                data.OnCancel?.Invoke();
                
            };
            OnCLickOkay.onClick.AddListener(() =>
            {
                OnClickCancelAction?.Invoke(); 
                Hide();
            });
            OnClickCancel.onClick.AddListener(() =>
            {
                OnClickCancelAction?.Invoke();
                Hide();
            });
            Show();
        }

        private void Show()
        {
            
            data.UIAnimation.Show(MessageBox,null);
        }

        private void Hide()
        {
            
            data.UIAnimation.Hide(MessageBox,null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Some Time We Need to Hide the Modal On Click
        }
    }
}