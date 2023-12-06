
using UnityEngine.UI;
using UnityEngine;


namespace GameSystems.Core
{
    public class ChangeColor : MonoBehaviour
    {

        public Color defaultColor;

        public Color inteactedColor;

        public Image image;

        public bool on = false;

        // Start is called before the first frame update
        void Start()
        {
            image.color = defaultColor;
        }

        public void TurnOn()
        {
            on = true;
            UpdateColor();
        }

        public void TurnOff()
        {
            @on = false;
            UpdateColor();
        }

        public void changeValue()
        {

            on = !@on;
            UpdateColor();
        }

        private void UpdateColor()
        {
            image.color = on ? inteactedColor : defaultColor;
        }

        public void SetState(bool state)
        {
            if (state)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }
    }
}