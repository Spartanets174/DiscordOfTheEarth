using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSupportCardDisplay : OutlineClicableUI
{
    [SerializeField]
    private Image supportImage;
    [SerializeField]
    private Image background;
    [SerializeField]
    private TextMeshProUGUI supportAbility;
    [SerializeField]
    private TextMeshProUGUI supportName;
    [SerializeField]
    private Sprite normalBackground;
    [SerializeField]
    private Sprite mythBackground;
    [SerializeField]
    private DragAndDropComponent m_dragAndDropComponent;
    public DragAndDropComponent DragAndDropComponent => m_dragAndDropComponent;

    private CardSupport m_currentCardSupport;
    public CardSupport CurrentCardSupport => m_currentCardSupport;
    private BaseSupport—ardAbility m_gameSupport—ardAbility;
    public BaseSupport—ardAbility GameSupport—ardAbility => m_gameSupport—ardAbility;
    private void Awake()
    {
        m_dragAndDropComponent.OnBeginDragEvent += OnBeginDrag;
        m_dragAndDropComponent.OnEndDragEvent += OnEndDrag;
    }
    private void OnBeginDrag(GameObject gameObject)
    {
        SetBlockerState(true);
    }
    private void OnEndDrag(GameObject gameObject)
    {
        transform.DOLocalMove(DragAndDropComponent.StartPos, 0.5f);
        SetBlockerState(false);
    }

    private void OnDestroy()
    {
        m_dragAndDropComponent.OnBeginDragEvent -= OnBeginDrag;
        m_dragAndDropComponent.OnEndDragEvent -= OnEndDrag;
    }

    public void SetData(CardSupport cardSupport, BattleSystem battleSystem)
    {
        m_currentCardSupport = cardSupport;
        supportImage.sprite = cardSupport.image;
        supportAbility.text = cardSupport.abilityText;
        supportName.text = cardSupport.cardName;

        if (cardSupport.support—ardAbilityData != null)
        {
            Type type = cardSupport.support—ardAbilityData.Support—ardAbility.Type;
            m_gameSupport—ardAbility = (BaseSupport—ardAbility)gameObject.AddComponent(type);
            m_gameSupport—ardAbility.Init(battleSystem, cardSupport.support—ardAbilityData);
        }
        background.sprite = GetImageByRarity(cardSupport.rarity);
    }

    private Sprite GetImageByRarity(Enums.Rarity rarity)
    {
        Sprite currentRarityBackground = null;
        switch (rarity)
        {
            case Enums.Rarity.Œ·˚˜Ì‡ˇ:
                currentRarityBackground = normalBackground;
                break;
            case Enums.Rarity.ÃËÙË˜ÂÒÍ‡ˇ:
                currentRarityBackground = mythBackground;
                break;
        }
        return currentRarityBackground;

    }
}
