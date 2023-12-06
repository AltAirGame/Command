namespace GameSystems.Core
{
    public interface ILevelEditorGridManager
    {
        public ICellEditService[,] grid { get; set; }
        
        void CreateGrid(int width, int height, float margin);
        void UpdateGrid(CellLayout[,] grid);
        void ClearGrid();
    }
}