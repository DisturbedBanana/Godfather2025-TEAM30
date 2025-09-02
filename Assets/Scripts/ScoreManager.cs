using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }
    
    [SerializeField] private int _currentScore;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _parText;

    private void Awake()
    {  
        _instance ??= this;
    }

    private void Start()
    {
        _scoreText.text = _currentScore.ToString();
    }

    public void AddScore()
    {
        _currentScore++;
        _scoreText.text = _currentScore.ToString();
    }
}
