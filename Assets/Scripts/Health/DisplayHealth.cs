using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisplayHealth : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider _healthSlider;

    [Header("Tweening Values")]
    [SerializeField] private float _easeDuration;

    [Header("Fading Values")]
    [SerializeField] private bool _fading;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _appearDuration;
    [SerializeField] private float _appearDurationTimer;

    // Debounce
    private bool _tweeningHealth;
    private bool _firstStateIteration;
    
    // State
    private bool _hidden;

    private void Start()
    {
        if (_fading)
        {
            _canvasGroup.alpha = 0;
            _hidden = true;
        }
        else
        {
            _canvasGroup.alpha = 1;
            _hidden = false;
        }
    }
    private void Update()
    {
        SetStateVariables();
        if (!_hidden)
        {
            _appearDurationTimer += Time.deltaTime;
        }
        if ((_appearDurationTimer > _appearDuration) && _fading)
        {
            _canvasGroup.DOFade(0, _fadeDuration);
            _appearDurationTimer = 0;
        }
    }
    private void SetStateVariables()
    {
        if (_canvasGroup.alpha == 0)
        {
            _hidden = true;
        }
        else if (_canvasGroup.alpha == 1)
        {
            _hidden = false;
        }
    }
    public void OnHealthChanged(float health, float maxHealth)
    {
        _healthSlider.DOValue(health / maxHealth, _easeDuration);
        if (_hidden)
        {
            _canvasGroup.DOFade(1, _fadeDuration);
        }
    }
}

    

