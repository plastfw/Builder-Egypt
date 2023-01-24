using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private float _coin;
    [SerializeField] private Building _building;
    [SerializeField] private IncomeBoosterSeller _incomeBoosterSeller;

    public float Coin
    {
        get
        {
            return _coin;
        }
        private set
        {
            _coin = value;
            CoinChanged?.Invoke(_coin);
            ES3.Save(SaveProgress.TitleKey.Coin, _coin, SaveProgress.FilePath.Coin);
        }
    }

    public event Action<float> CoinChanged;

    private void Awake()
    {
        Onload();
    }

    private void OnEnable()
    {
        _building.BuildingPartBuilt += AddCoin;
    }

    private void OnDisable()
    {
        _building.BuildingPartBuilt -= AddCoin;
    }

    public void ReduceCost(float coin)
    {
        Coin -= coin;
    }

    private void Onload()
    {
        _coin = ES3.Load(SaveProgress.TitleKey.Coin, SaveProgress.FilePath.Coin, _coin);
    }

    private void AddCoin(BuildingPart buildingPart)
    {
        Coin += _incomeBoosterSeller.AdditionalCoin;
    }
}
