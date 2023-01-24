using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    const int PyramidScene = 1;

    [SerializeField] private AnalyticManager _analytic;
    [SerializeField] private Data _data;
    [SerializeField] private bool _isRemoveDataOnStart;

    private bool _isFirstLoad = true;
    private int _lastScene = 0;

    private void Awake()
    {
        _lastScene = ES3.Load(SaveProgress.TitleKey.LastScene, SaveProgress.FilePath.SceneNumber, _lastScene);
        _isFirstLoad = ES3.Load(SaveProgress.TitleKey.IsFirstLoad, SaveProgress.FilePath.SceneNumber, _isFirstLoad);

        if (_isRemoveDataOnStart)
            _data.RemoveData();

        CheckSaveFile();
        _data.AddSession();
        _data.SetLastLoginDate(DateTime.Now);
        _analytic.SendEventOnGameInitialize(_data.GetSessionCount());
        _data.Save();

        LoadFirstLevel();
    }

    private void LoadFirstLevel()
    {
        if (_isFirstLoad == true)
        {
            _isFirstLoad = false;
            ES3.Save(SaveProgress.TitleKey.IsFirstLoad, _isFirstLoad, SaveProgress.FilePath.SceneNumber);
            SceneManager.LoadScene(PyramidScene);
        }
        else
        {
            SceneManager.LoadScene(_lastScene);
        }
    }

    private void CheckSaveFile()
    {
        if (PlayerPrefs.HasKey(_data.GetKeyName()))
            _data.Load();
        else
            _data.SetDateRegistration(DateTime.Now);
    }
}
