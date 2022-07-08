using System;
using MHamidi.Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace MHamidi.UI.UI_Messages
{
    public class ModalManger : MonoBehaviour
    {
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
        private void ShowModal()
        {
            var modalObject=Pool.Instance.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal=modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }
        private void ShowModal(string header, string message)
        {
            var modalObject=Pool.Instance.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent);
            var modal=modalObject.GetComponent<ModalWindow>();
            modal.Setup(new ModalWindowData());
        }
        private void ShowModal(ModalWindowData data)
        {
            var modalObject=Pool.Instance.Get("Modal");
            modalObject.SetActive(true);
            modalObject.transform.SetParent(modalMessageParent,false);
            var modal=modalObject.GetComponent<ModalWindow>();
            modal.Setup(data);
        }
    }
}