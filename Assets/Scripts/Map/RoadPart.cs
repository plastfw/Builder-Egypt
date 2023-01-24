using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadPart : MonoBehaviour
{
    const int MaximumValueScale = 1;

    [SerializeField] private List<UnityEngine.GameObject> _roadParts = new List<UnityEngine.GameObject>();
    [SerializeField] private string _nameOfKeyForShowingRoad;
    [SerializeField] private MapLevelDisplay _levelDisplay;
    [SerializeField] private Figure _figure;
    [SerializeField] private float _delay;
    [SerializeField] private float _height;

    private bool _isShowned = false;

    private void Awake()
    {
        _isShowned = ES3.Load(_nameOfKeyForShowingRoad, SaveProgress.FilePath.Map, _isShowned);

        if (_isShowned == true)
            ShowAtStart();
    }

    public IEnumerator Show()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);
        _isShowned = true;
        ES3.Save(_nameOfKeyForShowingRoad, _isShowned, SaveProgress.FilePath.Map);

        for (int i = 0; i < _roadParts.Count; i++)
        {
            Vector3 targetPoisition = new Vector3(_roadParts[i].transform.position.x, _roadParts[i].transform.position.y + _height, _roadParts[i].transform.position.z);
            _roadParts[i].transform.position = targetPoisition;
            _roadParts[i].gameObject.SetActive(true);
            _roadParts[i].gameObject.transform.DOMoveY(_roadParts[i].transform.position.y - _height, _delay);
            yield return waitForSeconds;
        }

        ShowBruch();
    }

    private void ShowAtStart()
    {
        for (int i = 0; i < _roadParts.Count; i++)
        {
            _roadParts[i].gameObject.SetActive(true);
        }

        _figure.gameObject.SetActive(true);
        _levelDisplay.SetMaximumAlpha();
    }

    private void ShowBruch()
    {
        float duration = 1f;

        _figure.gameObject.SetActive(true);
        _levelDisplay.Show();

        _figure.transform.DOScaleZ(MaximumValueScale, duration);
    }
}
