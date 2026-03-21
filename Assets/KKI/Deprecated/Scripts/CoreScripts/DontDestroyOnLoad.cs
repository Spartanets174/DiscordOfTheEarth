using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private string objectID;
    private List<DontDestroyOnLoad> dontDestroys;
    void Awake()
    {
        objectID = name + transform.position.ToString();
        dontDestroys = FindObjectsOfType<DontDestroyOnLoad>().ToList();
        for (int i = 0; i < dontDestroys.Count; i++)
        {
            if (dontDestroys[i]!=this)
            {
                if (dontDestroys[i].objectID==objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
