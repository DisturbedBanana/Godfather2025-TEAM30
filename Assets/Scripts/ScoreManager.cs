using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }
    
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _parText;

    private int _currentLevelScore;
    private int _totalScore;
    
    public int CurrentLevelScore => _currentLevelScore;
    public int TotalScore { get => _totalScore; set => _totalScore = value; }
    
    private void Awake()
    {  
        _instance ??= this;
    }

    private void Start()
    {
        _scoreText.text = "Score : " + _currentLevelScore.ToString();
    }

    public void AddLevelScore() // Call when player makes a move
    {
        _currentLevelScore++;
        _scoreText.text = "Score : " + _currentLevelScore.ToString();
    }

    public void ResetScoreTexts(int par) // Call when loading a new level
    {
        _currentLevelScore = 0;
        _scoreText.text = "Score : " + _currentLevelScore.ToString();
        _parText.text = "Par: " + par;
    }

    public void CalculateScores() // Call when level is completed
    {
        // Only add to total score if player met or beat par
        if (_currentLevelScore < LevelManager.Instance.CurrentPar) return;
        
        _totalScore += _currentLevelScore - LevelManager.Instance.CurrentLevelObject.GetComponent<Level>().Par;
    }
}
