using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct SettingsData
{
    public int ScreenScaleInd;
    public int ResolutionInd;
    public float DiageticSoundValue;

}
public enum SettingsDataValue { ScreenScale, Resolution, Quality, DiageticSoundValue, NonDiageticSoundValue, Vsync }

public class OptionsMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button StartButton;

    [SerializeField] private TMP_Dropdown ResoluationDropDown;

    [SerializeField] private TMP_Dropdown TypeDropDown;

    [SerializeField] private Slider DiageticSlider;

    private Resolution[] sizes;
    private int resIndex = 0;
    private StartMenu startMenu;

    private bool hasData = false;
    private SettingsData data = new SettingsData();
    protected override void Awake()
    {
        base.Awake();
        startMenu = GetComponent<StartMenu>();

        StartButton.onClick.AddListener(BackButton);

        BindObject(StartButton.gameObject);

        SetUpUiElements();

    }
    void Start() => setUpUserChoices();
    private void setUpUserChoices()
    {
        if (SettingsDataManager.Instance != null && SettingsDataManager.Instance.DataInFile())
        {
            data = SettingsDataManager.Instance.getFileData();
            hasData = true;
        }

        setSettings();
    }

    private void setSettings()
    {
        if (SettingsDataManager.Instance == null) return;

        if (hasData)
        {
            data = SettingsDataManager.Instance.getFileData();
            ApplySettings();
        }

        else
        {
            DefaultSettings();
            ApplySettings();
        }

    }

    private void ApplySettings()
    {


        setSize(data.ResolutionInd);
        ChangeWindowScale(data.ScreenScaleInd);

        ManageDiageticAudio(data.DiageticSoundValue);

        setUI();

        SettingsDataManager.Instance.setData(data);

    }

    void setUI()
    {
        ResoluationDropDown.value = data.ResolutionInd;
        ResoluationDropDown.RefreshShownValue();

        TypeDropDown.value = data.ScreenScaleInd;
        TypeDropDown.RefreshShownValue();

        DiageticSlider.value = data.DiageticSoundValue;

    }


    private void SetUpUiElements()
    {
        SetUpResDropDown();
        SetWindowScaleDropdown();
        SetUpSliders();
    }

    private void DefaultSettings()
    {
        data.ResolutionInd = resIndex;

        data.ScreenScaleInd = 1;
        data.DiageticSoundValue = 1f;
    }

    private void SetWindowScaleDropdown()
    {
        TypeDropDown.ClearOptions();

        List<string> Options = new List<string> { "Fullscreen", "Borderless", "Windowed" };

        TypeDropDown.AddOptions(Options);

        TypeDropDown.RefreshShownValue();

        TypeDropDown.onValueChanged.AddListener(ChangeWindowScale);
    }

    public void ChangeWindowScale(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen); break;
            case 1: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow); break;
            case 2: Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed); break;

        }

        WriteData(SettingsDataValue.ScreenScale, index);
    }

    private void SetUpResDropDown()
    {

        List<Resolution> uniqueRes = new List<Resolution>();
        foreach (var res in Screen.resolutions)
        {
            if (!uniqueRes.Exists(x => x.width == res.width && x.height == res.height))
            {
                uniqueRes.Add(res);
            }
        }
        sizes = uniqueRes.ToArray();

        ResoluationDropDown.ClearOptions();
        List<string> option = new List<string>();
        int sizesIndex = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            string choice = sizes[i].width + "x" + sizes[i].height;
            option.Add(choice);

            if (sizes[i].width == Screen.currentResolution.width && sizes[i].height == Screen.currentResolution.height)
            {
                sizesIndex = i;
            }
        }

        ResoluationDropDown.AddOptions(option);
        ResoluationDropDown.value = sizesIndex;
        ResoluationDropDown.RefreshShownValue();

        ResoluationDropDown.onValueChanged.AddListener(setSize);

        resIndex = sizesIndex;
    }

    public void setSize(int index)
    {
        if (sizes == null || sizes.Length == 0)
        {
            Debug.LogWarning("Resolutions array is not initialized!");
            return;
        }

        int safeIndex = Mathf.Clamp(index, 0, sizes.Length - 1);

        Resolution resolution = sizes[safeIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        WriteData(SettingsDataValue.Resolution, safeIndex);
        data.ResolutionInd = safeIndex;
    }

    private void SetUpSliders()
    {
        if (DiageticSlider == null) throw new UnityException("Sliders have not been binded");

        DiageticSlider.SetValueWithoutNotify(1f);

        DiageticSlider.onValueChanged.AddListener(ManageDiageticAudio);

    }

    private float PerceptialVolume(float volume)
    {
        float PerceptialVolume = Mathf.Pow(volume, 2f);
        return PerceptialVolume;
    }

    public void ManageDiageticAudio(float volume)
    {
        //SoundPlayer.ManageDiageticSound(PerceptialVolume(volume));
        WriteData(SettingsDataValue.DiageticSoundValue, volume);
    }

    private void BackButton()
    {
        Menu(false);
        startMenu.Menu(true);
    }

    private void WriteData(SettingsDataValue type, float value)
    {
        if (SettingsDataManager.Instance == null) return;

        switch (type)
        {
            case SettingsDataValue.Resolution: data.ResolutionInd = (int)value; break;
            case SettingsDataValue.ScreenScale: data.ScreenScaleInd = (int)value; break;
            case SettingsDataValue.DiageticSoundValue: data.DiageticSoundValue = value; break;
        }

        SettingsDataManager.Instance.setData(data);
    }
}
