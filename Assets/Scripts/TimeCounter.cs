using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TimeCounter : MonoBehaviour
{
    private const int MaximumTimeValue = 59;
    private const int TimeInDays = 23;
    private const int TenSeconds = 10;
    private const float OneSecond = 1f;
    private const int ZeroSecond = 0;
    private const string Separtor = ":";
    private const string ZeroTime = "0";
    private const string StartText = "Time Passed: ";

    [SerializeField] private Cart _cart;
    [SerializeField] private string _saveKeyForSeconds;
    [SerializeField] private string _saveKeyForMinutes;
    [SerializeField] private string _saveKeyForHours;

    private TMP_Text _textTime;
    private Coroutine _increaseTime;
    private int _hour;
    private int _minutes;
    private int _seconds;
    private string _hoursText;
    private string _minutesText;
    private string _secondsText;

    private void OnEnable()
    {
        _cart.StoneAreOver += StopIncreaseTime;
    }

    private void OnDisable()
    {
        _cart.StoneAreOver -= StopIncreaseTime;
    }

    private void Start()
    {
        _textTime = GetComponent<TMP_Text>();
        OnLoad();
        _increaseTime = StartCoroutine(IncreaseTime());
    }

    private void ConvertTime()
    {
        if (_seconds > MaximumTimeValue)
        {
            _seconds = ZeroSecond;
            _minutes++;
        }

        if (_minutes > MaximumTimeValue)
        {
            _minutes = ZeroSecond;
            _hour++;
        }

        if (_hour > TimeInDays)
            _hour = ZeroSecond;

        ShowTime();
    }

    private void OnLoad()
    {
        _seconds = ES3.Load(_saveKeyForSeconds, SaveProgress.FilePath.PassageTime, _seconds);
        _minutes = ES3.Load(_saveKeyForMinutes, SaveProgress.FilePath.PassageTime, _minutes);
        _hour = ES3.Load(_saveKeyForHours, SaveProgress.FilePath.PassageTime, _hour);
    }

    private void ShowTime()
    {
        if (_seconds < TenSeconds)
            _secondsText = ZeroTime + _seconds;
        else
            _secondsText = _seconds.ToString();

        if (_minutes < TenSeconds)
            _minutesText = ZeroTime + _minutes;
        else
            _minutesText = _minutes.ToString();

        if (_hour < TenSeconds)
            _hoursText = ZeroTime + _hour;
        else
            _hoursText = _hour.ToString();

        _seconds++;

        ES3.Save(_saveKeyForSeconds, _seconds, SaveProgress.FilePath.PassageTime);
        ES3.Save(_saveKeyForMinutes, _minutes, SaveProgress.FilePath.PassageTime);
        ES3.Save(_saveKeyForHours, _hour, SaveProgress.FilePath.PassageTime);

        _textTime.text = StartText + _hoursText + Separtor + _minutesText + Separtor + _secondsText;
    }

    private void StopIncreaseTime()
    {
        StopCoroutine(_increaseTime);
    }

    private IEnumerator IncreaseTime()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(OneSecond);

        while (true)
        {
            ConvertTime();
            yield return waitForSeconds;
        }
    }
}
