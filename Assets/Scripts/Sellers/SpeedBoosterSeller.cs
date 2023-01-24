using System;
using UnityEngine;

public class SpeedBoosterSeller : Seller
{
    [SerializeField] private SpeedBoosterButton _speedBoosterButton;

    private float _additionalSpeed = 0.05f;

    public float AdditionalSpeed => _additionalSpeed;

    public event Action ImprovementPurchased;

    private void Start()
    {
        PriceForOnePiece = ES3.Load(SaveProgress.TitleKey.PriceForOnePieceSpeedBooster, SaveProgress.FilePath.Sellers, PriceForOnePiece);
        _additionalSpeed = ES3.Load(SaveProgress.TitleKey.AdditionalSpeedSpeedBooster, SaveProgress.FilePath.Sellers, _additionalSpeed);
    }

    private void OnEnable()
    {
        _speedBoosterButton.ButtonPressed += RaisePrice;
    }

    private void OnDisable()
    {
        _speedBoosterButton.ButtonPressed -= RaisePrice;
    }

    public override void RaisePrice()
    {
        ImprovementPurchased?.Invoke();
        PriceForOnePiece += Surchange;

        ES3.Save(SaveProgress.TitleKey.PriceForOnePieceSpeedBooster, PriceForOnePiece, SaveProgress.FilePath.Sellers);
        ES3.Save(SaveProgress.TitleKey.AdditionalSpeedSpeedBooster, _additionalSpeed, SaveProgress.FilePath.Sellers);
    }
}
