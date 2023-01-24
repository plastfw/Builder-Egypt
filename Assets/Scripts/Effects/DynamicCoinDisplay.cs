using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DynamicCoinDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public IEnumerator Show(string additionalMoney)
    {
        _text.text = "+ " + additionalMoney;
        float delay = 0.5f;
        float duration = 0.5f;
        float yOffset = 0.6f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        transform.DOMoveY(transform.position.y + yOffset, duration);
        yield return waitForSeconds;
        gameObject.SetActive(false);
    }
}
