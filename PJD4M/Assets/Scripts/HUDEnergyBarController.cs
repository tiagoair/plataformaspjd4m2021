using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEnergyBarController : MonoBehaviour
{
    private Slider _mySlider;

    private void OnEnable()
    {
        HUDObserverManager.ONPlayerEnergyChanged += OnPlayerEnergyChanged;
    }

    private void OnDisable()
    {
        HUDObserverManager.ONPlayerEnergyChanged -= OnPlayerEnergyChanged;
    }

    private void Awake()
    {
        _mySlider = GetComponent<Slider>();
    }

    private void OnPlayerEnergyChanged(float obj)
    {
        _mySlider.value = obj;
    }
}
