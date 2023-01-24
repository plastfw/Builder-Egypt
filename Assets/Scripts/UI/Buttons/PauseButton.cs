using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private UnityEngine.GameObject _menuPanel;

   public void OnButtonClick()
    {
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
