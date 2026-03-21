using DG.Tweening;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerCreatePlayer : MonoBehaviour, ILoadable
{
    [Header("Action UI")]
    [SerializeField] private InputField Nick;
    [SerializeField] private InputField Password;
    [SerializeField] private Button submitRegistration;
    [SerializeField] private Button submitLogin;
    [SerializeField] private Button toRegistration;
    [SerializeField] private Button toLogin;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Toggle passwordToggle;

    [Header("Panels")]
    [SerializeField] private GameObject registrationPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private ChangeConncetionInfo changeConncetionInfo;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI warningTextNick;
    [SerializeField] private TextMeshProUGUI warningTextPassword;

    [Header("Scripts")]
    [SerializeField] private DataLoader dataLoader;
    private SettingsController settingsController;

    private Sequence sequenceName;
    private Sequence sequencePassword;
    private Regex validateGuidRegex;

    private float timer = 1f;
    private CompositeDisposable disposables = new();
    public void Init()
    {
        settingsController = FindObjectOfType<SettingsController>();
        loadingPanel.SetActive(false);
        changeConncetionInfo.gameObject.SetActive(false);
        Observable.EveryUpdate().Subscribe(x =>
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.V))
                {
                    changeConncetionInfo.gameObject.SetActive(!changeConncetionInfo.gameObject.activeSelf);
                    timer = 1f;
                    Debug.Log(changeConncetionInfo.gameObject.activeSelf);
                }
            }
        }).AddTo(disposables);

        validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[#?!@$%^&*-]).{4,}$");        

        dataLoader.OnPlayerDataRecieved += CheckRecievedData;
        dataLoader.OnPlayerDataChecked += OnPlayerDataChecked;

        submitLogin.onClick.AddListener(CheckInputsLogin);
        submitRegistration.onClick.AddListener(CheckInputs);
        toRegistration.onClick.AddListener(SwapPanels);
        toLogin.onClick.AddListener(SwapPanels);
        Nick.onValueChanged.AddListener(TurnOffWarningTextName);
        Password.onValueChanged.AddListener(TurnOffWarningTextPassword);
        passwordToggle.onValueChanged.AddListener(ChangePasswordVisibility);
        settingsButton.onClick.AddListener(settingsController.TogglePausedState);

        passwordToggle.isOn = false;
        dataLoader.CheckPlayerData();
    }

    private void OnPlayerDataChecked(bool state)
    {
        loadingPanel.SetActive(state);
    }

    private void ChangePasswordVisibility(bool state)
    {
        if (state)
        {
            Password.contentType = InputField.ContentType.Standard;
        }
        else
        {
            Password.contentType = InputField.ContentType.Password;
        }
        Password.ForceLabelUpdate();
    }

    private void OnDestroy()
    {
        dataLoader.OnPlayerDataRecieved -= CheckRecievedData; 
        dataLoader.OnPlayerDataChecked -= OnPlayerDataChecked;


        submitLogin.onClick.RemoveListener(CheckInputsLogin);
        submitRegistration.onClick.RemoveListener(CheckInputs);
        toRegistration.onClick.RemoveListener(SwapPanels);
        toLogin.onClick.RemoveListener(SwapPanels);
        Nick.onValueChanged.RemoveListener(TurnOffWarningTextName);
        Password.onValueChanged.RemoveListener(TurnOffWarningTextPassword);
        passwordToggle.onValueChanged.RemoveListener(ChangePasswordVisibility);

        StopAllCoroutines();

        disposables.Dispose();
        disposables.Clear();
        disposables = new();
    }

    private void CheckRecievedData(string connectionAnwser)
    {
        if (connectionAnwser == "loginned")
        {
            SceneController.ToMenu();            
        }
        if (connectionAnwser == "notConnected")
        {
            loadingPanel.SetActive(false);
            warningTextPassword.text = "Отсутсвует подключение к серверу!";
            OnWarningTextPassword();
        }
        if (connectionAnwser == "wrongCreditials")
        {
            loadingPanel.SetActive(false);
            warningTextPassword.text = "Неправильный логин или пароль!";
            OnWarningTextPassword();
        }
    }
    private void CheckInputsLogin()
    {
        Nick.text = Nick.text.Trim();
        Password.text = Password.text.Trim();
        StopAllCoroutines();

        bool isNameValid = CheckNameLogin();
        bool isPasswordValid = CheckPasswordLogin();

        if (isNameValid && isPasswordValid)
        {
            loadingPanel.SetActive(true);

            StartCoroutine(dataLoader.GetPlayerData(Nick.text, Password.text));
        }
    }
    public void CheckInputs()
    {
        Nick.text = Nick.text.Trim();
        Password.text = Password.text.Trim();
        StopAllCoroutines();

        bool isNameValid = CheckName();
        bool isPasswordValid = CheckPassword();

        if (isNameValid && isPasswordValid)
        {
            loadingPanel.SetActive(true);
            StartCoroutine(dataLoader.CreateNewPlayer(Nick.text, Password.text));
        }
    }
    private void SwapPanels()
    {
        registrationPanel.SetActive(!registrationPanel.activeSelf);
        loginPanel.SetActive(!loginPanel.activeSelf);
    }
    private void TurnOffWarningTextName(string args)
    {
        warningTextNick.gameObject.SetActive(false);
    }
    private void TurnOnWarningTextName()
    {
        warningTextNick.alpha = 1;
        warningTextNick.gameObject.SetActive(true);
    }
    private void TurnOffWarningTextPassword(string args)
    {
        warningTextPassword.gameObject.SetActive(false);
    }
    private void TurnOnWarningTextPassword()
    {
        warningTextPassword.alpha = 1;
        warningTextPassword.gameObject.SetActive(true);
    }
    private bool CheckPasswordLogin()
    {
        if (Password.text == "")
        {
            warningTextPassword.text = "Вы ничего не ввели!";
            OnWarningTextPassword();
            return false;
        }

        return true;
    }

    private bool CheckPassword()
    {
        if (Password.text == "")
        {
            warningTextPassword.text = "Вы ничего не ввели!";
            OnWarningTextPassword();
            return false;
        }
        if (Password.text.Length < 4)
        {
            warningTextPassword.text = "Пароль слишком маленький (минимум 4 символа)!";
            OnWarningTextPassword();
            return false;
        }
        if (!validateGuidRegex.IsMatch(Password.text))
        {
            warningTextPassword.text = "Пароль должен соответсвовать требованиям: \nХотя бы одна заглавная английская буква,хотя бы один специальный символ";
            OnWarningTextPassword();
            return false;
        }
        return true;
    }

    private bool CheckNameLogin()
    {
        if (Nick.text == "")
        {
            warningTextNick.text = "Вы ничего не ввели!";
            OnWarningTextName();
            return false;
        }

        if (dataLoader.IsNicknameInBase(Nick.text))
        {
            warningTextNick.text = "Данного имени не существует!";
            OnWarningTextName();
            return false;
        }

        return true;
    }

    private bool CheckName()
    {
        if (Nick.text == "")
        {
            warningTextNick.text = "Вы ничего не ввели!";
            OnWarningTextName();
            return false;        }

        if (Nick.text.Length > 20)
        {
            warningTextNick.text = "Имя слишком большое (максимум 15 символов)!";
            OnWarningTextName();
            return false;
        }
        if (Nick.text.Length < 4)
        {

            warningTextNick.text = "Имя слишком маленькое (минимум 4 символа)!";
            OnWarningTextName();
            return false;
        }

        if (!dataLoader.IsNicknameInBase(Nick.text))
        {
            warningTextNick.text = "Данное имя уже существует!";
            OnWarningTextName();
            return false;
        }


        return true;
    }

    private void OnWarningTextName()
    {
        sequenceName.Kill();
        TurnOnWarningTextName();
        sequenceName = DOTween.Sequence();
        sequenceName.AppendInterval(2).Append(warningTextNick.DOFade(0, 2f))
            .OnComplete(() =>
            {
                warningTextNick.color = Color.red;
                TurnOffWarningTextName("args");
                sequenceName.Kill();
            });
        sequenceName.Play();
    }

    private void OnWarningTextPassword()
    {
        sequencePassword.Kill();
        TurnOnWarningTextPassword();
        sequencePassword = DOTween.Sequence();
        sequencePassword.AppendInterval(2).Append(warningTextPassword.DOFade(0, 2f))
            .OnComplete(() =>
            {
                warningTextPassword.color = Color.red;
                TurnOffWarningTextPassword("args");
                sequencePassword.Kill();
            });
        sequencePassword.Play();
    }
}

