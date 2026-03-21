using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapperController : MonoBehaviour
{
    [SerializeField]
    private BootMode bootMode;

    [SerializeField]
    private List<GameObject> bootstrappers;
    

    void Start()
    {
        if (bootMode == BootMode.Start)
        {
            InitBootstrappers();
        }
    }

    private void Awake()
    {
        if (bootMode == BootMode.Awake)
        {
            InitBootstrappers();
        }
    }

    private void InitBootstrappers()
    {
        foreach (var bootstrap in bootstrappers)
        {
            if (bootstrap.TryGetComponent(out ILoadable loadable))
            {
                loadable.Init();
            }
            else
            {
                Debug.LogError($"Объект {bootstrap} не инициализирован!");
            }
        }
        
    }
}
public enum BootMode
{
    Start,
    Awake
}