using System.Collections;
using UnityEngine;

namespace GameSystems.Core
{
    public class LevelEditorCoroutineManager : ILevelEditorCoroutineManager
    {
        public IEnumerator UpdateBoard(Level currentLevel, ICellEditService[,] grid, int width, int height)
        {
            yield return ServiceLocator.Instance.RunCoroutine(ClearBoard(grid, width, height));

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Corrected access to the list of lists
                    var layout = currentLevel.LevelLayout[i][j];
                    grid[i, j].SetValue(layout.cellHeight, layout.Type);

                    if (currentLevel.startX == i && currentLevel.startY == j)
                    {
                        grid[i, j].SetAsStart();
                    }

                    // Assuming this is inside a coroutine since 'yield return' is used
                    yield return new WaitForSeconds(.05f);
                }
            }

        }

        public IEnumerator ClearBoard(ICellEditService[,] grid, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j].Clear();
                    yield return new WaitForSeconds(.05f);
                }
            }
        }

        // Additional coroutine methods as needed...
    }
}