using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Color[] _playerColours;
    [SerializeField] private Transform[] _playerPositions;
    [SerializeField] private Rigidbody2D _ball;
    [SerializeField] private Transform _ballPosition;
    [SerializeField] private ScoreSO[] _scoreSos;

    private PlayerInputManager _inputManager;

    private List<PlayerInput> _playerInputs = new();

    void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        foreach (var score in _scoreSos)
        {
            score.Value = 0;
        }
    }

    void OnEnable()
    {
        foreach (ScoreSO score in _scoreSos)
        {
            score.ScoreChanged += OnGoal;
        }
    }
    
    void OnDisable()
    {
        foreach(ScoreSO score in _scoreSos)
        {
            score.ScoreChanged -= OnGoal;
        }
    }

    private void OnGoal(int score)
    {
        // Reset ball
        _ball.linearVelocity = Vector2.zero;
        _ball.transform.position = _ballPosition.position;

        // Reset player
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            _playerInputs[i].transform.position = _playerPositions[i].position;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (_playerInputs.Contains(playerInput)) return;

        playerInput.GetComponent<SpriteRenderer>().color = _playerColours[_playerInputs.Count];
        playerInput.transform.position = _playerPositions[_playerInputs.Count].position;

        _playerInputs.Add(playerInput);

        if(_playerInputs.Count >= _inputManager.maxPlayerCount)
        {
            _inputManager.DisableJoining();
        }
    }


}