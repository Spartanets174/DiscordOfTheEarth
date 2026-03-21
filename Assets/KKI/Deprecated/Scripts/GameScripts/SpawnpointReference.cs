using UnityEngine;

public class SpawnpointReference : MonoBehaviour
{
    [SerializeField]
    private Enums.Directions direction;
    public Enums.Directions Direction => direction;
}
