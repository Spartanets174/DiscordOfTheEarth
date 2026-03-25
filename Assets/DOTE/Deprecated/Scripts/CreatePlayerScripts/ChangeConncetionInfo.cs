using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeConncetionInfo : MonoBehaviour, ILoadable
{
    [SerializeField]
    private TMP_InputField ipInputField;

    [SerializeField]
    private TMP_InputField portInputField;

    [SerializeField]
    private TMP_InputField uidInputField;

    [SerializeField]
    private TMP_InputField pwdInputField;

    [SerializeField]
    private TMP_InputField dataBaseInputField;

    [SerializeField]
    private Button reopenConnectionButton;

    [SerializeField]
    private DbManager dbManager;

    public void Init()
    {
        reopenConnectionButton.onClick.AddListener(ReopenConnection);
    }

    private void ReopenConnection()
    {
        ConnectionInfo.ChangeConncetionInfo(new ConnectionData(ipInputField.text, portInputField.text, uidInputField.text, pwdInputField.text, dataBaseInputField.text));
        dbManager.CloseCon();
        dbManager.OpenCon();
    }
}
