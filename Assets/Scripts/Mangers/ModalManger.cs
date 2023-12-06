using System;
using UnityEngine;


namespace GameSystems.Core
{
    public class ModalManger : MonoBehaviour
    {
        private IPoolService poolService;
        [SerializeField] private RectTransform modalMessageParent;

        private void OnEnable()
        {
            UiManager.UIManagerShowModal += UIManagerShowModal;
            UiManager.UIManagerShowModalString += UIManagerShowModal;
            UiManager.UIManagerShowModalData += UIManagerShowModal;
        }

        private void OnDisable()
        {
            UiManager.UIManagerShowModal -= UIManagerShowModal;
            UiManager.UIManagerShowModalString -= UIManagerShowModal;
            UiManager.UIManagerShowModalData -= UIManagerShowModal;
        }


        private void Start()
        {
            poolService = ServiceLocator.Instance.GetService<IPoolService>();
        }

        private void UIManagerShowModal()
        {
            var modalObject = poolService.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }

        private void UIManagerShowModal(string header, string message)
        {
            var modalObject = poolService.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }

        private void UIManagerShowModal(ModalWindowData data)
        {
            Debug.Log($"Pool is {(poolService is null ? "null" : "Not null")}");
            var modalObject = poolService.Get("Modal");
            Debug.Log($"Modal is {(modalObject is null ? "null" : "Not null")}");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent, false);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(data);
        }
    }
}