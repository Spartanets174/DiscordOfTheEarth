using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController:MonoBehaviour, ILoadable
{
    [SerializeField]
    private PlayerData m_playerData;
    public PlayerData PlayerData => m_playerData;

    private List<CharacterCard> m_allCharCards = new();
    public List<CharacterCard> AllCharCards => m_allCharCards;

    private List<CardSupport> m_allSupportCards = new();
    public List<CardSupport> AllSupportCards => m_allSupportCards;

    private List<CharacterCard> m_allShopCharCards = new();
    public List<CharacterCard> AllShopCharCards => m_allShopCharCards;

    private List<CardSupport> m_allShopSupportCards = new();
    public List<CardSupport> AllShopSupportCards => m_allShopSupportCards;

    private List<CharacterCard> m_allUserCharCards = new();
    public List<CharacterCard> AllUserCharCards => m_allUserCharCards;

    private List<CardSupport> m_allUserSupportCards = new();
    public List<CardSupport> AllUserSupportCards => m_allUserSupportCards;

    private List<CharacterCard> m_deckUserCharCards = new();
    public List<CharacterCard> DeckUserCharCards => m_deckUserCharCards;

    private List<CardSupport> m_deckUserSupportCards = new();
    public List<CardSupport> DeckUserSupportCards => m_deckUserSupportCards;

    private int m_money = 100000;
    public int Money
    {
        get=> m_money;
        set=> m_money = value;
    }

    private string m_characterName;
    public string CharacterName => m_characterName;

    public void Init()
    {
        this.m_allShopCharCards = m_playerData.allShopCharCards;
        this.m_allShopSupportCards = m_playerData.allShopSupportCards;
        this.m_allUserCharCards = m_playerData.allUserCharCards;
        this.m_allUserSupportCards = m_playerData.allUserSupportCards;
        this.m_deckUserCharCards = m_playerData.deckUserCharCards;
        this.m_deckUserSupportCards = m_playerData.deckUserSupportCards;
        this.m_money = m_playerData.money;
        this.m_characterName = m_playerData.Name;
        //playerData.allCharCards = this.allCharCards;
        //playerData.allSupportCards = this.allSupportCards;
        //playerData.allUserCharCards = this.allUserCharCards;
        //playerData.allUserSupportCards = this.allUserSupportCards;
        //playerData.deckUserCharCards = this.deckUserCharCards;
        //playerData.deckUserSupportCards = this.deckUserSupportCards;
        //playerData.money = this.money;
    }
    private void OnDestroy()
    {
        m_playerData.allShopCharCards = this.m_allShopCharCards;
        m_playerData.allShopSupportCards = this.m_allShopSupportCards;
        m_playerData.allUserCharCards = this.m_allUserCharCards;
        m_playerData.allUserSupportCards = this.m_allUserSupportCards;
        m_playerData.deckUserCharCards = this.m_deckUserCharCards;
        m_playerData.deckUserSupportCards = this.m_deckUserSupportCards;
        m_playerData.money = this.m_money;
    }

}