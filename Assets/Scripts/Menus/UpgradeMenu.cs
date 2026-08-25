using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Types of Upgrades Player Could Have")]
    [SerializeField] private List<BaseUpgrade> Upgrades;
    private List<BaseUpgrade> currentUpgrades = new List<BaseUpgrade>();

    [Header("Buttons")]
    private List<Button> Buttons;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<GameStateEvent>(RetrieveData);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<GameStateEvent>(RetrieveData);
    }

    private void RetrieveData(GameStateEvent data)
    {
        if (data.gameState == GameState.Upgrades)
        {
            getUpgrades();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitlaiseButtons();
    }

    private void InitlaiseButtons()
    {
        for (int i = 0; i < Buttons.Count;)
        {
            int index = i;
            Buttons[i].onClick.AddListener(() => invokeUpgrade(index));
        }
    }

    private void getUpgrades()
    {
        currentUpgrades.Clear();

        if (Upgrades.Count < Buttons.Count)
        {
            Debug.LogWarning("Not enough upgrades available to prevent duplicates!");
            return;
        }

        for (int i = 0; i < Buttons.Count; i++)
        {

            int index = Random.Range(0, Upgrades.Count);
            BaseUpgrade upgrade = Upgrades[index];

            while (currentUpgrades.Contains(upgrade))
            {
                index = Random.Range(0, Upgrades.Count);
                upgrade = Upgrades[index];
            }

            currentUpgrades.Add(Upgrades[index]);

            Menu(true);
        }
    }

    private void invokeUpgrade(int index)
    {
        if (currentUpgrades.Count > index && currentUpgrades[index] != null) return;

        currentUpgrades[index].Effect();

        currentUpgrades.Clear();
        Menu(false);

        EventBus.Act(new GameManagerEvent(GameState.StartWave));
    }

}
