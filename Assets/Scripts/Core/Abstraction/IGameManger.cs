using System;
using System.Collections.Generic;

namespace GameSystems.Core
{
    public interface IGameManger
    {

        event Action<List<string>> UpdatePlayerInput;
        event Action<string> UpdateLevelNameText;
        event Action<int, int, int> UpdateBufferUi;

        void NextLevel();
        void StartLevelZero();
        void StartLevel(Level level);
        Level GetNextLevel();
        void UpdateLevelBufferUi(int bufferSize, int p1Size, int p2Size);
        void UpdatePlayerInputUI(List<string> avilableCommand);
    }
}