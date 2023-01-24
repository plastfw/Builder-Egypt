using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CameraMover : MonoBehaviour
{
    [SerializeField] private Cart _cart;

    private PlayableDirector _playableDirector;
    private bool _isWork = true;

    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        _cart.StoneAreOver += ShowPharaoh;
    }

    private void OnDisable()
    {
        _cart.StoneAreOver -= ShowPharaoh;
    }

    private void ShowPharaoh()
    {
        if (_isWork == true)
        {
            _isWork = false;
            _playableDirector.Play();
        }
    }
}
