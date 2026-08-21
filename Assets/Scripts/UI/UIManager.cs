using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum UIevents { Waves, Bases, Ammo, NoAmmo, EnemiesLeft, AllNum, WaveStart, Tower }
public class UIManager : MonoBehaviour
{

    [Header("TextUI")]
    [SerializeField] private TMP_Text WaveCounter;
    [SerializeField] private TMP_Text BasesLeftText;
    [SerializeField] private TMP_Text AmmoText;
    [SerializeField] private TMP_Text EnemiesLeftText;

    [Header("Pop-up Text")]
    [SerializeField] private TMP_Text StartWaveCounter;
    [SerializeField] private TMP_Text TowerNotifyUI;


    private void OnEnable()
    {
        EventBus.Subscribe<UIEvent>(RetreiveData);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<UIEvent>(RetreiveData);
    }

    private void RetreiveData(UIEvent data)
    {
        switch (data)
        {
            case NumUIEvent numEvent: DetermineNumUI(numEvent.uiType, numEvent.num); break;
            case PopUpEvent popEvent: DeterminePopUp(popEvent.uiType, popEvent.stringText); break;
        }
    }
    private void DetermineNumUI(UIevents uIevents, int num)
    {
        switch (uIevents)
        {
            case UIevents.Waves: setNumText(WaveCounter, num, "Wave "); break;
            case UIevents.Bases: setNumText(BasesLeftText, num, "Bases Left "); break;
            case UIevents.Ammo: setNumText(AmmoText, num, "Ammo "); break;
            case UIevents.NoAmmo: setNumText(AmmoText, "Reloading..."); break;
            case UIevents.EnemiesLeft: setNumText(EnemiesLeftText, num, "Aliens Left: "); break;
        }
    }

    private void setNumText(TMP_Text text, string StringText) => text.text = StringText;
    private void setNumText(TMP_Text text, int num, string StringText) => text.text = StringText + num;
    private void DeterminePopUp(UIevents uIevents, string Text)
    {
        switch (uIevents)
        {
            case UIevents.WaveStart: setWaveCounter(Text); break;
            case UIevents.Tower: StartCoroutine(TowerDestoryedEvent(Text));break;
        }
    }

    private void setWaveCounter(string text)
    {
        StartWaveCounter.text = text;
    }

    private IEnumerator TowerDestoryedEvent(string text)
    {
        TowerNotifyUI.text = text;
        yield return new WaitForSeconds(0.6f);
         TowerNotifyUI.text = "";
    }
}
