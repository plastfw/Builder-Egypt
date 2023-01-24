using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Slave))]
public class SlaveMover : MonoBehaviour
{
    private const int MaximumStamina = 100;
    private const int MinimumStamina = 0;

    [SerializeField] private bool _isStartMoveToCart;

    private Slave _slave;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Coroutine _decreaseSpeed;
    private Coroutine _decreaseStamina;
    private Coroutine _rest;
    private Vector3 _cartPoisition = new Vector3(2.2224f, -10.35f, -10.65f);
    private float _currentStamina = MaximumStamina;
    private float _deductibleErndurance = 1f;
    private bool _isStay = false;
    private bool _isWork = true;
    private bool _isRest = false;
    private int _speedButtonLevel = 0;
    private float _speedButton = 0.005f;
    private float _baseSpeed = 1.5f;

    public bool IsRest => _isRest;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _slave = GetComponent<Slave>();
        _animator = GetComponentInChildren<Animator>();
        _decreaseStamina = StartCoroutine(DecreaseStamina());

        _speedButtonLevel = ES3.Load(SaveProgress.TitleKey.LevelNumberSpeedBooster, SaveProgress.FilePath.Buttons, _speedButtonLevel);
        float currentSpeed = _baseSpeed + (_speedButton * (_speedButtonLevel - 1));
        _navMeshAgent.speed = currentSpeed;

        if (_isStartMoveToCart == true)
            MoveToCart();
    }

    public void DeactivateDeacreaseStamina()
    {
        StopCoroutine(_decreaseStamina);
    }

    public void IncreaseStamina(int stamina)
    {
        if (_decreaseSpeed != null)
            StopCoroutine(_decreaseSpeed);

        if (_rest != null)
            StopCoroutine(_rest);

        _isStay = false;
        _currentStamina += stamina;
    }

    public void IncreaseSpeed(float movementSpeed)
    {
        _navMeshAgent.speed += movementSpeed;
    }

    public void SetStartSpeed(float movementSpeed)
    {
        _navMeshAgent.speed = movementSpeed;
    }

    public void StopRest()
    {
        _isRest = false;
    }

    public void MoveToPoint(Vector3 targetPoint)
    {
        _navMeshAgent.SetDestination(targetPoint);
    }

    public void MoveToCart()
    {
        _navMeshAgent.SetDestination(_cartPoisition);
    }

    public bool CheckDistanceToPoint()
    {
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            return true;

        return false;
    }

    private IEnumerator DecreaseStamina()
    {
        float delay = 0.1f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while (true)
        {
            _currentStamina -= _deductibleErndurance;
            _currentStamina = Mathf.Clamp(_currentStamina, MinimumStamina, MaximumStamina);

            if (_currentStamina == 0)
            {
                if (_isStay == false)
                {
                    _isStay = true;
                    _decreaseSpeed = StartCoroutine(DecreaseSpeed());
                    _rest = StartCoroutine(Rest());
                }
            }

            yield return waitForSeconds;
        }
    }

    private IEnumerator Rest()
    {
        _isRest = true;

        float delay = 5f;
        float minimumDelay = 1.5f;
        int minimumValue = 25;
        int maximumValue = 80;
        int randomStamina = Random.Range(minimumValue, maximumValue);
        float randomDelay = Random.Range(minimumDelay, delay);
        WaitForSeconds waitForSeconds = new WaitForSeconds(randomDelay);
        yield return waitForSeconds;

        _speedButtonLevel = ES3.Load(SaveProgress.TitleKey.LevelNumberSpeedBooster, SaveProgress.FilePath.Buttons, _speedButtonLevel);
        float currentSpeed = _baseSpeed + (_speedButton * (_speedButtonLevel - 1));
        _navMeshAgent.speed = currentSpeed;
        _isWork = true;
        IncreaseStamina(randomStamina);

        if (_slave.IsBusy == true)
            _animator.Play(SlaveAnimator.States.TakeStone);
        else
            _animator.Play(SlaveAnimator.States.Run);

        _isRest = false;
    }

    private IEnumerator DecreaseSpeed()
    {
        float deceleraingSpeed = 0;
        float acceleration = 1.5f;
        deceleraingSpeed = acceleration * Time.deltaTime;
        float minimumSpeed = 0f;

        while (true)
        {
            _navMeshAgent.speed -= deceleraingSpeed;

            if (_navMeshAgent.speed == minimumSpeed)
            {
                _animator.Play(SlaveAnimator.States.Tired);

                if (_isWork == true)
                {
                    _isWork = false;
                    _slave.StartQueasyParticle();
                }
            }

            yield return null;
        }
    }
}
