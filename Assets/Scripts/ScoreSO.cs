using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Score")]
public class ScoreSO : ScriptableObject
{
    [SerializeField] private int _value;

    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            if (_value == value)
            {
                return;
            }

            _value = value;
            ScoreChanged?.Invoke(value);
        }
    }

    public Action<int> ScoreChanged;
}