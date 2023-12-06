using System.Collections.Generic;
using TMPro;

namespace GameSystems.Core
{
    public interface ILevelEditorUIManager
    {
        void PopulatePlayerDirectionDropdown(TMP_Dropdown dropdown, List<PlayerDirection> directions);
        void PopulateLevelNameDropdown(TMP_Dropdown dropdown, List<string> levelNames);
        void UpdateToggles(List<LevelEditorToggle> toggles, List<string> availableCommands);
        void UpdateBuffers(DiscreteSlider bufferSizeSlider, DiscreteSlider p1SizeSlider, DiscreteSlider p2SizeSlider, Level currentLevel);
        // Additional UI related methods...
    }
}