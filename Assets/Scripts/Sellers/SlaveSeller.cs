using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlaveSeller : Seller
{
    [SerializeField] private List<Slave> _slaves = new List<Slave>();
    [SerializeField] private Slave _slavePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TraffickerButton _traffickerButton;
    [SerializeField] private SpeedBoosterSeller _speedBoosterSeller;

    private int _slaveCount;

    public event Action<Slave> SlaveIsSold;

    private void Awake()
    {
        PriceForOnePiece = ES3.Load(SaveProgress.TitleKey.PriceForOnePieceSlave, SaveProgress.FilePath.Sellers, PriceForOnePiece);
    }

    private void Start()
    {
        OnLoad();
    }

    private void OnEnable()
    {
        _traffickerButton.ButtonPressed += SpawnSlave;
        _speedBoosterSeller.ImprovementPurchased += IncreaseSpeed;
    }

    private void OnDisable()
    {
        _traffickerButton.ButtonPressed -= SpawnSlave;
        _speedBoosterSeller.ImprovementPurchased -= IncreaseSpeed;
    }

    public void IncreaseSpeed()
    {
        for (int i = 0; i < _slaves.Count; i++)
        {
            _slaves[i].IncreaseSpeed(_speedBoosterSeller.AdditionalSpeed);
        }
    }

    private void OnLoad()
    {
        if (ES3.KeyExists(SaveProgress.TitleKey.SlaveCount, SaveProgress.FilePath.Sellers))
        {
            _slaveCount = ES3.Load(SaveProgress.TitleKey.SlaveCount, SaveProgress.FilePath.Sellers, _slaveCount);
            StartCoroutine(SpawnSavedSlaves());
        }
    }

    public override void RaisePrice()
    {
        PriceForOnePiece += Surchange;

        ES3.Save(SaveProgress.TitleKey.PriceForOnePieceSlave, PriceForOnePiece, SaveProgress.FilePath.Sellers);

        if (_slaves.Count > 1)
            ES3.Save(SaveProgress.TitleKey.SlaveCount, _slaves.Count - 1, SaveProgress.FilePath.Sellers);
    }

    private IEnumerator SpawnSavedSlaves()
    {
        int startValue = 1;
        int numberOfPurchasedBoosters = ES3.Load(SaveProgress.TitleKey.LevelNumberSpeedBooster, SaveProgress.FilePath.Buttons, startValue);
        float delay = 0.4f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        for (int i = 0; i < _slaveCount; i++)
        {
            Slave newSlave = Instantiate(_slavePrefab, _spawnPoint.position, transform.rotation, transform);
            _slaves.Add(newSlave);
            SlaveIsSold?.Invoke(newSlave);

            yield return waitForSeconds;
        }
    }

    private void SpawnSlave()
    {
        Slave newSlave = Instantiate(_slavePrefab, _spawnPoint.position, transform.rotation, transform);
        _slaves.Add(newSlave);
        RaisePrice();
        SlaveIsSold?.Invoke(newSlave);
    }
}
