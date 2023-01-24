using System;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private List<BuildingPart> _allBuildingParts = new List<BuildingPart>();
    [SerializeField] private string _nameOfTheKeyForSavingParts;

    private List<BuildingPart> _enabledBuildingParts = new List<BuildingPart>();

    public int EnabledBuildingParts => _enabledBuildingParts.Count;
    public int AllBuildingParts => _allBuildingParts.Count;

    public event Action<BuildingPart> BuildingPartBuilt;

    private void Awake()
    {
        Onload();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _allBuildingParts.Count; i++)
        {
            _allBuildingParts[i].BuildingPartIsEnabled += AddBuildingPart;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _allBuildingParts.Count; i++)
        {
            _allBuildingParts[i].BuildingPartIsEnabled -= AddBuildingPart;
        }
    }

    private void AddBuildingPart(BuildingPart buildingPart)
    {
        _enabledBuildingParts.Add(buildingPart);
        BuildingPartBuilt?.Invoke(buildingPart);
        ES3.Save(_nameOfTheKeyForSavingParts, _enabledBuildingParts, SaveProgress.FilePath.BuildingParts);
    }

    private void Onload()
    {
        _enabledBuildingParts = ES3.Load(_nameOfTheKeyForSavingParts, SaveProgress.FilePath.BuildingParts, _enabledBuildingParts);

        for (int i = 0; i < _enabledBuildingParts.Count; i++)
        {
            _enabledBuildingParts[i].gameObject.SetActive(true);
        }
    }
}
