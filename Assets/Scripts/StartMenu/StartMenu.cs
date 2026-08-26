using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Buttons")]
    [SerializeField] private Button StartButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    private OptionsMenu _OptionsMenu;

    protected override void Awake()
    {
        _OptionsMenu = GetComponent<OptionsMenu>();

        BindObject(StartButton.gameObject);

        StartButton.onClick.AddListener(PlayNewGame);

        OptionsButton.onClick.AddListener(OptionsMenu);
        QuitButton.onClick.AddListener(OnApplicationQuit);

    }

    void Start()
    {
         SoundPlayer.PlaySound("MenuMusic");

        // https://pixabay.com/music/folk-forest-fear-survival-action-drama-music-453621/
        // https://pixabay.com/music/ambient-space-587944/
        // https://pixabay.com/music/synthwave-outer-space-151769/ 
    }

    private void PlayNewGame()
    {
        DOTween.KillAll();
        SoundPlayer.StopAllInGameSounds();
        SceneManager.LoadScene(1);
    }

    private void OptionsMenu()
    {
        Menu(false);
        _OptionsMenu.Menu(true);
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
        Application.Quit();
    }

}
