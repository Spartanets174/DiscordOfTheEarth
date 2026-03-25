using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardPresenter : MonoBehaviour
{

    [Space, Header("Parents")]
    [SerializeField]
    protected Transform parentToSpawnCharacterCards;
    [SerializeField]
    protected Transform parentToSpawnSupportCards;  

    protected abstract void SpawnCharacterCards();

    protected abstract void SpawnSupportCards();
}
