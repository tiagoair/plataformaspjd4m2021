using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDPressSpaceTextController : MonoBehaviour
{
    private TMP_Text _myText;

    private void OnEnable()
    {
        HUDObserverManager.ONPlayerVictory += OnPressSpaceVisible;
        HUDObserverManager.ONPlayerDied += OnPressSpaceVisible;
    }

    private void OnDisable()
    {
        HUDObserverManager.ONPlayerVictory -= OnPressSpaceVisible;
        HUDObserverManager.ONPlayerDied -= OnPressSpaceVisible;
    }

    private void Awake()
    {
        _myText = GetComponent<TMP_Text>();
    }
    
    private void OnPressSpaceVisible(bool obj)
    {
        if(obj) _myText.color = Color.white;
        else _myText.color = Color.clear;
    }
}
