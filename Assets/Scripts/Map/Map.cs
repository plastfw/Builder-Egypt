using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    const string Level = "Completed_Level_";
    const int NumberOfLevels = 6;

    [SerializeField] private List<RoadPart> _roadParts = new List<RoadPart>();
    [SerializeField] private Figure[] _figures;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isCompleted = false;
    private int _lastCompletedLevel = 1;

    private void Start()
    {
        _lastCompletedLevel = ES3.Load(SaveProgress.TitleKey.LastCompletedLevel, SaveProgress.FilePath.Map, _lastCompletedLevel);

        if (_lastCompletedLevel < NumberOfLevels)
        {
            DrawPath();
            StartCoroutine(EnableParticle());
        }
    }

    private void DrawPath()
    {
        _isCompleted = ES3.Load(Level + _lastCompletedLevel.ToString(), SaveProgress.FilePath.LevelProgress, _isCompleted);
        print(_isCompleted);

        if (_isCompleted == true)
        {
            _lastCompletedLevel++;
            StartCoroutine(EnableParticle());
            StartCoroutine(_roadParts[_lastCompletedLevel - 2].Show());
            ES3.Save(SaveProgress.TitleKey.LastCompletedLevel, _lastCompletedLevel, SaveProgress.FilePath.Map);
        }
    }

    private IEnumerator EnableParticle()
    {
        float delay = 3f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;

        _particleSystem.transform.position = _figures[_lastCompletedLevel - 2].transform.position;
        _particleSystem.gameObject.SetActive(true);
    }
}
