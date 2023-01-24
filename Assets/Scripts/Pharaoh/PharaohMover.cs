using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PharaohMover : MonoBehaviour
{
    private const float ZeroAngle = 0;

    [SerializeField] private float _durationPath;
    [SerializeField] private Transform _targetPoint;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Tween _move;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public IEnumerator MoveToCenter()
    {
        _move = _rigidbody.DOMove(_targetPoint.transform.position, _durationPath).SetEase(Ease.Linear);

        yield return _move.WaitForCompletion();
        Turn();
        _animator.Play(PharaohAnimator.States.Like);
    }

    private void Turn()
    {
        float duration = 0.5f;
        float yAngle = -180;
        Quaternion targetRotation = Quaternion.Euler(ZeroAngle, -yAngle, ZeroAngle);
        transform.DORotateQuaternion(Quaternion.identity, duration);
    }
}
