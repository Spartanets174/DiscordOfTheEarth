using DG.Tweening;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChosenCharacterCardDisplay : MonoBehaviour
{
    [SerializeField]
    private Image charImage;
    [SerializeField]
    private TextMeshProUGUI charStatsText;
    [SerializeField]
    private TextMeshProUGUI charDescription;
    [SerializeField]
    private TextMeshProUGUI charAbilities;
    [SerializeField]
    private Color attackColor;
    [SerializeField]
    private Color defenceColor;
    [SerializeField]
    private Color buffColor;
    [SerializeField]
    private Color passiveColor;

    [HideInInspector]
    public ReactiveProperty<CharacterCard> chosenCharCard;

    private void Start()
    {
        chosenCharCard.Value = null;
    }

    public void SetCharacterData(GameObject characterCardObject)
    {
        CharacterCard characterCard = characterCardObject.GetComponent<CardDisplay>().Card;
        chosenCharCard.Value = characterCard;
        charImage.sprite = characterCard.image;
        charImage.DOFade(1, 0);
        charStatsText.text = $"ЗД: {characterCard.health * 100}" + "\n" +
                    $"ФА: {characterCard.physAttack * 100}" + "\n" +
                    $"МА: {characterCard.magAttack * 100}" + "\n" +
                    $"ФЗ: {characterCard.physDefence * 100}" + "\n" +
                    $"МЗ: {characterCard.magDefence * 100}" + "\n" +
                    $"ВК: {characterCard.critChance * 100}";
        charDescription.text = characterCard.description;
        charAbilities.text = $"<color=#{attackColor.ToHexString()}>Атакующая способность</color>: {characterCard.attackAbility}" + "\n" + "\n" +
                    $"<color=#{defenceColor.ToHexString()}>Защитная способность</color>: {characterCard.defenceAbility}" + "\n" + "\n" +
                    $"<color=#{buffColor.ToHexString()}>Усиливающая способность</color>: {characterCard.buffAbility}" + "\n" + "\n" +
                    $"<color=#{passiveColor.ToHexString()}>Пассивная способность</color> {characterCard.passiveAbility}";

    }

    public void SetOnlyData(GameObject characterCardObject)
    {
        CharacterCard characterCard = characterCardObject.GetComponent<DeckCharacterCardDisplay>().CurrentCharacterCard;
        chosenCharCard.Value = null;
        charImage.sprite = characterCard.image;
        charImage.DOFade(1, 0);
        charStatsText.text = $"ЗД: {characterCard.health * 100}" + "\n" +
                    $"ФА: {characterCard.physAttack * 100}" + "\n" +
                    $"МА: {characterCard.magAttack * 100}" + "\n" +
                    $"ФЗ: {characterCard.physDefence * 100}" + "\n" +
                    $"МЗ: {characterCard.magDefence * 100}" + "\n" +
                    $"ВК: {characterCard.critChance * 100}";
        charDescription.text = characterCard.description;
        charAbilities.text = $"<color=#{attackColor.ToHexString()}>Атакующая способность</color>: {characterCard.attackAbility}" + "\n" + "\n" +
                    $"<color=#{defenceColor.ToHexString()}>Защитная способность</color>: {characterCard.defenceAbility}" + "\n" + "\n" +
                    $"<color=#{buffColor.ToHexString()}>Усиливающая способность</color>: {characterCard.buffAbility}" + "\n" + "\n" +
                    $"<color=#{passiveColor.ToHexString()}>Пассивная способность</color> {characterCard.passiveAbility}";

    }
}
