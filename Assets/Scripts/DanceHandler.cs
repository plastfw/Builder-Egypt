using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DanceHandler : MonoBehaviour
{
    private int ZeroAngle = 0;

    [SerializeField] private List<Slave> _spawnedSlaves = new List<Slave>();
    [SerializeField] private Transform[] _targetPoints;
    [SerializeField] private SlaveSeller _slaveSeller;
    [SerializeField] private Cart _cart;

    private Tween _turn;

    private void OnEnable()
    {
        _slaveSeller.SlaveIsSold += AddSlave;
        _cart.StoneAreOver += ShowDance;
    }

    private void OnDisable()
    {
        _slaveSeller.SlaveIsSold -= AddSlave;
        _cart.StoneAreOver -= ShowDance;
    }

    private void AddSlave(Slave slave)
    {
        _spawnedSlaves.Add(slave);
    }

    private void ShowDance()
    {
        for (int i = _spawnedSlaves.Count - 1; i >= 0; i--)
        {
            _spawnedSlaves[i].SetStartSpeed(5f);
            _spawnedSlaves[i].MoveToPoint(_targetPoints[i].transform.position);
            StartCoroutine(ActivateDance(_spawnedSlaves[i]));
        }
    }

    private void StartDance(Slave slave)
    {
        slave.StartAimationDance();
    }

    private IEnumerator ActivateDance(Slave slave)
    {
        float rotateDuration = 0.3f;
        float yAngle = -178.11f;
        Quaternion targetRotation = Quaternion.Euler(ZeroAngle, yAngle, ZeroAngle);
        float delay = 0.6f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        yield return waitForSeconds;
        _turn = slave.transform.DORotateQuaternion(targetRotation, rotateDuration);
        yield return _turn.WaitForCompletion();
        StartDance(slave);
    }
}
