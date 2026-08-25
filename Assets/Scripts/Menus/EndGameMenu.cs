using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEditor;

public class EndGameMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Button RestartButton;
    public Button MenuButton;
    public TMP_Text Title;
    public TMP_Text Reason;

    protected override void Awake()
    {
        base.Awake();
        BindObject(RestartButton.gameObject);
    }

    protected override void retrieveData(GameStateEvent data)
    {
        setButtonClickLinster(data.gameState);

        //if (data.gameState == GameState.Success)EvokeMenu(data.Title, data.Reason, data.Amount);
        if (data.gameState == GameState.Failure) EvokeMenu("Game Over", "You Failed to Protect The Bases");

    }

    private void setButtonClickLinster(GameState gameState)
    {
        if (RestartButton == null) return;

        TMP_Text ButtonText = RestartButton.gameObject.GetComponentInChildren<TMP_Text>();

        if (ButtonText == null) return;

        switch (gameState)
        {
            //  case GameState.Success: MenuButton.onClick.AddListener(NextLevel); ButtonText.text = NextLevelText; break;
            case GameState.Failure:
                RestartButton.onClick.AddListener(ResetLevel); ButtonText.text = "Restart";
                MenuButton.onClick.AddListener(QuitGame);
                break;
        }
    }

    private void ResetLevel()
    {
        DOTween.KillAll();
        // SoundPlayer.StopAllInGameSounds();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void QuitToMenu()
    {
        DOTween.KillAll();
    }

    private void QuitGame()
    {
        DOTween.KillAll();
        Application.Quit();
    }

    protected void EvokeMenu(string title, string reason, float tim = 1f)
    {
        Menu(true);
        Title.text = title;

        TimeSpan timeSpan = TimeSpan.FromSeconds(tim);

        Reason.text = reason /*+ " Your time was "
        + timeSpan.ToString(@"mm\:ss\:fff")*/;
    }
}
