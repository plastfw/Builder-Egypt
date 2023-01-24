using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(SlaveMover))]
public class Slave : MonoBehaviour
{
    const string AnimationSpeed = nameof(AnimationSpeed);

    [SerializeField] private Transform _breast;
    [SerializeField] private ParticleSystem _emojiParticle;
    [SerializeField] private ParticleSystem _sparksParticle;
    [SerializeField] private ParticleSystem _queasyParticle;

    private SlaveMover _slaveMover;
    private Stone _stoneInHand;
    private Tween _rotateAnimation;
    private Tween _throwAnimation;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isStronglyAcceleration = false;
    private Vector3 _targetScale = new Vector3(0.8f, 1.12f, 0.76f);
    private float _currentAnimationSpeed;
    private bool _isBusy = false;
    private int _speedButtonLevel = 0;
    private float _speedButton = 0.005f;
    private float _baseSpeed = 1.5f;

    public bool IsBusy => _isBusy;
    public bool IsStronglyAcceleration => _isStronglyAcceleration;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _slaveMover = GetComponent<SlaveMover>();
        _animator = GetComponentInChildren<Animator>();
        _currentAnimationSpeed = _animator.GetFloat(AnimationSpeed);
    }

    public void DeactivateDeacreaseStamina()
    {
        _slaveMover.DeactivateDeacreaseStamina();
    }

    public void StartAimationDance()
    {
        _animator.Play(SlaveAnimator.States.Dance);
    }

    public void StartQueasyParticle()
    {
        _queasyParticle.Play();
    }

    public void StartSparksParticle()
    {
        _sparksParticle.Play();
    }

    public void StartAnimationHitted()
    {
        _emojiParticle.Play();
        _animator.Play(SlaveAnimator.States.Hitted);
    }

    public void MoveToPoint(Vector3 targetPoint)
    {
        _slaveMover.MoveToPoint(targetPoint);
    }

    public void MoveToBuilding(BuildingPart buildingPart)
    {
        MakeBusy();
        float groundPosition = -10.86f;
        Vector3 newPosition = new Vector3(buildingPart.transform.position.x, groundPosition, buildingPart.transform.position.z);
        _slaveMover.MoveToPoint(newPosition);
    }

    public void IncreaseStamina(int stamina)
    {
        float currentSpeed = GetCurrentSpeed();

        if (_slaveMover.IsRest == true)
        {
            _navMeshAgent.speed = currentSpeed;
            _slaveMover.StopRest();
        }

        _slaveMover.IncreaseStamina(stamina);
        ChangeAnimationState();
    }

    public void IncreaseSpeed(float movementSpeed)
    {
        _slaveMover.IncreaseSpeed(movementSpeed);
    }

    public void IncreaseTempSpeed(float movementSpeed)
    {
        float maximumTempSpeed = 4;

        if (_navMeshAgent.speed < maximumTempSpeed)
            _slaveMover.IncreaseSpeed(movementSpeed);
    }

    public void SetStartSpeed(float movementSpeed)
    {
        _slaveMover.SetStartSpeed(movementSpeed);
    }

    public void TakeStone(Stone stoneInHand)
    {
        _stoneInHand = stoneInHand;
        SetStonePosition(stoneInHand);
        stoneInHand.gameObject.SetActive(true);
    }

    private void SetStonePosition(Stone stone)
    {
        int yAngle = 90;
        stone.transform.parent = _breast.transform;
        stone.transform.position = _breast.transform.position;
        stone.transform.localRotation = Quaternion.Euler(yAngle, stone.transform.localRotation.x, stone.transform.localRotation.z);
        stone.transform.localScale = _targetScale;
    }

    private void MakeBusy()
    {
        _animator.Play(SlaveAnimator.States.TakeStone);
        _isBusy = true;
    }

    private void MakeFree()
    {
        _stoneInHand.gameObject.SetActive(false);
        _isBusy = false;
        _slaveMover.MoveToCart();
    }
    private void ChangeAnimationState()
    {
        if (_isBusy == true)
            _animator.Play(SlaveAnimator.States.TakeStone);
        else
            _animator.Play(SlaveAnimator.States.Run);
    }

    private float GetCurrentSpeed()
    {
        _speedButtonLevel = ES3.Load(SaveProgress.TitleKey.LevelNumberSpeedBooster, SaveProgress.FilePath.Buttons, _speedButtonLevel);
        float currentSpeed = _baseSpeed + (_speedButton * (_speedButtonLevel - 1));
        return currentSpeed;
    }

    public IEnumerator DeacreaseSpeed()
    {
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);

        float targetSpeed = GetCurrentSpeed();
        float acceleration = 1f;
        float deceleraingSpeed = acceleration * Time.deltaTime;

        while (_navMeshAgent.speed >= targetSpeed)
        {
            _navMeshAgent.speed -= deceleraingSpeed;
            yield return null;
        }
    }

    public IEnumerator IncreaseTemporarilySpeed(float additionalSpeed, float delay)
    {
        _isStronglyAcceleration = true;
        ChangeAnimationState();
        float tempMovementSpeed = 0;
        tempMovementSpeed += additionalSpeed;
        _navMeshAgent.speed += tempMovementSpeed;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;

        DeacreaseSpeed();
        _isStronglyAcceleration = false;
    }

    public IEnumerator IncreaseTemporarilyAnimationSpeed(float additionalAnimationSpeed)
    {
        float delay = 1f;
        _currentAnimationSpeed += additionalAnimationSpeed;
        _animator.SetFloat(AnimationSpeed, _currentAnimationSpeed);

        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        _currentAnimationSpeed -= additionalAnimationSpeed;
        _animator.SetFloat(AnimationSpeed, _currentAnimationSpeed);
    }

    public IEnumerator ThrowStone(BuildingPart buildingPart)
    {
        while (_slaveMover.CheckDistanceToPoint() != true)
        {
            yield return null;
        }

        float duration = 0.2f;
        float jumpForce = 0.5f;
        int jumpNumbers = 1;
        float turningDuration = 0.3f;

        _rotateAnimation = transform.DOLookAt(buildingPart.transform.position, turningDuration, AxisConstraint.Y);
        yield return _rotateAnimation.WaitForCompletion();
        _stoneInHand.transform.parent = null;
        _throwAnimation = _stoneInHand.transform.DOJump(buildingPart.transform.position, jumpForce, jumpNumbers, duration);
        yield return _throwAnimation.WaitForCompletion();
        buildingPart.gameObject.SetActive(true);
        buildingPart.AddToBuilding();
        MakeFree();
        _animator.Play(SlaveAnimator.States.Run);
    }
}
