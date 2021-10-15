using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDDeathTextController : MonoBehaviour
{
    private TMP_Text _myText;

    private void OnEnable()
    {
        HUDObserverManager.ONPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        HUDObserverManager.ONPlayerDied -= OnPlayerDied;
    }

    private void Awake()
    {
        _myText = GetComponent<TMP_Text>();
    }
    
    private void OnPlayerDied(bool obj)
    {
        if(obj) _myText.color = Color.white;
        else _myText.color = Color.clear;
    }
}
