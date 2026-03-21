using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ButtonClass : GroupButton
{
    [SerializeField]
    private Classes m_class;
    public Classes Class => m_class;
    public event Action<ButtonClass> OnButtonClick;
    private Classes chosenClass;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        chosenClass = Class;
        m_button.onClick.AddListener(OnCLick);
    }
    protected override void OnCLick()
    {
        base.OnCLick();
        if (IsEnabled)
            {
                m_class = chosenClass;               
            }
            else
            {
                m_class = Classes.Все;
            }
            OnButtonClick?.Invoke(this);
    }

}
