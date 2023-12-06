using System;

namespace GameSystems.Core
{
    public interface IModalWindowData
    {
        public string Header { get; set; } 
        public string Message{ get; set; }
        public string OnClickOkayMessage{ get; set; }
        public string OnCancelMessage{ get; set; }
        public Action OnClickOkay{ get; set; }
        public Action OnCancel{ get; set; }
        public IUianimation UIAnimation{ get; set; }
        

    }
}