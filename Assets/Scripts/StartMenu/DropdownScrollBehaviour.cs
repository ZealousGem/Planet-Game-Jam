using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownScrollBehaviour : MonoBehaviour, ISelectHandler
{
    /*
    
    Add this Script on the Item object in the Dropdown Menu not dropdown itself moron.
    
    
    */
    private ScrollRect Scroll;
    private RectTransform Rectransform; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       Rectransform = GetComponent<RectTransform>();   

    }

     public void OnSelect(BaseEventData eventData)
    {
      StartCoroutine(snapToPos());

    }

    private IEnumerator snapToPos()
     {
        yield return new WaitForEndOfFrame();

        if(Cursor.lockState == CursorLockMode.None) yield break;
        if (Scroll == null) Scroll = GetComponentInParent<ScrollRect>();
    
        RectTransform content = Scroll.content;
    
   
    float totalHeight = content.rect.height - Scroll.viewport.rect.height;
    if (totalHeight <= 0) yield break;

    float itemTopPos = -Rectransform.localPosition.y; 
     
    float targetPos = 1f - (itemTopPos / totalHeight);

    Scroll.verticalNormalizedPosition = Mathf.Clamp01(targetPos);
    }
}
