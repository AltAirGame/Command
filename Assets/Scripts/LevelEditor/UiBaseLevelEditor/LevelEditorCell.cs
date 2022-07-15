using System;
using DG.Tweening;
using MHamidi;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace MHamidi
{
    public enum CellType
    {   NoneInteractable,
        Interactable
     
    }

    public class LevelEditorCell : MonoBehaviour, ICellEditor
    {
        [SerializeField] private Image backGround;
        [SerializeField] private TextMeshProUGUI heightText;
        [SerializeField] private Image startIcon;
        [SerializeField] private ChangeColor lamp;


        public CellType type { get; set; }
        public bool IsStart { get; set; }


        public int CellLevelHeight { get; set; }


        public void Interact()
        {
            //I Removed some Code Here
        }

        public void InteractionTwo()
        {
            SetAsStart();
        }

        public void InteractionThree()
        {
            //I Removed some Code Here
        }


        private void Start()
        {
            CellLevelHeight = 0;
            type = CellType.NoneInteractable;
            UpdateView();
        }

        public void SetValue(CellType type)
        {
            this.type = type;
            UpdateView();
        }

        public void SetValue(int height, CellType isInteractable)
        {
            CellLevelHeight = height;
            type = isInteractable;
            UpdateView();
        }

        public void SetAsStart()
        {
            IsStart = !IsStart;
            var startIconDisplay = IsStart ? true : false;
            startIcon.gameObject.SetActive(startIconDisplay);
        }

        public void Clear()
        {
            type = CellType.NoneInteractable;
            IsStart = false;
            CellLevelHeight = 0;
            UpdateView();
        }

        public void ChangeLight()
        {
            switch (type)
            {
                case CellType.NoneInteractable:
                    type = CellType.Interactable;
                    break;
                case CellType.Interactable:
                    type = CellType.NoneInteractable;
                    break;
                default:
                    break;
            }
        }

        public void IncreasHeight()
        {
            CellLevelHeight += 1;
            UpdateView();
        }

        public void DecreasHeight()
        {
            if (CellLevelHeight <= 0)
            {
                CellLevelHeight = 0;
                UpdateView();
                return;
            }

            CellLevelHeight -= 1;
            UpdateView();
        }

        private void UpdateView()
        {
            if (lamp == null)
            {
                Util.ShowMessag($"the Icon Array is Empty or Null");
                return;
            }
            var startIconDisplay = IsStart ? true : false;
            startIcon.gameObject.SetActive(startIconDisplay);
            backGround.color = HeightToColor();
            heightText.text = CellLevelHeight.ToString();
            if (CellLevelHeight > 10)
            {
                heightText.color = Color.white;
            }
            else
            {
                heightText.color = Color.black;
            }

            transform.DOScale(.5f * Vector3.one, .2f).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
            });
            switch (type)
            {
                case CellType.Interactable:
                    lamp.TurnOn();
                    break;
                case CellType.NoneInteractable:
                    lamp.TurnOff();
                    break;
                default:
                    break;
            }
        }

        private Color HeightToColor()
        {
            var color = Color.white;

            color = new Color((float)1 - (CellLevelHeight * color.r / 10), 1 - (CellLevelHeight * color.g /10),
                1 - (CellLevelHeight * color.b / 10), 1);
            return color;
        }
    }
}