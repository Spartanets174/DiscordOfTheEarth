using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectBehaviour : ICardUsable
{
    public GameObject spawnedPrefab;
    public Vector3 rotation;
    private GameObject prefab;

    public SpawnObjectBehaviour(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public event Action OnCardUse;
    public void UseAbility(GameObject gameObject)
    {
        spawnedPrefab = GameObject.Instantiate(prefab,Vector3.zero,Quaternion.identity, gameObject.transform);
        spawnedPrefab.transform.localPosition =new Vector3(0, 0, 0);
        spawnedPrefab.transform.DOLocalRotate(rotation, 0);
        OnCardUse?.Invoke();
    }

}
