using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ListDeadEnemies))]
public class DisplayKills : MonoBehaviour
{
    private ListDeadEnemies _listDeadEnemies;
    [SerializeField] private int _kills;
    [SerializeField] private TMPro.TextMeshProUGUI _textMeshProElement;
    [SerializeField] private string _countPrefix;

    // Start is called before the first frame update
    private void Start()
    {
        _listDeadEnemies = gameObject.GetComponent<ListDeadEnemies>();
        _kills = 0;
        UpdateText();
    }
    private void Update()
    {
        _kills = _listDeadEnemies._deadEnemies.Count;
        UpdateText();
    }
    private void UpdateText()
    {
        _textMeshProElement.text = $"{_countPrefix}{_kills.ToString()}";
    }
}
