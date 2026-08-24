using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseMainMenu : UIController
{
    public GameObject menu;
    [SerializeField]private RectTransform menuScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    protected override void OnEnable()
    {
       EventBus.Subscribe<GameStateEvent>(retrieveData);
       base.OnEnable();
    }

    protected override void OnDisable()
    {
      EventBus.Unsubscribe<GameStateEvent>(retrieveData); 
      base.OnDisable();
    }

    protected virtual void retrieveData(GameStateEvent data){}

    protected virtual void Awake()
    {
        menu.SetActive(false);
    }
    
    public virtual void Menu(bool state)
    {
        if(menu == null || menuScale == null) return;

        menuScale.DOKill();

        if (state)
        { 
            menu.SetActive(true);
            //menuScale.DOKill();
            menuScale.localScale = Vector3.zero;
            DOTween.To(() => menuScale.localScale, x => menuScale.localScale = x, Vector3.one, 0.5f)
               .SetEase(Ease.OutBack)
               .SetUpdate(true);
        }

        else
        {
            menuScale.transform.DOScale(Vector3.zero, 0.25f)
             .SetUpdate(true).OnComplete(() => menu.SetActive(false));
            
        }
    }

    public virtual void ReturnToMainMenu()
    {
        //SoundPlayer.StopAllInGameSounds();
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    } 
    
}