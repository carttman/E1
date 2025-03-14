using System;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    [SerializeField] private Game game;
    
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        game.goldChanged += GameOngoldChanged;
    }

    private void GameOngoldChanged(int newGold)
    {
        _textMesh.text = $"Gold: {newGold}";
    }
}
