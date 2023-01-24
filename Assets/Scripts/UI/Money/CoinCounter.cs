using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    private const string Coin = nameof(Coin);

    [SerializeField] private Wallet _wallet;

    private TMP_Text _coinText;

    private void Start()
    {
        _coinText = GetComponentInChildren<TMP_Text>();
        OnLoad();
    }

    private void OnLoad()
    {
        float initialCoin = 20000;
        float loadedCoin = ES3.Load(Coin, SaveProgress.FilePath.Coin, initialCoin);
        _coinText.text = FormatNumberHelper.FormatNumber(loadedCoin);
    }

    private void OnEnable()
    {
        _wallet.CoinChanged += ChangeMoney;
    }

    private void OnDisable()
    {
        _wallet.CoinChanged -= ChangeMoney;
    }

    private void ChangeMoney(float coin)
    {
        _coinText.text = FormatNumberHelper.FormatNumber(coin);
    }
}
