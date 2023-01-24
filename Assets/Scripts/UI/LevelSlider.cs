using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    private const float MaximumSliderValue = 1;

    [SerializeField] private Building _building;
    [SerializeField] private float _duration;
    [SerializeField] private string _keyNameLevelProgress;
    [SerializeField] private string _keyNameCompletedLevel;

    private Slider _slider;
    private bool _isCompleted = false;

    public float Value => _slider.value;

    private void OnEnable()
    {
        _building.BuildingPartBuilt += ChangeValue;
    }

    private void OnDisable()
    {
        _building.BuildingPartBuilt -= ChangeValue;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = ES3.Load(_keyNameLevelProgress, SaveProgress.FilePath.LevelProgress, _slider.value);
    }

    private void Start()
    {
        _isCompleted = ES3.Load(_keyNameCompletedLevel, SaveProgress.FilePath.LevelProgress, _isCompleted);
    }

    private void ChangeValue(BuildingPart buildingPart)
    {
        float percentege = MaximumSliderValue / _building.AllBuildingParts;
        float targetValue = percentege + _slider.value;

        _slider.value = targetValue;
        ES3.Save(_keyNameLevelProgress, targetValue, SaveProgress.FilePath.LevelProgress);
        float error = 0.98f;

        if (_slider.value >= error)
        {
            _isCompleted = true;
            ES3.Save(_keyNameCompletedLevel, _isCompleted, SaveProgress.FilePath.LevelProgress);
        }
    }
}
