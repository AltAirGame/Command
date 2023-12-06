using System;

namespace GameSystems.Core
{
    public class ModalWindowData:IModalWindowData
    {
        
        public string Header { get; set; }
        public string Message { get; set; }
        public string OnClickOkayMessage { get; set; }
        public string OnCancelMessage { get; set; }
        public Action OnClickOkay { get; set; }
        public Action OnCancel { get; set; }
        public IUianimation UIAnimation { get; set; }

        public ModalWindowData()
        {
            Header=$"Default Header";
            Message=$"Default Message";
            OnClickOkayMessage="Okay";
            OnCancelMessage=$"Cancel";
            OnClickOkay=null;
            OnCancel=null;
            UIAnimation = null;
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
}