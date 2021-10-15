using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCanvasController : MonoBehaviour
{
    public static HUDCanvasController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
