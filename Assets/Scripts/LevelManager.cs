using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    
    [SerializeField] private GameObject _levelNameObject;
    
    [SerializeField] private List<GameObject> _levelPrefabs;
    private int _currentLevelIndex = 0;
    public GameObject CurrentLevelObject => _levelPrefabs[_currentLevelIndex];
    
    private void Awake()
    {  
        _instance ??= this;
    }

    public void EndLevel()
    {
        Debug.Log("Ending level");
    }
    
    [Button("Load Next Level")]
    public void LoadNextLevel()
    {
        if (_currentLevelIndex <= _levelPrefabs.Count - 1)
        {
            var levelName = _levelPrefabs[_currentLevelIndex].GetComponent<Level>().LevelName;
            _levelNameObject.GetComponent<LevelNameMovements>()
                .MoveObjectAndSetText(levelName, () =>
                {
                    Debug.Log("Loading level: " + _levelPrefabs[_currentLevelIndex].name);
                    Instantiate(_levelPrefabs[_currentLevelIndex], Vector2.zero, Quaternion.identity);
                    _currentLevelIndex++;
                });
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }
}
