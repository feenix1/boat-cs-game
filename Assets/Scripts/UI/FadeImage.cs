using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    [SerializeField] private Image _imageToFade;
    [SerializeField] private float _fadeDuration;
    [SerializeField][Range(0, 1)] private float _minOpacity;
    [SerializeField][Range(0, 1)] private float _maxOpacity;

    public UnityEvent _OnFadeInComplete;
    public UnityEvent _OnFadeOutComplete;

    // Start is called before the first frame update
    private void Start()
    {
        if (_imageToFade == null)
        {
            Debug.LogError("FadeImage: Image is null!");
        }
    }
    public void FadeIn()
    {
        _imageToFade.DOFade(_maxOpacity, _fadeDuration);
        StartCoroutine(InvokeFadeInEvent());
    }
    public void FadeOut()
    {
        _imageToFade.DOFade(_minOpacity, _fadeDuration);
        StartCoroutine(InvokeFadeOutEvent());
    }
    private IEnumerator InvokeFadeInEvent()
    {
        yield return new WaitForSeconds(_fadeDuration);
        _OnFadeInComplete?.Invoke();
    }
    private IEnumerator InvokeFadeOutEvent()
    {
        yield return new WaitForSeconds(_fadeDuration);
        _OnFadeOutComplete?.Invoke();
    }
}
