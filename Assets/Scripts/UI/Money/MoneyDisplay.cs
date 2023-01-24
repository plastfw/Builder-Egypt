using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MoneyPool
{
    [SerializeField] private Building _building;
    [SerializeField] private IncomeBoosterSeller _incomeBoosterSeller;
    [SerializeField] private DynamicCoinDisplay _dynamicCoinDisplay;
    [SerializeField] private CoinTossEffect _coinTossEffect;

    private void OnEnable()
    {
        _building.BuildingPartBuilt += ShowMoney;
    }

    private void OnDisable()
    {
        _building.BuildingPartBuilt -= ShowMoney;
    }

    private void Start()
    {
        InitializeDynamicCoinDisplays(_dynamicCoinDisplay);
        InitializeCoinTossEffect(_coinTossEffect);
    }

    private void ShowMoney(BuildingPart buildingPart)
    {
        string number = FormatNumberHelper.FormatNumber(_incomeBoosterSeller.AdditionalCoin);
        float yOffset1 = 1f;
        float yOffset2 = 2f;
        Vector3 targetPosition1 = SetNewPosition(buildingPart, yOffset1);
        Vector3 targetPosition2 = SetNewPosition(buildingPart, yOffset2);

        if (TryGetObjectDynamicCoinDisplay(out DynamicCoinDisplay dynamicCoinDisplay))
        {
            dynamicCoinDisplay.transform.position = targetPosition2;
            dynamicCoinDisplay.gameObject.SetActive(true);
            StartCoroutine(dynamicCoinDisplay.Show(number));
        }

        if (TryGetObjectCoinTossEffect(out CoinTossEffect coinTossEffect) == true)
        {
            coinTossEffect.gameObject.SetActive(true);
            coinTossEffect.transform.position = targetPosition1;
            StartCoroutine(coinTossEffect.Show());
        }
    }

    private Vector3 SetNewPosition(BuildingPart buildingPart, float yOffset)
    {
        Vector3 position = new Vector3(buildingPart.transform.position.x, buildingPart.transform.position.y + yOffset, buildingPart.transform.position.z);
        return position;
    }
}
