using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _sceneNumber;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene(_sceneNumber);
    }
}
