using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class SelectHandler : MonoBehaviour
{
    const string CompletedLevel = "Completed_Level_";
    const string Level = "Level_";
    const string Seconds = "_Seconds";
    const string Mitunes = "_Minutes";
    const string Hour = "_Hour";


    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private float _rayDistance;
    [SerializeField] private AnalyticManager _analytic;
    [SerializeField] private Data _data;

    private Camera _camera;
    private string[] _sceneTitles = new string[6] { "Pyramid", "Sphinx", "Anubis", "Rameses", "Columns", "Temple", };
    private bool _isCompleted = false;
    private int _sceneOfNumber = 1;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OpenScene();
    }

    private void OpenScene()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance, _raycastMask))
        {
            for (int i = 0; i < _sceneTitles.Length; i++)
            {
                if (hit.collider.name == _sceneTitles[i])
                {
                    _analytic.SendEventOnLevelRestart(i);
                    TryDeletingData();
                    SceneManager.LoadScene(_sceneTitles[i]);
                }

                _sceneOfNumber++;
            }
        }
    }

    private void TryDeletingData()
    {
        _isCompleted = ES3.Load(CompletedLevel + _sceneOfNumber, SaveProgress.FilePath.LevelProgress, _isCompleted);

        if (_isCompleted == true)
        {
            ES3.DeleteKey(Level + _sceneOfNumber, SaveProgress.FilePath.LevelProgress);
            ES3.DeleteKey(Level + _sceneOfNumber, SaveProgress.FilePath.BuildingParts);
            ES3.DeleteKey(Level + _sceneOfNumber + Seconds, SaveProgress.FilePath.PassageTime);
            ES3.DeleteKey(Level + _sceneOfNumber + Mitunes, SaveProgress.FilePath.PassageTime);
            ES3.DeleteKey(Level + _sceneOfNumber + Hour, SaveProgress.FilePath.PassageTime);
        }
    }
}