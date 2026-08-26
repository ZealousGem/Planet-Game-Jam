using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPaused = false;

    private bool PauseActive = false;

    [SerializeField] private GameObject ResumeButton;

    protected override void Awake()
    {
        base.Awake();
        BindObject(ResumeButton);
    }
    protected override void retrieveData(GameStateEvent data)
    {
        //Debug.Log(PauseActive + " PauseMenu");
        if (GameState.Failure == data.gameState || GameState.Upgrades == data.gameState)
        {
            PauseAction(false);
            PauseActive = false;
        }

        if (GameState.Ongoing == data.gameState || GameState.StartWave == data.gameState)
        {
            PauseActive = true;
            PauseAction(true);
        }
    }

    private void PauseAction(bool state)
    {
        if (state)
        {
            map.UIController.Pause.Enable();
        }

        else
        {
            map.UIController.Pause.Disable();
            isPaused = false;
        }
    }
    protected override void EnableActions()
    {
        // EventBus.Subscribe<endGameUI>(retrieveData);
        base.EnableActions();
        map.UIController.Pause.performed += HandlePauseMenu;
    }

    protected override void DisableActions()
    {
        // EventBus.Unsubscribe<endGameUI>(retrieveData); 
        base.DisableActions();
        map.UIController.Pause.performed -= HandlePauseMenu;
    }

    private void HandlePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed || !PauseActive) return;

        if (isPaused)
        {
            UnPauseGame();
        }

        else
        {
            PauseGame();
        }
    }

    public void ResetLevel()
    {
        if (isPaused) Time.timeScale = 1f;
        DOTween.KillAll();
        // SoundPlayer.StopAllInGameSounds();    
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void PauseGame()
    {
        //  SoundPlayer.PauseSound();
        isPaused = true;

        Time.timeScale = 0f;
        Menu(true);
        EventBus.Act(new GameStateEvent(GameState.Paused));
    }

    public void UnPauseGame()
    {
        // SoundPlayer.UnpauseSound();
        isPaused = false;

        Time.timeScale = 1f;
        Menu(false);
        EventBus.Act(new GameStateEvent(GameState.Ongoing));

    }

    public override void ReturnToMainMenu()
    {
        if (isPaused) Time.timeScale = 1f;
        DOTween.KillAll();
        //base.ReturnToMainMenu();
    }

}
