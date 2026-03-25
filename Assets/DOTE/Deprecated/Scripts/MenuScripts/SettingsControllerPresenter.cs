using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControllerPresenter : MonoBehaviour, ILoadable
{
    [Header("Game objects")]
    [SerializeField]
    private GameObject sureToExitWindow;

    [Header("Action UI")]
    [SerializeField]
    private Button closeSureToExitWindowButton;
    [SerializeField]
    private Button noSureToExitWindowButton;
    [SerializeField]
    private Button yesSureToExitWindowButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button exitAccountButton;
    [SerializeField]
    private Dropdown screenSizeDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;

    [Header("Sounds UI")]
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private Slider musicSlider;

    [Header("Settings")]
    [SerializeField]
    private bool isGame;

    private List<Button> soundButtons = new();
    private List<Dropdown> soundDropdowns = new();

    private SettingsController settingsController;

    public void Init()
    {
        soundButtons = FindObjectsOfType<Button>(true).ToList();
        soundDropdowns = FindObjectsOfType<Dropdown>(true).ToList();
        settingsController = FindObjectOfType<SettingsController>();

        foreach (var button in soundButtons)
        {
            button.onClick.AddListener(settingsController.PlayClickSound);
        }

        foreach (var dropdown in soundDropdowns)
        {
            dropdown.onValueChanged.AddListener((x) => settingsController.PlayClickSound());
        }

        List<string> resolutions = new();
        foreach (var resolution in settingsController.Resolutions)
        {
            resolutions.Add($"{resolution.Value.x}x{resolution.Value.y}");
        }
        screenSizeDropdown.ClearOptions();
        screenSizeDropdown.AddOptions(resolutions);
        closeSureToExitWindowButton.onClick.AddListener(CloseSureToExitWindow);
        noSureToExitWindowButton.onClick.AddListener(CloseSureToExitWindow);
        yesSureToExitWindowButton.onClick.AddListener(ExitAccount);
        closeButton.onClick.AddListener(CloseSettings);
        exitAccountButton.onClick.AddListener(OpenSureToExitWindow);

        soundSlider.onValueChanged.AddListener(OnSoundValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreenState);
        screenSizeDropdown.onValueChanged.AddListener(SetScreenSize);

        soundSlider.value = settingsController.GetSoundVolume();
        musicSlider.value = settingsController.GetMusicVolume();
        fullScreenToggle.SetIsOnWithoutNotify(settingsController.GetFullScreenState());
        screenSizeDropdown.SetValueWithoutNotify(settingsController.GetScreenSize());

        settingsController.OnPauseStateChanged += TogglePausedState;
    }

    private void TogglePausedState(bool state)
    {
        gameObject.SetActive(state);
    }

    private void SetScreenSize(int id)
    {
        settingsController.ChangeScreenSize(id);
    }

    private void SetFullScreenState(bool state)
    {
        settingsController.ChangeFullScreenState(state);
    }

    private void OnSoundValueChanged(float value)
    {
        settingsController.ChangeSoundLevel(value);
    }

    private void OnMusicValueChanged(float value)
    {
        settingsController.ChangeMusicLevel(value);
    }

    private void OpenSureToExitWindow()
    {
        sureToExitWindow.SetActive(true);
    }

    private void CloseSureToExitWindow()
    {
        sureToExitWindow.SetActive(false);
    }

    private void CloseSettings()
    {
        settingsController.TogglePausedState();
        foreach (var outlineInteractableObject in settingsController.OutlineInteractableObjects)
        {
            outlineInteractableObject.IsEnabled = true;
        }
    }

    protected virtual void ExitAccount()
    {
        if (isGame)
        {
            SceneController.ToMenu();
        }
        else
        {
            settingsController.ExitAccount();
        }

    }
}
