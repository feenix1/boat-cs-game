using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class QuitOnSelected : MonoBehaviour
{
    [SerializeField] private Image _elementToFadeIn;
    [SerializeField] private float _fadeDuration;
    // Start is called before the first frame update
    private void Start()
    {
        Scene _activeScene = SceneManager.GetActiveScene();
        if (_activeScene.name == "Main Menu")
        {
            _elementToFadeIn.color = Color.clear;
        }
    }
    public void OnEnter()
    {
        StartCoroutine(OnEnterCoroutine());
    }
    private IEnumerator OnEnterCoroutine()
    {
        _elementToFadeIn.DOFade(1, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        Quit();
    }
    private void Quit()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
