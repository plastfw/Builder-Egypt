using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private Camera _camera;
    [SerializeField] private Cart _cart;
    [SerializeField] private SlaveSeller _slaveSeller;
    [SerializeField] private List<Slave> _slaves = new List<Slave>();
    [SerializeField] private ParticleSystem _clickParticle;

    private float _additionalSpeedAtTargetClick = 0.7f;
    private float _additionalSpeedAtEmpty = 0.05f;
    private float _additionalAnimationSpeed = 0.1f;
    private int _rayDistance = 100;
    private Collider[] _colliders;
    private float _radius = 0.5f;
    private int _stamina1 = 99;
    private int _stamina2 = 20;
    private Coroutine _increaseSpeed;
    private bool _isClick = false;
    float _timeBetweenDeceleration = 1f;
    float _elapsedTime;
    private Coroutine _click;
    private Coroutine _decreaseSpeed;

    private void OnEnable()
    {
        _slaveSeller.SlaveIsSold += AddSlave;
        _cart.StoneAreOver += DeactivateClicks;
    }

    private void OnDisable()
    {
        _slaveSeller.SlaveIsSold -= AddSlave;
        _cart.StoneAreOver -= DeactivateClicks;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Hit();

            if (_click != null)
                StopCoroutine(_click);

            _click = StartCoroutine(Click());
        }
        else
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _timeBetweenDeceleration)
            {
                if (_isClick == false)
                {
                    for (int i = 0; i < _slaves.Count; i++)
                    {
                        if (_slaves[i].IsStronglyAcceleration != true)
                        _decreaseSpeed = StartCoroutine(_slaves[i].DeacreaseSpeed());
                    }
                }

                _elapsedTime = 0;
            }
        }
    }

    private void Hit()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance, _raycastMask))
        {
            _clickParticle.transform.position = hit.point;
            _clickParticle.Play();
            float duration = 2.5f;
            int minimumValue = 60;
            _colliders = Physics.OverlapSphere(hit.point, _radius);

            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i].TryGetComponent(out Slave slave))
                {
                    slave.StartAnimationHitted();
                    slave.StartSparksParticle();
                    slave.IncreaseTemporarilyAnimationSpeed(_additionalAnimationSpeed);
                    StartCoroutine(slave.IncreaseTemporarilySpeed(_additionalSpeedAtTargetClick, duration));
                    int randomStamina = Random.Range(minimumValue, _stamina1);
                    slave.IncreaseStamina(randomStamina);
                }
                else
                    IncreaseSpeedAllSlaves();
            }
        }
    }

    private void DeactivateClicks()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _slaves.Count; i++)
        {
            _slaves[i].DeactivateDeacreaseStamina();
        }
    }

    private void AddSlave(Slave slave)
    {
        _slaves.Add(slave);
    }

    private void IncreaseSpeedAllSlaves()
    {
        int minimumValue = 5;

        for (int i = 0; i < _slaves.Count; i++)
        {
            _increaseSpeed = StartCoroutine(_slaves[i].IncreaseTemporarilyAnimationSpeed(_additionalAnimationSpeed));
            if (_slaves[i].IsStronglyAcceleration == false)
                _slaves[i].IncreaseTempSpeed(_additionalSpeedAtEmpty);
            int randomStamina = Random.Range(minimumValue, _stamina2);
            _slaves[i].IncreaseStamina(randomStamina);
        }
    }

    private IEnumerator Click()
    {
        float delay = 0.3f;
        _isClick = true;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        _isClick = false;
    }
}
