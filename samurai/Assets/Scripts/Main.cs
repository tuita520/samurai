using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏入口，一般放在splash页面，保证不会重复执行
/// </summary>
public class Main : MonoBehaviour
{
    public List<GameObject> globalObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        foreach (var item in globalObjects)
        {
            DontDestroyOnLoad(item);
        }        
    }    
}
