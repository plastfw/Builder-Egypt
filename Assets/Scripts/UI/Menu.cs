using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _goToMapButton;
    [SerializeField] private string _keyNameForCompletedLevel;

    private const string Map = nameof(Map);
    private bool _isCompleted = false;

    private void Start()
    {
        if (ES3.KeyExists(_keyNameForCompletedLevel, SaveProgress.FilePath.LevelProgress))
            _isCompleted = ES3.Load(_keyNameForCompletedLevel, SaveProgress.FilePath.LevelProgress, _isCompleted);

        if (_isCompleted == true)
            _goToMapButton.gameObject.SetActive(true);
    }

    public void GoToMap()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Map);
    }

    public void Hide()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
