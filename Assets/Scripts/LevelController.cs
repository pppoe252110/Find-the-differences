using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float _time = 120;

    private float _timer;

    private bool _levelStarted;

    private DisappearHandler[] _disappearHandlers;

    private int _guessed;

    private void Start()
    {
        var handler = GetComponentInChildren<DisappearHandler>();
        handler.SetLevelController(this);
        handler.Initialize();

        _disappearHandlers = GetComponentsInChildren<DisappearHandler>();
        _disappearHandlers[1].SetLevelController(this);


        _disappearHandlers[1].InitializeItems();
    }

    public void OnGuess(int index)
    {
        for (int i = 0; i < _disappearHandlers.Length; i++)
        {
            _disappearHandlers[i].Guessed(index);
        }
        _guessed++;
        if(_guessed >= _disappearHandlers[0]._items.Length)
        {
            GameManager.ShowWinLoseSceen("Победа");
        }
    }

    public void StartLevel()
    {
        _timer = _time;
        GameManager.SetTimeLeft(_timer);
        _levelStarted = true;
    }

    internal void AddTime(float time)
    {
        _timer += time;
    }

    private void Update()
    {
        if (!_levelStarted)
            return;
        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            _levelStarted = false;
            GameManager.ShowWinLoseSceen("Вы проиграли");
        }
        GameManager.SetTimeLeft(_timer);
    }
}
