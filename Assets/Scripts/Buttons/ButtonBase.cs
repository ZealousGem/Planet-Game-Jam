using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public abstract void OnDeselect(BaseEventData eventData);
   

    public  abstract void OnPointerClick(PointerEventData eventData);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void OnPointerEnter(PointerEventData eventData);
   
    public abstract void OnPointerExit(PointerEventData eventData);

    public abstract void OnSelect(BaseEventData eventData);
    

    public abstract void OnSubmit(BaseEventData eventData);

    public abstract void Entered();

    public abstract void Left();

    public abstract void Clicked();
    
}
