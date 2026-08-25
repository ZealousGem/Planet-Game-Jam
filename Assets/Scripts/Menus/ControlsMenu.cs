using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button ReadyButton;
    [SerializeField] private Button QuitButton;
    protected override void Awake()
    {
        ReadyButton.onClick.AddListener(ReadiedUp);
        QuitButton.onClick.AddListener(QuittingGame);
    }

    private void Start()
    {
        StartCoroutine(ShowMenu());
    }

    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(0.5f);
        Menu(true);
    }

    private void ReadiedUp()
    {
        Menu(false);
        EventBus.Act(new GameManagerEvent(GameState.StartWave));
    }

    private void QuittingGame()
    {
        DOTween.KillAll();
        Application.Quit();
    }

}
