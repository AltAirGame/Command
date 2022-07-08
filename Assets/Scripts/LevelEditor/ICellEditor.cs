using System;

namespace MHamidi
{
    public interface ICellEditor:IInteractable
    {
        
        public CellType type { get; set; }
        public bool IsStart { get; set; }
        public int CellLevelHeight { get; set; } 
        
        
        //Update Value is For Level Editor 

        void SetValue(int height,CellType Interactable);
        void SetAsStart();
        void IncreasHeight();
        void DecreasHeight();


    }
}