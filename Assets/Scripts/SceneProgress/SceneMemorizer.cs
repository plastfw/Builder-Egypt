using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMemorizer : MonoBehaviour
{
    [SerializeField] private AnalyticManager _analyticManager;
    [SerializeField] private Data _data;
    [SerializeField] private Cart _cart;

    private int _lastScene = 0;
    private float _coin = 0;

    private void OnEnable()
    {
        if (_cart != null)
            _cart.StoneAreOver += AddCompleteLevel;
    }

    private void OnDisable()
    {
        if (_cart != null)
            _cart.StoneAreOver -= AddCompleteLevel;
    }

    private void Start()
    {
        _lastScene = SceneManager.GetActiveScene().buildIndex;
        _analyticManager.SendEventOnLevelStart(_lastScene);
        ES3.Save(SaveProgress.TitleKey.LastScene, _lastScene, SaveProgress.FilePath.SceneNumber);
    }

    private void AddCompleteLevel()
    {
        _analyticManager.SendEventOnLevelComplete(_lastScene);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            _data.Load();
            _coin = ES3.Load(SaveProgress.TitleKey.Coin, SaveProgress.FilePath.Coin, _coin);
            _analyticManager.SendEventOnGameExit(_data.GetRegistrationDate(), _data.GetSessionCount(), _data.GetNumberDaysAfterRegistration(), _coin);
            _data.Save();
        }
    }

    private void OnApplicationQuit()
    {
        _data.Load();
        _coin = ES3.Load(SaveProgress.TitleKey.Coin, SaveProgress.FilePath.Coin, _coin);

        _analyticManager.SendEventOnGameExit(_data.GetRegistrationDate(), _data.GetSessionCount(), _data.GetNumberDaysAfterRegistration(), _coin);
        _data.Save();
    }
}
