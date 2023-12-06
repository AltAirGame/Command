
using DG.Tweening;

using UnityEngine;

namespace GameSystems.Core
{
    

public interface IButtonAnimation
{
    public void OnClick();
    public void OnPointerEnter();
    public void OnPointerExit();
    public void Idle();
    public void OnHold();
    public void OnPointerUp();


}

public class LevelMenuButtonAnimation : MonoBehaviour,IButtonAnimation
{

    public Ease ease;
    private Vector3 initalPos;

    private void Start()
    {
        
    }

    public void OnClick()
    {
        var position = transform.position;
        transform.DOMove(new Vector3(position.x+150, position.y, position.z), .1f).SetEase(ease);
        
    }

    public void OnPointerEnter()
    {
     
      
    }

    public void OnPointerExit()
    {
        
    }

    public void OnPointerUp()
    {
        Util.ShowMessage($" On Pointer UP",TextColor.White);
        var position = transform.position;
        transform.position = new Vector3(position.x - 150, position.y, position.z);
    }

  

    public void Idle()
    {
        transform.DOShakePosition(.3f, new Vector3(0, 20, 0)).SetEase(ease);;
    }

    public void OnHold()
    {
       
    }
}
    
}