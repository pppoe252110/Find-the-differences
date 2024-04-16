using AppodealAds.Unity.Api;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private RectTransform _winlossPanel;
    [SerializeField] private TextMeshProUGUI _winlossText;

    private LevelSaver _level;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindAnyObjectByType<GameManager>();
            return _instance;
        }
    }
    private static GameManager _instance;

    private LevelController _currentLevel;
    private AsyncOperationHandle<GameObject> _levelHandle;

    public static void SetTimeLeft(float time) 
    {
        Instance._timerText.text = "Время:\n" + TimeSpan.FromSeconds(Mathf.Max(0, time)).ToString("m\\:ss");
    }

    private void OnEnable()
    {
        SaveManager.OnSave += OnSave;
        SaveManager.OnLoaded += OnLoaded;
        Addressables.LoadAssetAsync<GameObject>("Levels/Level1").Completed += LoadLevel;
    }

    private void OnDisable()
    {
        SaveManager.OnSave -= OnSave;
        SaveManager.OnLoaded -= OnLoaded;
        Addressables.Release(_levelHandle);
    }

    private void OnSave()
    {
        SaveManager.TryStoreIfNotExist(_level);
    }

    private void OnLoaded()
    {
        _level = SaveManager.GetSaveData<LevelSaver>();
        _levelText.text = "Уровень:\n" + _level.save.currentLevel;
    }

    public void PlayButtonPressed()
    {
        _level.save.currentLevel++;
        _levelText.text = "Уровень:\n" + _level.save.currentLevel;
        _winlossPanel.gameObject.SetActive(false);

        SaveManager.Save();

        if (_levelHandle.Status == AsyncOperationStatus.None)
        {
            Addressables.LoadAssetAsync<GameObject>("Levels/Level1").Completed += LoadLevel;
        }
        else
        {
            if (_currentLevel)
            {
                Destroy(_currentLevel.gameObject);
            }
            if (_levelHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _currentLevel = Instantiate(_levelHandle.Result).GetComponent<LevelController>();
                _currentLevel.StartLevel();
            }
        }
    }

    public void AddSeconds()
    {
        _currentLevel.AddTime(10f);
        Message.Instance.DoMessage("UNITY IAP");
    }

    private void LoadLevel(AsyncOperationHandle<GameObject> handle)
    {
        _levelHandle = handle;
    }

    public static void ShowWinLoseSceen(string infoText)
    {
        Instance._winlossText.text = infoText;
        Instance._winlossPanel.gameObject.SetActive(true);
        Message.Instance.DoMessage("INTERSTITIAL AD");
        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
            Appodeal.show(Appodeal.INTERSTITIAL);
    }
}
