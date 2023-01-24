using UnityEngine;
using RayFire;

[RequireComponent(typeof(RayfireDebris))]
[RequireComponent(typeof(RayfireDust))]
public class DebrisEffect : MonoBehaviour
{
    private RayfireDebris _rayfireDebris;
    private RayfireDust _rayfireDust;
    private Cart _cart;

    private void Awake()
    {
        _rayfireDebris = GetComponent<RayfireDebris>();
        _rayfireDust = GetComponent<RayfireDust>();
        _cart = GetComponentInParent<Cart>();
    }

    private void OnEnable()
    {
        _cart.StoneIsTaken += Show;
    }

    private void OnDisable()
    {
        _cart.StoneIsTaken -= Show;
    }

    private void Show()
    {
        _rayfireDebris.Emit();
        _rayfireDust.Emit();
    }
}
