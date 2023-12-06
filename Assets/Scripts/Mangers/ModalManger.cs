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
            UiManager.ShowModal += ShowModal;
            UiManager.ShowModalString += ShowModal;
            UiManager.ShowModalData += ShowModal;
        }

        private void OnDisable()
        {
            UiManager.ShowModal -= ShowModal;
            UiManager.ShowModalString -= ShowModal;
            UiManager.ShowModalData -= ShowModal;
        }


        private void Start()
        {
            poolService = ServiceLocator.Instance.GetService<IPoolService>();
        }

        private void ShowModal()
        {
            var modalObject = poolService.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }

        private void ShowModal(string header, string message)
        {
            var modalObject = poolService.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }

        private void ShowModal(ModalWindowData data)
        {
            Debug.Log($"Pool is {(poolService is null?"null":"Not null")}");
            var modalObject = poolService.Get("Modal");
            Debug.Log($"Modal is {(modalObject is null?"null":"Not null")}");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent, false);
            var modal = modalObject.GetComponent<ModalWindow>();
            modal.Setup(data);
        }
    }
}