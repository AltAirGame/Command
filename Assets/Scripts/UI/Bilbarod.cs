

using UnityEngine;


namespace GameSystems.Core
{

   public class Bilbarod : MonoBehaviour
   {
      private Camera _camera;

      private void Start()
      {
         _camera = ServiceLocator.Instance.GetComponent<Camera>();
      }

      private void LateUpdate()
      {
         transform.LookAt(_camera.transform);
         // transform.rotation=Quaternion.Euler(Dipendency.Instance.x,transform.rotation.eulerAngles.y+Dipendency.Instance.y,Dipendency.Instance.z);
      }
   }
}