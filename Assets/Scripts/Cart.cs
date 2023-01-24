using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
public class Cart : ObjectPool
{
    const int FirstBuildingPart = 0;
    const int ZeroAngle = 0;

    [SerializeField] private AnalyticManager _analyticManager;
    [SerializeField] private List<Stone> _buildingPartsTemplates = new List<Stone>();
    [SerializeField] private List<BuildingPart> _buildingParts = new List<BuildingPart>();

    private List<BuildingPart> _disabledBuildingParts = new List<BuildingPart>();
    private Sequence _wiggleAnimation;

    public event Action StoneIsTaken;
    public event Action StoneAreOver;

    private void Start()
    {
        GetActivatedBuildingPart();
        Initialize(_buildingPartsTemplates);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent(out Slave slave) && slave.IsBusy == false && _disabledBuildingParts.Count != 0)
        {
            if (TryGetObject(out Stone stone))
            {
                StoneIsTaken?.Invoke();
                StartWiggleAnimation();
                slave.TakeStone(stone);
                slave.MoveToBuilding(_disabledBuildingParts[FirstBuildingPart]);
                StartCoroutine(slave.ThrowStone(_disabledBuildingParts[FirstBuildingPart]));
                _disabledBuildingParts.RemoveAt(FirstBuildingPart);
            }
        }
        else if (_disabledBuildingParts.Count == 0)
            StartCoroutine(ActivateEvent());
    }

    private void GetActivatedBuildingPart()
    {
        for (int i = 0; i < _buildingParts.Count; i++)
        {
            if (_buildingParts[i].gameObject.activeSelf == false)
                _disabledBuildingParts.Add(_buildingParts[i]);
        }

        _disabledBuildingParts.OrderBy(BuildingPart => BuildingPart.name).ToList();
    }

    private void StartWiggleAnimation()
    {
        _wiggleAnimation = DOTween.Sequence();
        float duration = 0.3f;
        float yAngle = 156.614f;
        float zAngle = -5f;
        Quaternion targetRotation1 = Quaternion.Euler(ZeroAngle, yAngle, zAngle);
        Quaternion targetRotation2 = Quaternion.Euler(ZeroAngle, yAngle, ZeroAngle);

        _wiggleAnimation
            .Append(transform.DORotateQuaternion(targetRotation1, duration))
            .Append(transform.DORotateQuaternion(targetRotation2, duration));
    }

    private IEnumerator ActivateEvent()
    {
        float delay = 1f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        yield return waitForSeconds;
        StoneAreOver?.Invoke();
        gameObject.SetActive(false);
    }
}