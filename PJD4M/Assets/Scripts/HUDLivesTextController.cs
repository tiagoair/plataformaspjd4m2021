using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDLivesTextController : MonoBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private GameObject energyBar;
    
    
    private TMP_Text _myText;

    private void OnEnable()
    {
        HUDObserverManager.ONLivesChanged += OnLivesChanged;
    }

    private void OnDisable()
    {
        HUDObserverManager.ONLivesChanged -= OnLivesChanged;
    }

    private void Awake()
    {
        _myText = GetComponent<TMP_Text>();
    }

    private void OnLivesChanged(int obj)
    {
        if (obj < 0)
        {
            _myText.color = Color.clear;
            playerIcon.color = Color.clear;
            energyBar.SetActive(false);
        }
        else
        {
            _myText.color = Color.white;
            playerIcon.color = Color.white;
            _myText.text = "x"+obj;
            energyBar.SetActive(true);
        }
    }
}
