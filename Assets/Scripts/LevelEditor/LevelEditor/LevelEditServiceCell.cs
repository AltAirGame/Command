
using DG.Tweening;

using TMPro;
using UnityEngine;

using Image = UnityEngine.UI.Image;

namespace GameSystems.Core
{
    public enum CellType
    {   NoneInteractable,
        Interactable
     
    }

    
    public class LevelEditServiceCell : MonoBehaviour, ICellEditService
    {
        [SerializeField] private Image backGround;
        [SerializeField] private TextMeshProUGUI heightText;
        [SerializeField] private Image startIcon;
        [SerializeField] private ChangeColor lamp;

        public CellType type { get; set; }
        public bool IsStart { get; set; }
        public int CellLevelHeight { get; set; }

        private void Start()
        {
            Clear(); // Initialize state
        }

        public void SetValue(int height, CellType cellType)
        {
            CellLevelHeight = height;
            type = cellType;
            UpdateView();
        }

        public void SetAsStart()
        {
            IsStart = !IsStart;
            startIcon.gameObject.SetActive(IsStart);
        }
        private void SetAsStart(bool isStart)
        {
            IsStart = isStart;
            startIcon.gameObject.SetActive(isStart);
        }

        public void Clear()
        {
            SetValue(0, CellType.NoneInteractable);
            SetAsStart(false); // Ensure start icon is not active
        }

        public void IncreasHeight()
        {
            CellLevelHeight = CellLevelHeight+1;
            UpdateView();
        }

        public void DecreasHeight()
        {
            CellLevelHeight = CellLevelHeight-1;
            UpdateView();
        }

        public void UpdateCellState(int cellHeight, CellType cellType)
        {
            SetValue(cellHeight, cellType);
            if (cellHeight > 10)
            {
                heightText.color = Color.white;
            }
            else
            {
                heightText.color = Color.black;
            }
            UpdateView();
        }

        private void UpdateView()
        {
            if (lamp == null)
            {
                Util.ShowMessage($"Lamp is not assigned.");
                return;
            }
            backGround.color = HeightToColor();
            heightText.text = CellLevelHeight.ToString();
            lamp.SetState(type == CellType.Interactable); // Assuming SetState handles the lamp's on/off state

            // Scale animation
            transform.DOScale(.5f * Vector3.one, .2f).OnComplete(() => transform.localScale = Vector3.one);
        }

        private Color HeightToColor()
        {
            float inverseHeightRatio = 1f - (CellLevelHeight / 10f);
            return new Color(inverseHeightRatio, inverseHeightRatio, inverseHeightRatio, 1);
        }

        public void Interact()
        {
            lamp.changeValue();
            UpdateView();
        }
        public void InteractionTwo()
        {
            SetAsStart();
        }
        public void InteractionThree()
        {
            // Example: Toggle the height of the cell
            if (CellLevelHeight < 5)
            {
                IncreasHeight();
            }
            else
            {
                DecreasHeight();
            }
        }


    
}

}