using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PharaohMover))]
public class Pharaoh : MonoBehaviour
{
    [SerializeField] Cart _cart;

    private PharaohMover _pharaohMover;

    public event Action PharaohIsComing;

    private void OnEnable()
    {
        _cart.StoneAreOver += StartMove;
    }

    private void OnDisable()
    {
        _cart.StoneAreOver -= StartMove;
    }

    private void Start()
    {
        _pharaohMover = GetComponent<PharaohMover>();
    }

    private void StartMove()
    {
        PharaohIsComing?.Invoke();
        StartCoroutine(_pharaohMover.MoveToCenter());
    }
}
