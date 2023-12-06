


namespace GameSystems.Core
{
    public interface ICellEditService:IInteractable
    {
        
        public CellType type { get; set; }
        public bool IsStart { get; set; }
        public int CellLevelHeight { get; set; } 
        
        
        //Update Value is For Level Editor 

        void SetValue(int height,CellType Interactable);
        void SetAsStart();
        void Clear();
        void IncreasHeight();
        void DecreasHeight();


        void UpdateCellState(int cellHeight, CellType cellType);
    }
}