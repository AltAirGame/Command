using System.Collections;

namespace GameSystems.Core
{
    public interface ILevelEditorCoroutineManager
    {
        IEnumerator UpdateBoard(Level currentLevel, ICellEditService[,] grid, int width, int height);
        IEnumerator ClearBoard(ICellEditService[,] grid, int width, int height);
        // Add other coroutine management methods as needed...
    }
}