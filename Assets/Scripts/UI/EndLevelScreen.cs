using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLevelScreen : MonoBehaviour
{
    const int ZeroAlpha = 0;
    const int MaximumAlpha = 1;
    const float MaximumSliderValue = 0.99f;
    const string Map = nameof(Map);

    [SerializeField] private Cart _cart;
    [SerializeField] private LevelSlider _levelSlider;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private UnityEngine.GameObject _allUI;
    [SerializeField] private CanvasGroup _canvasGroup1;
    [SerializeField] private CanvasGroup _canvasGroup2;
    [SerializeField] private ContinueButton _continueButton;

    private Image _image;
    private Tween _emergenceOfCavasGroup;

    private void OnEnable()
    {
        _cart.StoneAreOver += Show;
        _continueButton.ButtonPresed += HideScreen;
    }

    private void OnDisable()
    {
        _cart.StoneAreOver -= Show;
        _continueButton.ButtonPresed -= HideScreen;
    }

    private void Start()
    {
        _continueButton.gameObject.SetActive(false);
        _image = GetComponent<Image>();

        if (_levelSlider.Value >= MaximumSliderValue)
            SceneManager.LoadScene(Map);
    }

    private void Show()
    {
        _continueButton.gameObject.SetActive(true);
        StartCoroutine(StartShow());
    }

    private void HideScreen()
    {
        float duration = 1f;
        _image.DOFade(MaximumAlpha, duration);
        _canvasGroup2.DOFade(ZeroAlpha, duration);
    }

    private void HideButton()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = false;
        }
    }

    private IEnumerator StartShow()
    {
        float duration1 = 1f;
        float duration2 = 0.5f;
        float delay = 4.7f;
        HideButton();
        _emergenceOfCavasGroup = _canvasGroup1.DOFade(ZeroAlpha, duration1);

        yield return _emergenceOfCavasGroup.WaitForCompletion();
        _allUI.SetActive(false);
        _canvasGroup2.DOFade(MaximumAlpha, duration2).SetDelay(delay);
    }
}
