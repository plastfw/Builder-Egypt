using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Firework : MonoBehaviour
{
    [SerializeField] private Pharaoh _pharaoh;

    private ParticleSystem _particleSystem;

    private void OnEnable()
    {
        _pharaoh.PharaohIsComing += Show;
    }

    private void OnDisable()
    {
        _pharaoh.PharaohIsComing -= Show;
    }

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Show()
    {
        StartCoroutine(StartParticle());
    }

    private IEnumerator StartParticle()
    {
        float delay = 5.8f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        _particleSystem.Play();
    }
}
