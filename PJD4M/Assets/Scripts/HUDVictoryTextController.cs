using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDVictoryTextController : MonoBehaviour
{
    private TMP_Text _myText;

    private void OnEnable()
    {
        HUDObserverManager.ONPlayerVictory += OnPlayerVictory;
    }

    private void OnDisable()
    {
        HUDObserverManager.ONPlayerVictory -= OnPlayerVictory;
    }

    private void Awake()
    {
        _myText = GetComponent<TMP_Text>();
    }
    
    private void OnPlayerVictory(bool obj)
    {
        if(obj) _myText.color = Color.white;
        else _myText.color = Color.clear;
    }
}
