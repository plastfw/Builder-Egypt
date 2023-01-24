using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Figure : MonoBehaviour
{
    const float MaximumProgressValue = 0.99f;

    [SerializeField] private string _levelKey;
    [SerializeField] private string _nameKeyForInclusionOfFigure;
    [SerializeField] private float _duration;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnityEngine.GameObject _figureTemplate;
    [SerializeField] private RoadPart _roadPart;
    [SerializeField] private Vector3 _scaleFigureTemplate;

    private Tween _animationDisappearance;
    private float _progress = 0;
    private float _targetScaleZ = 0;
    private bool _isWork = true;

    private IEnumerator Start()
    {
        _isWork = ES3.Load(_nameKeyForInclusionOfFigure, SaveProgress.FilePath.LevelProgress, _isWork);
        _progress = ES3.Load(_levelKey, SaveProgress.FilePath.LevelProgress, _progress);

        if (_isWork == true)
        {
            if (_progress >= MaximumProgressValue)
            {
                _animationDisappearance = transform.DOScaleZ(_targetScaleZ, _duration);
                yield return _animationDisappearance.WaitForCompletion();

                UnityEngine.GameObject figure = Instantiate(_figureTemplate, _spawnPoint.position, _figureTemplate.transform.rotation);
                figure.transform.DOScale(_scaleFigureTemplate, _duration);

                _isWork = false;
                ES3.Save(_nameKeyForInclusionOfFigure, _isWork, SaveProgress.FilePath.LevelProgress);
            }
        }
        else
            SpawnSavedFigure();
    }

    private void SpawnSavedFigure()
    {
        float duration = 0.01f;
        UnityEngine.GameObject figure = Instantiate(_figureTemplate, _spawnPoint.position, _figureTemplate.transform.rotation);
        figure.transform.DOScale(_scaleFigureTemplate, duration);
        transform.localScale = Vector3.zero;
    }
}

