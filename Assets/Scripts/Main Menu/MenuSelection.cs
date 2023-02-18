using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private List<SelectableElement> _uiElements;
    [SerializeField] private float _scaleDuration;
    [System.Serializable]
    public class SelectableElement
    {
        public GameObject _element;
        public UnityEvent _OnEnter;
        public float _originalSize;
        public float _selectedSize;
    }
    private int _currentSelectionIndex;

    // Debounce
    private int _lastAxisInput;
    private bool _selectionMade;

    // Start is called before the first frame update
    private void Start()
    {
        _currentSelectionIndex = 0;
        if (_uiElements.Count == 0)
        {
            Debug.LogError("MenuSelection: No UI Elements to select!");
        }
        foreach (SelectableElement selectableElement in _uiElements)
        {
            selectableElement._element.transform.localScale = new Vector3(selectableElement._originalSize, selectableElement._originalSize, selectableElement._originalSize);
        }
        _lastAxisInput = 0;
        _selectionMade = false;
    }
    // Update is called once per frame
    private void Update()
    {
        ParseInput();
        ShowSelection();
        for (int i = 0; i < _uiElements.Count; i++)
        {
            if (i != _currentSelectionIndex)
            {
                continue;
            }
            else
            {
                if (Input.GetAxisRaw("Submit") == 0)
                {
                    continue;
                }
                if (Input.GetAxisRaw("Submit") == 1 && !_selectionMade)
                {
                    _selectionMade = true;
                    _uiElements[i]._OnEnter?.Invoke();
                }
            }
        }
    }
    void ParseInput()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            _lastAxisInput = 0;
            return;
        }
        if ((Input.GetAxisRaw("Horizontal") == -1) && (_lastAxisInput != -1))
        {
            if (_currentSelectionIndex - 1 < 0)
            {
                _lastAxisInput = -1;
                return;
            }
            _currentSelectionIndex--;
            _lastAxisInput = -1;
        }
        if ((Input.GetAxisRaw("Horizontal") == 1) && (_lastAxisInput != -1))
        {
            if (_currentSelectionIndex + 1 >= _uiElements.Count)
            {
                _lastAxisInput = 1;
                return;
            }
            _currentSelectionIndex++;
            _lastAxisInput = 1;
        }
    }
    void ShowSelection()
    {
        for (int i = 0; i < _uiElements.Count; i++)
        {
            if (i != _currentSelectionIndex)
            {
                if (_uiElements[i]._element.transform.localScale.x != _uiElements[i]._selectedSize)
                {
                    continue;
                }
                _uiElements[i]._element.transform.DOScale(_uiElements[i]._originalSize, _scaleDuration);
                continue;
            }
            else
            {
                if (_uiElements[i]._element.transform.localScale.x != _uiElements[i]._originalSize)
                {
                    continue;
                }
                _uiElements[i]._element.transform.DOScale(_uiElements[i]._selectedSize, _scaleDuration);
            }
        }
    }
}
