using UnityEngine;

public abstract class Seller : MonoBehaviour
{
    [SerializeField] protected float PriceForOnePiece;
    [SerializeField] protected float Surchange;

    protected int Percent = 100;

    public float Price => PriceForOnePiece;

    public abstract void RaisePrice();
}
