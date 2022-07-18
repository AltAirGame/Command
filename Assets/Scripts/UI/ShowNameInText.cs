using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowNameInText : MonoBehaviour,ITextRename
{
    
   
   public TextMeshProUGUI text { get; set; }
   public void RenameTOText(string name)
   {
       text = GetComponentInChildren<TextMeshProUGUI>();
       if (text is not null)
       {
           var correctedName=name.Replace("command", "");
           gameObject.name = correctedName;
           text.text = correctedName;    
       }
       else
       {
           
       }
       
   }
}
