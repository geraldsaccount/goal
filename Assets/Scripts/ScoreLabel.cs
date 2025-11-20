using System;
using TMPro;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    [SerializeField] private ScoreSO _score;
    private TextMeshProUGUI _label;

    void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
        UpdateLabel(0);
    }

    void OnEnable()
    {
        _score.ScoreChanged += UpdateLabel;
    }

    void OnDisable()
    {
        _score.ScoreChanged -= UpdateLabel;
    }

    private void UpdateLabel(int newScore)
    {
        _label.text = newScore.ToString();
    }
}