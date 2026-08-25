using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectedButton : MonoBehaviour
{
     [SerializeField] private GameObject firstButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        StartCoroutine(SelecetButton());
    }

    private IEnumerator SelecetButton()
    {
        yield return new WaitForEndOfFrame();

        if (firstButton != null && Cursor.lockState == CursorLockMode.Locked)
        {
             EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }
}
