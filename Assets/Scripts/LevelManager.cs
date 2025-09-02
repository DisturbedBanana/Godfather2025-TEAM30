using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    
    [SerializeField] private List<Level> _levels;
    private int _currentLevelIndex = 0;
    public Level CurrentLevel => _levels[_currentLevelIndex];
    
    private void Awake()
    {  
        _instance ??= this;
    }

    public void EndLevel()
    {
        Debug.Log("Ending level");
    }
}
