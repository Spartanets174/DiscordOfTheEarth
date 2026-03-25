using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public abstract class CardController : MonoBehaviour, ILoadable
{
    protected PlayerDataController m_PlayerManager;
    public PlayerDataController PlayerDataController => m_PlayerManager;

    private List<OutlineInteractableObject> outlineInteractableObjects;
    public List<OutlineInteractableObject> OutlineInteractableObjects => outlineInteractableObjects;

    private Races m_currentRace;
    public Races CurrentRace
    {
        get => m_currentRace;
    }

    private Classes m_currentClass;
    public Classes CurrentClass
    {
        get => m_currentClass;
    }

    private TypeOfSupport m_currentTypeOfSupport;
    public TypeOfSupport CurrentTypeOfSupport 
        {
        get => m_currentTypeOfSupport;
    }
    public virtual void Init()
    {
        m_currentRace = Races.Все;
        m_currentClass = Classes.Все;
        m_currentTypeOfSupport = TypeOfSupport.Все;
        m_PlayerManager = FindAnyObjectByType<PlayerDataController>();
        outlineInteractableObjects = FindObjectsOfType<OutlineInteractableObject>().ToList();
    }

    public void SetCurrentClass(Classes cardClass)
    {
        m_currentClass = cardClass;
    }
    public void SetCurrentRace(Races cardRace)
    {
        m_currentRace = cardRace;
    }
    public void SetCurrentTypeOfSupport(TypeOfSupport cardSupportType)
    {
        m_currentTypeOfSupport = cardSupportType;
    }

    public List<CharacterCard> FilterCharacterCards(List<CharacterCard> listToFilter)
    {
        List<CharacterCard> cards= listToFilter;
        //Если выбран только класс
        if (CurrentRace == Races.Все && CurrentClass != Classes.Все)
        {
            cards = listToFilter.Where(x => x.Class == CurrentClass).ToList();
        }
        //Если выбрана только раса
        if (CurrentRace != Races.Все && CurrentClass == Classes.Все)
        {
            cards = listToFilter.Where(x => x.race == CurrentRace).ToList();
            
        }
        //Если выбраны оба параметра
        if (CurrentRace != Races.Все && CurrentClass != Classes.Все)
        {
            cards = listToFilter.Where(x => x.race == CurrentRace && x.Class == CurrentClass).ToList();
        }
        return cards;
    }

    public List<CardSupport> FilterSupportCards(List<CardSupport> listToFilter)
    {
  
        List<CardSupport> cardsSupport = listToFilter;
        if (CurrentTypeOfSupport != TypeOfSupport.Все)
        {
             cardsSupport = listToFilter.Where(x => x.type == CurrentTypeOfSupport).ToList();
        }
        return cardsSupport;
    }
}
