using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private string _scoringObjectTag = "Ball";
    [SerializeField] private ScoreSO _score;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(_scoringObjectTag)) return;
        _score.Value++;
    }
}