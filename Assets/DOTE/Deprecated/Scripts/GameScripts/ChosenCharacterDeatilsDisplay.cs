using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChosenCharacterDeatilsDisplay : MonoBehaviour, ILoadable
{
    [Header("Scripts")]
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private BattleSystem battleSystem;

    [Header("Card data")]
    [SerializeField]
    private TextMeshProUGUI characterNameText;
    [SerializeField]
    private TextMeshProUGUI cardRaceText;

    [SerializeField]
    private HealthBar healthBar;

    [Header("Card images")]
    [SerializeField]
    private Image cardImage;
    [SerializeField]
    private Image cardClassImage;
    [SerializeField]
    private Image cardAttackImage;
    [SerializeField]
    private Image cardDefenceImage;
    [SerializeField]
    private Image cardBuffImage;

    [Header("Card characteristics")]
    [SerializeField]
    private TextMeshProUGUI physAttackText;
    [SerializeField]
    private TextMeshProUGUI magAttackText;
    [SerializeField]
    private TextMeshProUGUI physDefenceText;
    [SerializeField]
    private TextMeshProUGUI magDefenceText;

    [Header("Card abilites")]
    [SerializeField]
    private OutlineClicableUI attackAbilityButton;
    [SerializeField]
    private OutlineClicableUI defenceAbilityButton;
    [SerializeField]
    private OutlineClicableUI buffAbilityButton;

    [Header("Sprites")]
    [SerializeField]
    private Sprite warriorSprite;
    [SerializeField]
    private Sprite archerSprite;
    [SerializeField]
    private Sprite wizardSprite;
    [SerializeField]
    private Sprite сavalrySprite;
    [SerializeField]
    private Sprite defaultAttackImage;
    [SerializeField]
    private Sprite defaultDefenceImage;
    [SerializeField]
    private Sprite defaultBuffImage;

    [Space, Header("Text panel")]
    [SerializeField]
    private TextMeshProUGUI tipsText;
    [SerializeField]
    private GameObject tipsTextParent;
    [SerializeField]
    private TextMeshProUGUI characterAbilityText;
    [SerializeField]
    private GameObject characterAbilityPanel;

    public ReactiveProperty<Character> currentCharacter = new();

    public ReactiveProperty<Character> usingAbilityCharacter = new();

    private CompositeDisposable disposables = new();

    public event Action OnAbilitySelected;
    public event Action OnAbilityUsed;
    public event Action OnAbilityUsingCancel;
    private void Start()
    {
        DisableInfoPanel();
    }
    public void Init()
    {
        attackAbilityButton.OnHoverEnter += ()=> EnableInfoPanel(Enums.TypeOfAbility.attack);
        attackAbilityButton.OnHoverExit += DisableInfoPanel;

        defenceAbilityButton.OnHoverEnter += () => EnableInfoPanel(Enums.TypeOfAbility.defence);
        defenceAbilityButton.OnHoverExit += DisableInfoPanel;

        buffAbilityButton.OnHoverEnter += () => EnableInfoPanel(Enums.TypeOfAbility.buff);
        buffAbilityButton.OnHoverExit += DisableInfoPanel;
        characterAbilityPanel.SetActive(false);

        foreach (var playerCharacter in playerController.PlayerCharactersObjects)
        {
            if (playerCharacter.PassiveCharacterAbility != null)
            {
                playerCharacter.PassiveCharacterAbility.Init(battleSystem, playerCharacter);
            }


            playerCharacter.AttackCharacterAbility.OnCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.AttackCharacterAbility.OnSecondCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.AttackCharacterAbility.OnThirdCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.AttackCharacterAbility.OnCardAbilityCharacterSelected += OnCardAbilityCharacterSelected;
            playerCharacter.AttackCharacterAbility.OnCardAbilityUsed += OnCardAbilityUsed;
            playerCharacter.AttackCharacterAbility.OnUsingCancel += OnUsingCancel;
            playerCharacter.AttackCharacterAbility.Init(battleSystem, playerCharacter, playerCharacter.Card.attackCharacterAbilityData);

            playerCharacter.DefenceCharacterAbility.OnCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.DefenceCharacterAbility.OnSecondCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.DefenceCharacterAbility.OnThirdCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.DefenceCharacterAbility.OnCardAbilityCharacterSelected += OnCardAbilityCharacterSelected;
            playerCharacter.DefenceCharacterAbility.OnCardAbilityUsed += OnCardAbilityUsed;
            playerCharacter.DefenceCharacterAbility.OnUsingCancel += OnUsingCancel;
            playerCharacter.DefenceCharacterAbility.Init(battleSystem, playerCharacter, playerCharacter.Card.defenceCharacterAbilityData);

            playerCharacter.BuffCharacterAbility.OnCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.BuffCharacterAbility.OnSecondCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.BuffCharacterAbility.OnThirdCardAbilitySelected += OnCardAbilitySelected;
            playerCharacter.BuffCharacterAbility.OnCardAbilityCharacterSelected += OnCardAbilityCharacterSelected;
            playerCharacter.BuffCharacterAbility.OnCardAbilityUsed += OnCardAbilityUsed;
            playerCharacter.BuffCharacterAbility.OnUsingCancel += OnUsingCancel;
            playerCharacter.BuffCharacterAbility.Init(battleSystem, playerCharacter, playerCharacter.Card.buffCharacterAbilityData);
        }
        DisableInfoPanel();
    }
    private void OnDestroy()
    {
        attackAbilityButton.OnHoverEnter -= () => EnableInfoPanel(Enums.TypeOfAbility.attack);
        attackAbilityButton.OnHoverExit -= DisableInfoPanel;

        defenceAbilityButton.OnHoverEnter -= () => EnableInfoPanel(Enums.TypeOfAbility.defence);
        defenceAbilityButton.OnHoverExit -= DisableInfoPanel;

        buffAbilityButton.OnHoverEnter -= () => EnableInfoPanel(Enums.TypeOfAbility.buff);
        buffAbilityButton.OnHoverExit -= DisableInfoPanel;
        disposables.Dispose();
        disposables.Clear();
    }
    private void OnCardAbilitySelected(ICardSelectable selectable)
    {
        SetAbilityButtonsState(false);
        tipsTextParent.SetActive(true);
        SetTipsText($"{selectable.SelectCardTipText}");
        OnAbilitySelected?.Invoke();
    }

    private void OnCardAbilityCharacterSelected(ICharacterSelectable characterSelectable)
    {
        SetTipsText($"{characterSelectable.SelectCharacterTipText}");
        playerController.PlayerTurn.ClearDisposables();
    }
    private void OnCardAbilityUsed(BaseCharacterAbility ability)
    {
        StartCoroutine(OnCardAbilityUsedDelayed(ability));
    }

    private IEnumerator OnCardAbilityUsedDelayed(BaseCharacterAbility ability)
    {
        yield return new WaitForEndOfFrame();
        if (ability.TypeOfAbility == Enums.TypeOfAbility.attack)
        {
            attackAbilityButton.IsEnabled = false;
            usingAbilityCharacter.Value.IsAttackAbilityUsed = true;
        }
        if (ability.TypeOfAbility == Enums.TypeOfAbility.defence)
        {
            defenceAbilityButton.IsEnabled = false;
            usingAbilityCharacter.Value.IsDefenceAbilityUsed = true;
        }
        if (ability.TypeOfAbility == Enums.TypeOfAbility.buff)
        {
            buffAbilityButton.IsEnabled = false;
            usingAbilityCharacter.Value.IsBuffAbilityUsed = true;
        }

        SetRulesAbilityButtonsState(usingAbilityCharacter.Value);

        playerController.PlayerTurn.SetStateToNormal();
        battleSystem.FieldController.TurnOnCells();
        playerController.PlayerTurn.ClearDisposables();

        battleSystem.PointsOfAction.Value -= usingAbilityCharacter.Value.UseAbilityCost;

        tipsTextParent.SetActive(false);
        SetTipsText("");
        OnAbilityUsed?.Invoke();
        StopCoroutine(OnCardAbilityUsedDelayed(ability));
    }

    private void OnUsingCancel(BaseCharacterAbility ability)
    {
        playerController.PlayerTurn.SetStateToNormal();
        battleSystem.FieldController.TurnOnCells();
        playerController.PlayerTurn.ClearDisposables();

        SetRulesAbilityButtonsState(usingAbilityCharacter.Value);
        if (ability is ITurnCountable turnCountable)
        {
            if (turnCountable.IsBuff)
            {
                battleSystem.PlayerTurnCountables.Remove(turnCountable);
            }
            else
            {
                battleSystem.EnemyTurnCountables.Remove(turnCountable);
            }
        }

        tipsTextParent.SetActive(false);
        SetTipsText("");
        OnAbilityUsingCancel?.Invoke();
    }

    public void SetData(Character character)
    {
        if (character == null)
        {
            ResetData();
            return;
        }
        cardImage.DOFade(1, 0);
        cardClassImage.DOFade(1, 0);
        if (currentCharacter != null)
        {
            attackAbilityButton.OnClick -= UseAttackAbility;
            defenceAbilityButton.OnClick -= UseDefencebility;
            buffAbilityButton.OnClick -= UseBuffAbility;
        }

        currentCharacter.Value = character;

        characterNameText.text = currentCharacter.Value.CharacterName;
        healthBar.SetMaxHealth(currentCharacter.Value.Card.health);
        healthBar.SetHealth(currentCharacter.Value.Health, 0);
        physAttackText.text = $"Физическая атака: {currentCharacter.Value.PhysAttack * 100}";
        magAttackText.text = $"Магическая атака: {currentCharacter.Value.MagAttack * 100}";
        physDefenceText.text = $"Физическая защита: {currentCharacter.Value.PhysDefence * 100}";
        magDefenceText.text = $"Магическая защита: {currentCharacter.Value.MagDefence * 100}";

        if (character.Card.attackCharacterAbilityData != null)
        {
            if (character.Card.attackCharacterAbilityData.abilityImage != null)
            {
                cardAttackImage.sprite = character.Card.attackCharacterAbilityData.abilityImage;
            }
            else
            {
                cardAttackImage.sprite = defaultAttackImage;
            }
        }

        if (character.Card.defenceCharacterAbilityData != null)
        {
            if (character.Card.defenceCharacterAbilityData.abilityImage != null)
            {
                cardDefenceImage.sprite = character.Card.defenceCharacterAbilityData.abilityImage;
            }
            else
            {
                cardDefenceImage.sprite = defaultDefenceImage;

            }
        }

        if (character.Card.buffCharacterAbilityData !=null )
        {
            if (character.Card.buffCharacterAbilityData.abilityImage != null)
            {
                cardBuffImage.sprite = character.Card.buffCharacterAbilityData.abilityImage;
            }
            else
            {
                cardBuffImage.sprite = defaultBuffImage;
            }
        }

        

        cardImage.sprite = currentCharacter.Value.Card.image;
        switch (currentCharacter.Value.Card.Class)
        {
            case Enums.Classes.Паладин:
                cardClassImage.sprite = warriorSprite;
                break;
            case Enums.Classes.Лучник:
                cardClassImage.sprite = archerSprite;
                break;
            case Enums.Classes.Маг:
                cardClassImage.sprite = wizardSprite;
                break;
            case Enums.Classes.Кавалерия:
                cardClassImage.sprite = сavalrySprite;
                break;
        }

        switch (currentCharacter.Value.Card.race)
        {
            case Enums.Races.Гномы:
                cardRaceText.text = "Г";
                break;
            case Enums.Races.Люди:
                cardRaceText.text = "Л";
                break;
            case Enums.Races.МагическиеСущества:
                cardRaceText.text = "МС";
                break;
            case Enums.Races.ТёмныеЭльфы:
                cardRaceText.text = "ТЭ";
                break;
            case Enums.Races.Эльфы:
                cardRaceText.text = "Э";
                break;
        }

        if (currentCharacter.Value is PlayerCharacter && battleSystem.State is PlayerTurn)
        {
            SetRulesAbilityButtonsState(currentCharacter.Value);
            attackAbilityButton.OnClick += UseAttackAbility;
            defenceAbilityButton.OnClick += UseDefencebility;
            buffAbilityButton.OnClick += UseBuffAbility;
        }
        else
        {
            DisableInfoPanel();
            SetAbilityButtonsState(false);
        }
    }

    private void DisableInfoPanel()
    {
        characterAbilityPanel.SetActive(false);
        characterAbilityText.text = "";
    }

    private void EnableInfoPanel(Enums.TypeOfAbility typeOfAbility)
    {
        if (currentCharacter.Value == null) return;
        characterAbilityPanel.SetActive(true);
        switch (typeOfAbility)
        {
            case Enums.TypeOfAbility.attack:
                characterAbilityText.text = currentCharacter.Value.Card.attackAbility;
                break;
            case Enums.TypeOfAbility.defence:
                characterAbilityText.text = currentCharacter.Value.Card.defenceAbility;
                break;
            case Enums.TypeOfAbility.buff:
                characterAbilityText.text = currentCharacter.Value.Card.buffAbility;
                break;
        }
        
    }

    private void ResetData()
    {
        characterNameText.text = string.Empty;

        healthBar.SetMaxHealth(0);
        healthBar.SetHealth(0, 0);

        physAttackText.text = string.Empty;
        magAttackText.text = string.Empty;
        physDefenceText.text = string.Empty;
        magDefenceText.text = string.Empty;

        cardRaceText.text = string.Empty;

        cardImage.DOFade(0, 0);
        cardClassImage.DOFade(0, 0);

        cardRaceText.text = string.Empty;

        SetAbilityButtonsState(false);
    }

    private void UseAttackAbility(GameObject gameObject)
    {
        usingAbilityCharacter.Value = currentCharacter.Value;
        battleSystem.OnAttackAbilityButton(currentCharacter.Value.gameObject);
    }
    private void UseDefencebility(GameObject gameObject)
    {
        usingAbilityCharacter.Value = currentCharacter.Value;
        battleSystem.OnDefensiveAbilityButton(currentCharacter.Value.gameObject);
    }
    private void UseBuffAbility(GameObject gameObject)
    {
        usingAbilityCharacter.Value = currentCharacter.Value;
        battleSystem.OnBuffAbilityButton(currentCharacter.Value.gameObject);
    }

    public void SetRulesAbilityButtonsState(Character character)
    {
        if (currentCharacter.Value == null) return;
        if (!currentCharacter.Value.CanUseAbilities)
        {
            SetAbilityButtonsState(false);
            return;
        }
        attackAbilityButton.IsEnabled = !character.IsAttackAbilityUsed;
        defenceAbilityButton.IsEnabled = !character.IsDefenceAbilityUsed;
        buffAbilityButton.IsEnabled = !character.IsBuffAbilityUsed;
    }

    public void SetAbilityButtonsState(bool state)
    {
        if (currentCharacter.Value == null) return;
        if (!currentCharacter.Value.CanUseAbilities)
        {
            state = false;
        }
        attackAbilityButton.IsEnabled = state;
        defenceAbilityButton.IsEnabled = state;
        buffAbilityButton.IsEnabled = state;
    }
    private void SetTipsText(string message)
    {
        tipsText.text = message;
    }
}
