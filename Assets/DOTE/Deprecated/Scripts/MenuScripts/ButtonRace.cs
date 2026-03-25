using System;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class ButtonRace : GroupButton
{

    [SerializeField]
    private Races m_race;
    public Races Race => m_race;
    public event Action<ButtonRace> OnButtonClick;
    private Races chosenRace;

    private void Start()
    {       
        Init();
    }

    public override void Init()
    {
        base.Init();
        chosenRace = Race;
        m_button.onClick.AddListener(this.OnCLick);
    }
    protected override void OnCLick()
    {
        base.OnCLick();
        if (IsEnabled)
        { 
            m_race = chosenRace;
        }
        else
        {
            m_race = Races.Все;            
        }
        OnButtonClick?.Invoke(this);
    }
}
