using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class MapLevelDisplay : MonoBehaviour
{
    const float MaximumSliderValue = 0.98f;
    const int MaximumValueAlpha = 1;
    const int MaximimumStarsCount = 3;
    const int AverageStarsCount = 2;
    const int MinimumStartsCount = 1;
    const int MaximumMinutesCount = 15;
    const int AverageMinutesCount = 8;
    const int MinimumMinutesCount = 5;

    [SerializeField] private float _duration;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private UnityEngine.GameObject[] _activeStars;
    [SerializeField] private string _keyNameForLevel;
    [SerializeField] private string _keyForPassageTime;

    private int _numberOfActivateStars = 0;
    private int _minutes = 0;
    private float _sliderValue;

    private void Start()
    {
        _minutes = ES3.Load(_keyForPassageTime, SaveProgress.FilePath.PassageTime, _minutes);
        _sliderValue = ES3.Load(_keyNameForLevel, SaveProgress.FilePath.LevelProgress, _sliderValue);

        if (_sliderValue >= MaximumSliderValue)
            EnableStar();
    }

    public void Show()
    {
        _canvasGroup.DOFade(MaximumValueAlpha, _duration);
    }

    public void SetMaximumAlpha()
    {
        _canvasGroup.alpha = 1;
    }

    private void EnableStar()
    {
        SetNumberOfStars();

        for (int i = 0; i < _numberOfActivateStars; i++)
        {
            _activeStars[i].SetActive(true);
        }
    }

    private void SetNumberOfStars()
    {
        if (_minutes >= MaximumMinutesCount)
            _numberOfActivateStars = MinimumStartsCount;
        else if (_minutes <= AverageMinutesCount && _minutes >= MinimumMinutesCount)
            _numberOfActivateStars = AverageStarsCount;
        else
            _numberOfActivateStars = MaximimumStarsCount;
    }
}
