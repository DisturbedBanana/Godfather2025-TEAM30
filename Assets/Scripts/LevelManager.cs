using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int _levelDimTimer;
    [SerializeField] private SunAndMoon _sunAndMoon;
    
    private int _currentLevelIndex;
    
    private GameObject _currentLevelObject;
    public GameObject CurrentLevelObject => _currentLevelObject;
    
    private int _currentPar;
    public int CurrentPar => _currentPar;
    
    private string _currentLevelName;
    public string CurrentLevelName => _currentLevelName;
    
    
    private void Awake()
    {  
        _instance ??= this;
    }
    
    public void LoadNextLevel(bool isARetry = false)
    {
        // For retry, we need to go back one level since we already incremented
        if (isARetry && _currentLevelIndex > 0)
        {
            _currentLevelIndex--;
        }

        DOTween.To(
            () => _levelLight.intensity,
            x => _levelLight.intensity = x,
            1f,
            2f
        ).SetEase(Ease.InOutElastic);

        if (_currentLevelIndex >= 0 && _currentLevelIndex < _levelPrefabs.Count)
        {
            int levelIndexToLoad = _currentLevelIndex;

            _currentLevelName = _levelPrefabs[levelIndexToLoad].GetComponent<Level>().LevelName;
            _levelNameObject.GetComponent<LevelNameMovements>()
                .MoveObjectAndSetText(_currentLevelName, () =>
                {
                    Debug.Log("Loading level: " + _levelPrefabs[levelIndexToLoad].name);
                    _currentLevelObject = Instantiate(_levelPrefabs[levelIndexToLoad], Vector2.zero, Quaternion.identity);
                    _currentPar = _currentLevelObject.GetComponent<Level>().Par;
                    StartCoroutine(DimTimer());
                });

            // Only increment level index if not retrying
            if (!isARetry) _currentLevelIndex++;
        }
        else
        {
            Debug.LogWarning("No more levels to load or index is out of range.");
        }
    }

    public void DestroyLevel()
    {
        Destroy(CurrentLevelObject);
        _sunAndMoon.Rotate();
    }

    private IEnumerator DimTimer()
    {
        yield return new WaitForSeconds(_levelDimTimer);

        DOTween.To(
            () => _levelLight.intensity,
            x => _levelLight.intensity = x,
            0f,
            2f
        ).SetEase(Ease.InOutElastic);
        
        _sunAndMoon.Rotate();
        
        Ball ball = FindFirstObjectByType<Ball>();
        if (ball != null)
        {
            ball.CanBeLaunched = true;
        }
    }
}
