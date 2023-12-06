using UnityEngine;

namespace GameSystems.Core
{
    public class LevelEditorGridManager : ILevelEditorGridManager
    {
        public ICellEditService[,] grid { get; set; }
        public Vector2Int StartPos = Vector2Int.zero;
        private IPoolService poolService; // Assuming you have a pooling service
        public CellLayout[,] gridLayout;
        private RectTransform parentRectTransform;
        private float cellMargin;
        private CellLayout[,] _gridLayout;

        public LevelEditorGridManager(IPoolService poolService, RectTransform parentRectTransform)
        {
            this.poolService = poolService;
            this.parentRectTransform = parentRectTransform;
        }


        

        public void CreateGrid(int width, int height, float margin)
        {
            this.cellMargin = margin;
            grid = new ICellEditService[width, height];
            var parentSize = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);
            var cellSizeX = parentSize.x / width;
            var cellSizeY = parentSize.y / height;
            var halfCellX = cellSizeX / 2f;
            var halfCellY = cellSizeY / 2f;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var newCell = poolService.Get("EditorCell");
                    newCell.gameObject.name = $" [{i}] ,[{j}]";
                    newCell.SetActive(true);
                    newCell.transform.SetParent(parentRectTransform, false);
                    RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(cellSizeX - margin / 2, cellSizeY - margin / 2);
                    rectTransform.anchoredPosition =
                        new Vector2((i * cellSizeX) + halfCellX, (j * cellSizeY) + halfCellY);
                    grid[i, j] = newCell.GetComponent<ICellEditService>();
                    
                }
            }
        }


        public void UpdateGrid(CellLayout[,] newGridLayout)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    // Assuming grid[i, j] and newGridLayout[i, j] can be mapped to each other.
                    // Update the cell state based on newGridLayout.
                    if (grid[i, j] != null && newGridLayout[i, j] != null)
                    {
                        grid[i, j].UpdateCellState(newGridLayout[i, j].cellHeight, newGridLayout[i, j].Type);

                        if (newGridLayout[i, j].IsStart)
                        {
                            StartPos = new Vector2Int(i, j);
                        }
                    }
                }
            }
        }


        public void ClearGrid()
        {
            // Logic to clear the grid
        }

        // Add any additional methods or logic as required for your grid management...
    }
}