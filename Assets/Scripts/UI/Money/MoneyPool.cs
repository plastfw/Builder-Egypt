using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyPool : MonoBehaviour
{
    [SerializeField] private GameObject Container1;
    [SerializeField] private GameObject Container2;
    [SerializeField] private int _capacity;

    private List<DynamicCoinDisplay> _gameObjects1 = new List<DynamicCoinDisplay>();
    private List<CoinTossEffect> _gameObjects2 = new List<CoinTossEffect>();

    protected void InitializeDynamicCoinDisplays(DynamicCoinDisplay dynamicCoinDisplay)
    {
        for (int i = 0; i < _capacity; i++)
        {
            DynamicCoinDisplay spawned = Instantiate(dynamicCoinDisplay, Container1.transform);

            spawned.gameObject.SetActive(false);
            _gameObjects1.Add(spawned);
        }
    }

    protected void InitializeCoinTossEffect(CoinTossEffect coinEffect)
    {
        for (int i = 0; i < _capacity; i++)
        {
            CoinTossEffect spawned = Instantiate(coinEffect, Container2.transform);

            spawned.gameObject.SetActive(false);
            _gameObjects2.Add(spawned);
        }
    }

    protected bool TryGetObjectDynamicCoinDisplay(out DynamicCoinDisplay result)
    {
        int randomIndex = Random.Range(0, _gameObjects1.Count);
        result = _gameObjects1.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result != null;
    }

    protected bool TryGetObjectCoinTossEffect(out CoinTossEffect result)
    {
        int randomIndex = Random.Range(0, _gameObjects1.Count);

        result = _gameObjects2.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result != null;
    }
}
