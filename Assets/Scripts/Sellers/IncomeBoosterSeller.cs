using UnityEngine;

public class IncomeBoosterSeller : Seller
{
    [SerializeField] private IncomeBoosterButton _incomeBoosterButton;
    [SerializeField] private int _additionalCoin;

    private int _currentAdditionalCoin = 500;

    public int AdditionalCoin => _currentAdditionalCoin;

    private void OnEnable()
    {
        _incomeBoosterButton.ButtonPressed += RaisePrice;
    }

    private void OnDisable()
    {
        _incomeBoosterButton.ButtonPressed -= RaisePrice;
    }

    private void Start()
    {
        PriceForOnePiece = ES3.Load(SaveProgress.TitleKey.PriceForOneIncomeBooster, SaveProgress.FilePath.Sellers, PriceForOnePiece);
        _currentAdditionalCoin = ES3.Load(SaveProgress.TitleKey.AdditionalCoinIncomeBooster, SaveProgress.FilePath.Sellers, _currentAdditionalCoin);
    }

    public override void RaisePrice()
    {
        _currentAdditionalCoin += _additionalCoin;

        PriceForOnePiece += Surchange;

        ES3.Save(SaveProgress.TitleKey.PriceForOneIncomeBooster, PriceForOnePiece, SaveProgress.FilePath.Sellers);
        ES3.Save(SaveProgress.TitleKey.AdditionalCoinIncomeBooster, _currentAdditionalCoin, SaveProgress.FilePath.Sellers);
    }
}
