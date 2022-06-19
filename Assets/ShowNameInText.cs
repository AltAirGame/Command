using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowNameInText : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI text;

   private void OnValidate()
   {
      text.text = gameObject.name;
   }
}
