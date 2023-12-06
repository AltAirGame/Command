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
            data.UIAnimation = new SlidInOut();
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

        private void Show()
        {
            if (data is null|data.UIAnimation is null)
            {
                gameObject.SetActive(true);
                return;
            }
            data.UIAnimation.Show(gameObject,null);
        }

        private void Hide()
        {
            if (data is null||data.UIAnimation is null)
            {
                gameObject.SetActive(false);
                return;
            }
            data.UIAnimation.Hide(gameObject,null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Some Time We Need to Hide the Modal On Click
        }
    }
}