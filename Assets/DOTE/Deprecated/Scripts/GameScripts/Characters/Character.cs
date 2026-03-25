using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class Character : ChildOutlineInteractableObject
{
    [SerializeField]
    protected CharacterCard m_card;
    public CharacterCard Card => m_card;

    [SerializeField]
    protected HealthBar healthBar;

    [Header("Spawn points")]
    [SerializeField]
    private Transform topPoint;
    [SerializeField]
    private Transform bottomPoint;
    [SerializeField]
    private Transform middlePoint;

    protected string m_characterName;
    public string CharacterName => m_characterName;

    protected Enums.Races m_race;
    public Enums.Races Race => m_race;

    protected Enums.Classes m_Class;
    public Enums.Classes Class => m_Class;

    protected Enums.Rarity m_rarity;
    public Enums.Rarity Êarity => m_rarity;

    protected float m_maxHealth;
    public float MaxHealth
    {
        get => m_maxHealth;
        set
        {
            m_maxHealth = value;
        }
    }

    protected float m_health;
    public float Health
    {
        get => m_health;
        private set
        {
            m_health = value;
            healthBar.SetHealth(m_health, 1);
        }
    }

    protected int m_speed;
    public int Speed
    {
        get => m_speed;
        set => m_speed = value;
    }

    protected float m_maxSpeed;
    public float MaxSpeed
    {
        get => m_maxSpeed;
        set => m_maxSpeed = value;
    }

    protected float m_physAttack;
    public float PhysAttack
    {
        get => m_physAttack;
        set
        {
            m_physAttack = value;
            if (m_physAttack < 1)
            {
                m_physAttack = 1;
            }
        }
    }

    protected float m_magAttack;
    public float MagAttack
    {
        get => m_magAttack;
        set
        {
            m_magAttack = value;
            if (m_magAttack < 1)
            {
                m_magAttack = 1;
            }
        }
    }

    protected int m_range;
    public int Range
    {
        get => m_range;
        set => m_range = value;
    }

    protected float m_physDefence;
    public float PhysDefence
    {
        get => m_physDefence;
        set
        {
            m_physDefence = value;
            if (m_physDefence < 1)
            {
                m_physDefence = 1;
            }
        }
    }

    protected float m_magDefence;
    public float MagDefence
    {
        get => m_magDefence;
        set
        {
            m_magDefence = value;
            if (m_magDefence < 1)
            {
                m_magDefence = 1;
            }
        }
    }

    protected float m_critChance;
    public float CritChance
    {
        get => m_critChance;
        set => m_critChance = value;
    }

    protected float m_critNum;
    public float CritNum
    {
        get => m_critNum;
        set => m_critNum = value;
    }

    protected float m_chanceToFreeAttack = 0;
    public float ChanceToFreeAttack
    {
        get => m_chanceToFreeAttack;
        set => m_chanceToFreeAttack = value;
    }

    protected float m_chanceToAvoidDamage = 0;
    public float ChanceToAvoidDamage
    {
        get => m_chanceToAvoidDamage;
        set => m_chanceToAvoidDamage = value;
    }


    protected float m_lastHealmount;
    public float LastHealAmount
    {
        get => m_lastHealmount;
        private set => m_lastHealmount = value;
    }

    protected float m_lastDamageAmount;
    public float LastDamageAmount
    {
        get => m_lastDamageAmount;
        private set => m_lastDamageAmount = value;
    }

    protected float m_physDamageMultiplier;
    public float PhysDamageMultiplier
    {
        get => m_physDamageMultiplier;
        set => m_physDamageMultiplier = value;
    }

    protected float m_magDamageMultiplier;
    public float MagDamageMultiplier
    {
        get => m_magDamageMultiplier;
        set => m_magDamageMultiplier = value;
    }

    protected Character m_lastAttackedCharacter;
    public Character LastAttackedCharacter
    {
        get => m_lastAttackedCharacter;
        private set => m_lastAttackedCharacter = value;
    }

    protected bool m_isChosen = false;
    public bool IsChosen
    {
        get
        {
            return m_isChosen;
        }
        set
        {
            m_isChosen = value;
            SetEnabledToHealthBar(m_isChosen);
            SetOutlineState(IsChosen);
        }
    }

    private bool m_canBeDamaged;
    public bool CanBeDamaged
    {
        get => m_canBeDamaged;
        set => m_canBeDamaged = value;
    }

    private bool m_canDamage;
    public bool CanDamage
    {
        get => m_canDamage;
        set => m_canDamage = value;
    }

    protected bool m_isAttackedOnTheMove = false;
    public bool IsAttackedOnTheMove
    {
        get => m_isAttackedOnTheMove;
    }

    private bool m_ignorePhysDamage;
    public bool IgnorePhysDamage
    {
        get => m_ignorePhysDamage;
        set => m_ignorePhysDamage = value;
    }

    private bool m_ignoreMagDamage;
    public bool IgnoreMagDamage
    {
        get => m_ignoreMagDamage;
        set => m_ignoreMagDamage = value;
    }
    private bool m_ignoreMoveCost;
    public bool IgnoreMoveCost
    {
        get => m_ignoreMoveCost;
        set => m_ignoreMoveCost = value;
    }
    private bool m_ignoreMoveCostTroughtSwamp;
    public bool IgnoreMoveCostTroughtSwamp
    {
        get => m_ignoreMoveCostTroughtSwamp;
        set => m_ignoreMoveCostTroughtSwamp = value;
    }

    private bool m_IsFreezed;
    public bool IsFreezed
    {
        get => m_IsFreezed;
        set
        {
            m_IsFreezed = value;
            if (m_IsFreezed)
            {
                Speed = 0;
            }
        }
    }

    protected bool m_canUseAbilities = false;
    public bool CanUseAbilities
    {
        get => m_canUseAbilities;
        set => m_canUseAbilities = value;
    }

    protected bool m_isAttackAbilityUsed = false;
    public bool IsAttackAbilityUsed
    {
        get => m_isAttackAbilityUsed;
        set => m_isAttackAbilityUsed = value;
    }

    protected bool m_isDefenceAbilityUsed = false;
    public bool IsDefenceAbilityUsed
    {
        get => m_isDefenceAbilityUsed;
        set => m_isDefenceAbilityUsed = value;
    }
    protected bool m_isBuffAbilityUsed = false;
    public bool IsBuffAbilityUsed
    {
        get => m_isBuffAbilityUsed;
        set => m_isBuffAbilityUsed = value;
    }

    protected int m_useAbilityCost = 11;
    public int UseAbilityCost
    {
        get => m_useAbilityCost;
        set => m_useAbilityCost = value;
    }

    protected int m_index = 0;
    public int Index => m_index;
    public Vector2 PositionOnField
    {
        get
        {
            return transform.GetComponentInParent<Cell>().CellIndex;
        }
    }

    public Cell ParentCell
    {
        get
        {
            return transform.GetComponentInParent<Cell>();
        }
    }

    private BasePassiveCharacterAbility m_passiveCharacterAbility;
    public BasePassiveCharacterAbility PassiveCharacterAbility => m_passiveCharacterAbility;

    private BaseCharacterAbility m_attackCharacterAbility;
    public BaseCharacterAbility AttackCharacterAbility => m_attackCharacterAbility;

    private BaseCharacterAbility m_defenceCharacterAbility;
    public BaseCharacterAbility DefenceCharacterAbility => m_defenceCharacterAbility;

    private BaseCharacterAbility m_buffCharacterAbility;
    public BaseCharacterAbility BuffCharacterAbility => m_buffCharacterAbility;

    private Dictionary<Enums.Classes, float> m_attackMultiplierByClassesDict = new();
    public Dictionary<Enums.Classes, float> AttackMultiplierByClassesDict => m_attackMultiplierByClassesDict;

    private Dictionary<Enums.Races, float> m_attackMultiplierByRacesDict = new();
    public Dictionary<Enums.Races, float> AttackMultiplierByRacesDict => m_attackMultiplierByRacesDict;

    private Dictionary<Enums.Races, float> m_damageMultiplierByRacesDict = new();
    public Dictionary<Enums.Races, float> DamageMultiplierByRacesDict => m_damageMultiplierByRacesDict;

    private Dictionary<Enums.Classes, float> m_damageMultiplierByClassesDict = new();
    public Dictionary<Enums.Classes, float> DamageMultiplierByClassesDict => m_damageMultiplierByClassesDict;


    private Dictionary<Enums.Classes, bool> m_canBeDamagedByClassesDict = new();
    public Dictionary<Enums.Classes, bool> CanBeDamagedByClassesDict => m_canBeDamagedByClassesDict;

    public event Action<Character> OnAttack;
    public event Action<Character, string, float> OnHeal;
    public event Action<Character> OnDeath;
    public event Action<Character, string, float> OnDamaged;

    public event Action<Character> OnPositionOnFieldChanged;

    public virtual void SetData(CharacterCard card, int currentIndex)
    {
        List<SpawnpointReference> points = transform.GetComponentsInChildren<SpawnpointReference>().ToList();
        topPoint = points.Where(x => Enums.Directions.top == x.Direction).First().gameObject.transform;
        middlePoint = points.Where(x => Enums.Directions.middle == x.Direction).First().gameObject.transform;
        bottomPoint = points.Where(x => Enums.Directions.bottom == x.Direction).First().gameObject.transform;


        if (healthBar == null)
        {
            healthBar = gameObject.GetComponentInChildren<HealthBar>(true);
        }
        healthBar.gameObject.SetActive(false);

        m_card = card;
        m_characterName = m_card.cardName;
        m_race = m_card.race;
        m_Class = m_card.Class;
        m_rarity = m_card.rarity;
        m_maxHealth = m_card.health;
        Health = m_card.health;
        m_speed = m_card.speed;
        m_maxSpeed = m_card.speed;
        m_physAttack = m_card.physAttack;
        m_magAttack = m_card.magAttack;
        m_range = m_card.range;
        m_physDefence = m_card.physDefence;
        m_magDefence = m_card.magDefence;
        m_critChance = m_card.critChance;
        m_critNum = m_card.critNum;

        m_chanceToFreeAttack = 0;
        if (m_card.passiveCharacterAbilityData != null)
        {
            Type type = m_card.passiveCharacterAbilityData.passiveCharacterAbility.Type;
            m_passiveCharacterAbility = (BasePassiveCharacterAbility)gameObject.AddComponent(type);
            if (m_card.passiveCharacterAbilityData != null)
            {
                m_passiveCharacterAbility.baseAbilityData = m_card.passiveCharacterAbilityData;
            }

        }
        if (m_card.attackCharacterAbilityData != null)
        {
            Type type = m_card.attackCharacterAbilityData.characterAbility.Type;
            m_attackCharacterAbility = (BaseCharacterAbility)gameObject.AddComponent(type);
            m_attackCharacterAbility.SetAbilityType(Enums.TypeOfAbility.attack);
        }

        if (m_card.defenceCharacterAbilityData != null)
        {
            Type type = m_card.defenceCharacterAbilityData.characterAbility.Type;
            m_defenceCharacterAbility = (BaseCharacterAbility)gameObject.AddComponent(type);
            m_defenceCharacterAbility.SetAbilityType(Enums.TypeOfAbility.defence);

        }

        if (m_card.buffCharacterAbilityData != null)
        {
            Type type = m_card.buffCharacterAbilityData.characterAbility.Type;
            m_buffCharacterAbility = (BaseCharacterAbility)gameObject.AddComponent(type);
            m_buffCharacterAbility.SetAbilityType(Enums.TypeOfAbility.buff);

        }

        m_physDamageMultiplier = 1;
        m_magDamageMultiplier = 1;
        m_useAbilityCost = 11;

        foreach (Enums.Classes characterClass in Enum.GetValues(typeof(Enums.Classes)))
        {
            m_canBeDamagedByClassesDict.Add(characterClass, true);
        }
        foreach (Enums.Races characterRace in Enum.GetValues(typeof(Enums.Races)))
        {
            m_damageMultiplierByRacesDict.Add(characterRace, 1);
            m_attackMultiplierByRacesDict.Add(characterRace, 1);
        }
        foreach (Enums.Classes characterClass in Enum.GetValues(typeof(Enums.Classes)))
        {
            m_attackMultiplierByClassesDict.Add(characterClass, 1);
            m_damageMultiplierByClassesDict.Add(characterClass, 1);
        }
        OnClick += OnCharacterClickedInvoke;

        OnHoverEnter += EnableHealthBar;
        OnHoverExit += DisableHealthBar;

        OnDamaged += (x, y, z) => InstantiateEffectOnCharacter(card.damageEffect);
        OnHeal += (x, y, z) => InstantiateEffectOnCharacter(card.healEffect);
        OnAttack += (x) => InstantiateEffectOnCharacter(card.attackEffect);

        IsChosen = false;
        CanBeDamaged = true;
        CanDamage = true;
        CanUseAbilities = true;
        IgnorePhysDamage = false;
        IgnoreMagDamage = false;
    }

    public GameObject InstantiateEffectOnCharacter(EffectData effectData)
    {

        if (effectData.effect != null)
        {
            GameObject spawnedEffect;
            switch (effectData.spawnPoint)
            {
                case Enums.Directions.top:
                    spawnedEffect = Instantiate(effectData.effect, Vector3.zero, Quaternion.identity, topPoint);
                    break;
                case Enums.Directions.bottom:
                    spawnedEffect = Instantiate(effectData.effect, Vector3.zero, Quaternion.identity, bottomPoint);
                    break;
                case Enums.Directions.middle:
                    spawnedEffect = Instantiate(effectData.effect, Vector3.zero, Quaternion.identity, middlePoint);
                    break;
                default:
                    spawnedEffect = Instantiate(effectData.effect, Vector3.zero, Quaternion.identity, middlePoint);
                    break;
            }
            spawnedEffect.transform.localPosition = Vector3.zero;
            return spawnedEffect;
        }
        else
        {
            Debug.LogError("null effect");
            return null;
        }

    }

    public virtual bool Damage(Character chosenCharacter)
    {
        LastAttackedCharacter = chosenCharacter;

        if (!CanBeDamaged)
        {
            OnDamaged?.Invoke(this, chosenCharacter.CharacterName, 0);
            LastDamageAmount = 0;
            return false;
        }

        if (IsDamageAvoided())
        {
            OnDamaged?.Invoke(this, chosenCharacter.CharacterName, 0);
            LastDamageAmount = 0;
            return false;
        }

        if (!CanBeDamagedByClass(chosenCharacter.Class))
        {
            OnDamaged?.Invoke(this, chosenCharacter.CharacterName, 0);
            LastDamageAmount = 0;
            return false;
        }

        float crit = IsCrit(chosenCharacter.CritChance, chosenCharacter.CritNum);
        float finalPhysDamage = IgnorePhysDamage ? 0 : ((11 + chosenCharacter.PhysAttack) * chosenCharacter.PhysAttack * crit * (chosenCharacter.PhysAttack - PhysDefence + m_maxHealth)) / 256 * m_physDamageMultiplier;
        float finalMagDamage = IgnoreMagDamage ? 0 : ((11 + chosenCharacter.MagAttack) * chosenCharacter.MagAttack * crit * (chosenCharacter.MagAttack - MagDefence + m_maxHealth)) / 256 * m_magDamageMultiplier;
        float finalDamage = Math.Max(finalMagDamage, finalPhysDamage) * GetDamageMultiplierByRace(chosenCharacter.Race) * GetDamageMultiplierByClass(chosenCharacter.Class) * chosenCharacter.GetAttackMultiplierByRace(Race) * chosenCharacter.GetAttackMultiplierByClass(Class);

        Health = Math.Max(0, Health - finalDamage);

        LastDamageAmount = finalDamage;

        OnDamaged?.Invoke(this, chosenCharacter.CharacterName, finalDamage);
        if (Health == 0)
        {
            OnDeath?.Invoke(this);
        }

        return Health == 0;
    }

    public virtual bool Damage(Character chosenCharacter, string abilityName, float damage)
    {
        LastAttackedCharacter = chosenCharacter;

        if (!CanBeDamaged)
        {
            OnDamaged?.Invoke(this, abilityName, 0);
            LastDamageAmount = 0;
            return false;
        }

        if (IsDamageAvoided())
        {
            LastDamageAmount = 0;
            return false;
        }

        float crit = IsCrit(chosenCharacter.CritChance, chosenCharacter.CritNum);
        float finalPhysDamage = ((11 + damage) * damage * crit * (damage - PhysDefence + m_maxHealth)) / 256;
        float finalMagDamage = ((11 + damage) * damage * crit * (damage - MagDefence + m_maxHealth)) / 256;
        float finalDamage = Math.Max(finalMagDamage, finalPhysDamage);

        if (finalDamage == 0)
        {
            finalDamage = UnityEngine.Random.Range(0.01f, 0.1f);
        }
        Health = Math.Max(0, Health - finalDamage);

        LastDamageAmount = finalDamage;

        OnDamaged?.Invoke(this, abilityName, finalDamage);
        if (Health == 0)
        {
            OnDeath?.Invoke(this);
        }

        return Health == 0;
    }

    public bool Damage(float damage, string nameObject)
    {
        if (!CanBeDamaged)
        {
            OnDamaged?.Invoke(this, nameObject, 0);
            LastDamageAmount = 0;
            return false;
        }

        if (IsDamageAvoided())
        {
            OnDamaged?.Invoke(this, nameObject, 0);
            LastDamageAmount = 0;
            return false;
        }

        LastDamageAmount = damage;
        Health = Math.Max(0, Health - damage);

        OnDamaged?.Invoke(this, nameObject, damage);
        if (Health == 0)
        {
            OnDeath?.Invoke(this);
        }
        return Health == 0;
    }
    public float GetAttackMultiplierByClass(Enums.Classes characterClass)
    {
        return m_attackMultiplierByClassesDict[characterClass];
    }
    private float GetDamageMultiplierByRace(Enums.Races characterRace)
    {
        return m_damageMultiplierByRacesDict[characterRace];
    }
    public float GetAttackMultiplierByRace(Enums.Races characterRace)
    {
        return m_attackMultiplierByRacesDict[characterRace];
    }
    private float GetDamageMultiplierByClass(Enums.Classes characterClass)
    {
        return m_damageMultiplierByClassesDict[characterClass];
    }

    private bool CanBeDamagedByClass(Enums.Classes characterClass)
    {
        return m_canBeDamagedByClassesDict[characterClass];
    }
    protected bool IsDamageAvoided()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < ChanceToAvoidDamage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected float IsCrit(float critChance, float m_critNum)
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < critChance)
        {
            return m_critNum;
        }
        else
        {
            return 1;
        }
    }

    public void Heal(float amount, string nameObject)
    {
        float temp = Health + amount;
        if (temp > m_maxHealth)
        {
            m_lastHealmount = m_maxHealth - Health;
            Health = m_maxHealth;
            OnHeal?.Invoke(this, nameObject, m_lastHealmount);
        }
        else
        {
            Health = temp;
            m_lastHealmount = amount;
            OnHeal?.Invoke(this, nameObject, amount);
        }

    }

    public void HealMoreThenMax(float amount, string nameObject)
    {
        Health += amount;
        OnHeal?.Invoke(this, nameObject, amount);
    }

    public void Move(int moveCost, Transform positionToMove, float moveTime = 0.5f)
    {
        Speed -= Convert.ToInt32(moveCost);

        Vector3 cellToMovePos = positionToMove.position;
        transform.DOMove(new Vector3(cellToMovePos.x, transform.position.y, cellToMovePos.z), moveTime).OnComplete(() =>
        {
            transform.SetParent(positionToMove);
            transform.localPosition = Vector3.zero;
            OnPositionOnFieldChanged?.Invoke(this);
        });
    }
    public void ResetCharacter()
    {
        if (!IsFreezed)
        {
            m_speed = (int)m_maxSpeed;
        }
        m_isAttackedOnTheMove = false;
        m_useAbilityCost = 11;
    }

    public void RemoveDebuffs()
    {
        Debug.Log("Stats is normal");
        IsFreezed = false;
        CanDamage = true;
        CanUseAbilities = true;


        if (m_physAttack < m_card.physAttack) m_physAttack = m_card.physAttack;
        if (m_magAttack < m_card.magAttack) m_magAttack = m_card.magAttack;
        if (m_range < m_card.range) m_range = m_card.range;
        if (m_physDefence < m_card.physDefence) m_physDefence = m_card.physDefence;
        if (m_magDefence < m_card.magDefence) m_magDefence = m_card.magDefence;
        if (m_critChance < m_card.critChance) m_critChance = m_card.critChance;
        if (m_critNum < m_card.critNum) m_critNum = m_card.critNum;
    }
    public void UseAtackAbility()
    {
        m_attackCharacterAbility.SelectCard();
    }
    public void UseDefenceAbility()
    {
        m_defenceCharacterAbility.SelectCard();
    }
    public void UseBuffAbility()
    {
        m_buffCharacterAbility.SelectCard();
    }

    public void OnAttackInvoke()
    {
        m_isAttackedOnTheMove = true;
        OnAttack?.Invoke(this);
    }
    protected void OnCharacterClickedInvoke(GameObject gameObject)
    {
        IsChosen = true;
    }


    private void SetEnabledToHealthBar(bool state)
    {
        if (state)
        {
            EnableHealthBar(null);
        }
        else
        {
            DisableHealthBar(null);
        }
    }
    private void DisableHealthBar(GameObject gameObject)
    {
        healthBar.gameObject.SetActive(false);
    }

    private void EnableHealthBar(GameObject gameObject)
    {
        healthBar.gameObject.SetActive(true);
    }
    protected override void SubscribeOnMouseExit()
    {
        m_collider.OnMouseExitAsObservable().Where(x => IsEnabled).Subscribe(x =>
        {
            if (!IsChosen)
            {
                OnHoverExitInvoke();
            }
        }).AddTo(disposables);
    }
}
