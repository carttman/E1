using System;
using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    [SerializeField] private Game game;
    
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        game.livesChanged += OnLivesChanged;
    }

    private void OnLivesChanged(int newLives)
    {
        _textMesh.text = $"Lives: {newLives}";
    }
}
