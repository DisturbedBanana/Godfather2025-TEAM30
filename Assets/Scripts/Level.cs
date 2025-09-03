using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Level : MonoBehaviour
{
    [SerializeField] private string _levelName;
    public string LevelName => _levelName;
    
    [SerializeField] private int _par;
    public int Par => _par;

    private void Awake()
    {
        ScoreManager.Instance?.ResetScoreTexts(Par);
    }
}