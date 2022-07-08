using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singlton;
public class Bilbarod : MonoBehaviour
{
  
   
   private void LateUpdate()
   {
      transform.LookAt(Dipendency.Instance.Camera.transform);
      transform.rotation=Quaternion.Euler(Dipendency.Instance.x,transform.rotation.eulerAngles.y+Dipendency.Instance.y,Dipendency.Instance.z);
   }
}
