using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace GameSystems.Core
{
    public class LevelEditorUIManager : ILevelEditorUIManager
    {
        public LevelEditorUIManager()
        {
            // Constructor logic if needed
        }

        public void PopulatePlayerDirectionDropdown(TMP_Dropdown dropdown, List<PlayerDirection> directions)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(directions.Select(direction => direction.ToString()).ToList());
        }

        public void PopulateLevelNameDropdown(TMP_Dropdown dropdown, List<string> levelNames)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(levelNames);
        }

        public void UpdateToggles(List<LevelEditorToggle> toggles, List<string> availableCommands)
        {
            foreach (var toggle in toggles)
            {
                toggle.toggle.isOn = availableCommands.Contains(toggle.Name);
            }
        }

        public void UpdateBuffers(DiscreteSlider bufferSizeSlider, DiscreteSlider p1SizeSlider, DiscreteSlider p2SizeSlider, Level currentLevel)
        {
            bufferSizeSlider.SetCurrentValue(currentLevel.maxBufferSize);
            p1SizeSlider.SetCurrentValue(currentLevel.maxP1Size);
            p2SizeSlider.SetCurrentValue(currentLevel.maxP2Size);
        }

        // Implement other methods as required...
    }
}