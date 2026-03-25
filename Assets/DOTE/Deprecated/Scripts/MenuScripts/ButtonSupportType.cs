using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ButtonSupportType : GroupButton
{
    [SerializeField]
    private TypeOfSupport m_typeOfSupport;
    public TypeOfSupport TypeOfSupport => m_typeOfSupport;
    public event Action<ButtonSupportType> OnButtonClick;
    private TypeOfSupport chosenTypeOfSupport;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        chosenTypeOfSupport = TypeOfSupport;
        m_button.onClick.AddListener(OnCLick);
    }
        
    protected override void OnCLick()
    {
        base.OnCLick();
        if (IsEnabled)
        {
            m_typeOfSupport = chosenTypeOfSupport;

        }
        else
        {
            m_typeOfSupport = TypeOfSupport.Все;
        }
        OnButtonClick?.Invoke(this);
    }

}
