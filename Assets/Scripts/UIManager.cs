using System;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _levelCanvas;
    [SerializeField] private GameObject _postLevelCanvas;
    [SerializeField] private GameObject _dialogCanvas;
    [SerializeField] private GameObject _pauseCanvas;
    
    [SerializeField] private TextMeshProUGUI _postLevelNameText;
    [SerializeField] private TextMeshProUGUI _postLevelScoreText;
    [SerializeField] private TextMeshProUGUI _postLevelTotalScoreText;
    [SerializeField] private TextMeshProUGUI _postLevelParText;

    private void Awake()
    {
        Instance ??= this;
    }

    public void DisplayDialog()
    {
        _menuCanvas.SetActive(false);
        _dialogCanvas.SetActive(true);
    }

    public void HideDialog()
    {
        _dialogCanvas.SetActive(false);
        _levelCanvas.SetActive(true);
        LevelManager.Instance.LoadNextLevel();
        ScoreManager.Instance.TotalScore = 0;
    }
    
    public void PlayNext(bool fromMenu)
    {
        if (fromMenu)
            _menuCanvas.SetActive(false);
        else
            _postLevelCanvas.SetActive(false);
        
        _levelCanvas.SetActive(true);
        LevelManager.Instance.LoadNextLevel();
    }

    public void BackToMainMenu()
    {
        _levelCanvas.SetActive(false);
        _postLevelCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);
        _menuCanvas.SetActive(true);
        ScoreManager.Instance.TotalScore = 0;
        LevelManager.Instance.DestroyLevel();
        LevelManager.Instance.CurrentLevelIndex = 0;
    }

    public void Retry()
    {
        _postLevelCanvas.SetActive(false);
        _levelCanvas.SetActive(true);
        LevelManager.Instance.DestroyLevel();
        LevelManager.Instance.LoadNextLevel(true);
    }


    public void LevelFinished() 
    {
        ScoreManager.Instance.CalculateScores();
        _levelCanvas.SetActive(false);
        _postLevelCanvas.SetActive(true);
        _postLevelNameText.text = LevelManager.Instance.CurrentLevelName;
        _postLevelParText.text = $"Par: {LevelManager.Instance.CurrentPar}";
        _postLevelScoreText.text = $"Level Score: {ScoreManager.Instance.CurrentLevelScore}";
        _postLevelTotalScoreText.text = $"Total Score: {ScoreManager.Instance.TotalScore}";
        LevelManager.Instance.DestroyLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseCanvas.SetActive(!_pauseCanvas.activeInHierarchy);
        }
    }
}
