using System.Collections;
using UnityEngine;

public class CoinTossEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystems;

    public IEnumerator Show()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_particleSystems.duration);
        _particleSystems.Play();
        yield return waitForSeconds;
        gameObject.SetActive(false);
    }
}
