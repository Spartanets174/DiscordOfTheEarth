using DOTE.Gameplay.Infrastructure;
using DOTE.SharedKernel.Domain;
using DOTE.SharedKernel.Infrastructure;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIEnemyPlayerConfig", menuName = "DOTE/Gameplay/AIEnemyPlayerConfig")]
public class AIEnemyPlayerConfig : ScriptableObject
{
    [Header("Data")]
    [SerializeField]
    private GuidProperty id;
    [SerializeField]
    private string enemyPlayerName;
    [SerializeField]
    private Sprite enemyPlayerImage;

    [Header("Deck")]
    [SerializeField]
    private List<CharacterConfig> characters;

    [SerializeField]
    private List<ASupportCardConfig> supportCards;

    [Header("AI")]
    [SerializeField]
    private GameObject behaviorAlgorithmPrefab;

    public string GetId() => id.guidString;
    public string GetEnemyPlayerName() => enemyPlayerName;
    public Sprite GetEnemyPlayerImage() => enemyPlayerImage;
    public List<CharacterConfig> GetEnemyPlayerCharacters() => characters;
    public List<ASupportCardConfig> GetEnemylayerSupportCards() => supportCards;
    public GameObject GetBehaviorAlgorithmPrefab() => behaviorAlgorithmPrefab;
}
