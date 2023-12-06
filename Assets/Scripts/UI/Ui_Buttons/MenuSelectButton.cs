using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameSystems.Core
{
    public class MenuSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler,
        IPointerUpHandler
    {
        [SerializeField] private float TimeSinceLastInteraction = 0;
        [SerializeField] private float timeTOGetIdel;

        public UnityEvent OnPointerEnterEvent;
        public UnityEvent OnPointerExitEvent;
        public UnityEvent OnClickEvent;

        private void Update()
        {
            TimeSinceLastInteraction += Time.deltaTime;
            if (TimeSinceLastInteraction > timeTOGetIdel)
            {
                GetComponent<IButtonAnimation>().Idle();
                TimeSinceLastInteraction = 0;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TimeSinceLastInteraction = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke();
            GetComponent<IButtonAnimation>().OnClick();
            TimeSinceLastInteraction = 0;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke();
            GetComponent<IButtonAnimation>().OnPointerExit();
            TimeSinceLastInteraction = 0;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GetComponent<IButtonAnimation>().OnPointerUp();
            TimeSinceLastInteraction = 0;
        }
    }
}