using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    
    [SerializeField] private GameObject _levelNameObject;
    [SerializeField] private Light2D _levelLight;
    
    [SerializeField] private List<GameObject> _levelPrefabs;
    private int _currentLevelIndex = 0;
    public GameObject CurrentLevelObject;
    
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
        DOTween.To(
            () => _levelLight.intensity,
            x => _levelLight.intensity = x,
            1f,
            2f
        ).SetEase(Ease.InOutElastic);
        if (_currentLevelIndex <= _levelPrefabs.Count - 1)
        {
            var levelName = _levelPrefabs[_currentLevelIndex].GetComponent<Level>().LevelName;
            _levelNameObject.GetComponent<LevelNameMovements>()
                .MoveObjectAndSetText(levelName, () =>
                {
                    Debug.Log("Loading level: " + _levelPrefabs[_currentLevelIndex].name);
                    CurrentLevelObject = Instantiate(_levelPrefabs[_currentLevelIndex], Vector2.zero, Quaternion.identity);
                    _currentLevelIndex++;
                    DOTween.To(
                        () => _levelLight.intensity,
                        x => _levelLight.intensity = x,
                        0f,
                        2f
                    ).SetEase(Ease.InOutElastic);
                });
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }
}
