using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;


public class Ritual_affordance : MonoBehaviour
{
    private int _fillAmount;
    public Slider slider;
    private GameManager _manager;

    private void Awake() {
        _manager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        slider.minValue = 0;
        slider.maxValue = _manager.MAXSLIDERVALUE;
        _manager = FindObjectOfType<GameManager>();
    }

    public IEnumerator Ritual_progression()
    {
        foreach (var item in _manager.heroes)
        {
            if(item.Health != 0)
            _fillAmount += item.BattleValue;    
        }
        slider.value = _fillAmount;
        yield break;
    }
}
