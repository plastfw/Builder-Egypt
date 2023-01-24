using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TraffickerButton : BuyButton
{
    public event Action ButtonPressed;

    private void Start()
    {
        LevelText.text = ES3.Load(SaveProgress.TitleKey.LevelTextSlaveButton, SaveProgress.FilePath.Buttons, LevelText.text);
        TextPrice.text = ES3.Load(SaveProgress.TitleKey.TextPriceSlaveButton, SaveProgress.FilePath.Buttons, TextPrice.text);
        Level = ES3.Load(SaveProgress.TitleKey.LevelNumberSlaveButton, SaveProgress.FilePath.Buttons, Level);

        Button.interactable = false;
    }

    public override void OnButtonClick()
    {
        AddLevel();
        StartScaleAnimation();

        if (ES3.KeyExists(SaveProgress.TitleKey.SoftCount, SaveProgress.FilePath.Analytic))
        {
            SoftCount = ES3.Load(SaveProgress.TitleKey.SoftCount, SaveProgress.FilePath.Analytic, SoftCount);
            SoftCount++;
        }
        else
            SoftCount++;

        ES3.Save(SaveProgress.TitleKey.SoftCount, SoftCount, SaveProgress.FilePath.Analytic);

        Analytic.SendEventOnSoftSpend("Improvement", "Buying a stickman", Seller.Price, SoftCount);

        Wallet.ReduceCost(Seller.Price);
        ButtonPressed?.Invoke();
        TextPrice.text = FormatNumberHelper.FormatNumber(Seller.Price);

        if (Wallet.Coin < Seller.Price)
        {
            Button.interactable = false;
            TextPrice.text = FormatNumberHelper.FormatNumber(Seller.Price);
        }

        ES3.Save(SaveProgress.TitleKey.LevelTextSlaveButton, LevelText.text, SaveProgress.FilePath.Buttons);
        ES3.Save(SaveProgress.TitleKey.TextPriceSlaveButton, TextPrice.text, SaveProgress.FilePath.Buttons);
        ES3.Save(SaveProgress.TitleKey.LevelNumberSlaveButton, Level, SaveProgress.FilePath.Buttons);
    }
}
