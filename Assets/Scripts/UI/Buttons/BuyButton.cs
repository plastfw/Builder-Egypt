using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public abstract class BuyButton : MonoBehaviour
{
    const string LevelNumber = "lvl. "; 

    [SerializeField] protected TMP_Text LevelText;
    [SerializeField] protected TMP_Text TextPrice;
    [SerializeField] protected UnityEngine.GameObject Frame;
    [SerializeField] protected Wallet Wallet;
    [SerializeField] protected Seller Seller;
    [SerializeField] protected AnalyticManager Analytic;

    protected Button Button;
    protected Sequence ScaleAnimation;
    protected int Level = 1;
    protected float _currentCoin = 0;
    protected int SoftCount = 0;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Wallet.CoinChanged += UpdateButtonState;
    }

    private void OnDisable()
    {
        Wallet.CoinChanged -= UpdateButtonState;
    }

    public abstract void OnButtonClick();

    protected void StartScaleAnimation()
    {
        ScaleAnimation = DOTween.Sequence();
        float scale = 1.3f;
        Vector3 targetScale = new Vector3(scale, scale, scale);
        float duration = 0.2f;

        ScaleAnimation
            .Append(Frame.transform.DOScale(targetScale, duration))
            .Append(Frame.transform.DOScale(Vector3.one, duration));
    }

    protected void AddLevel()
    {
        Level++;
        LevelText.text = LevelNumber + Level.ToString();
    }

    protected virtual void UpdateButtonState(float coin)
    {
        if (coin < Seller.Price)
            Button.interactable = false;
        else
            Button.interactable = true;
    }
}
