using UnityEngine;
using UnityEngine.UI;

public class TargetScoreSetter : MonoBehaviour
{
    [SerializeField] private ScoreSO _targetScore;
    [SerializeField] private Slider _slider;


    void Awake(){
        _slider.value = _targetScore.Value;
    }

    public void SetTargetScore(float target)
    {
        _targetScore.Value = (int)target;
    }

    // Super wichtige sachen, die vielleicht nicht ganz richtig sind
}