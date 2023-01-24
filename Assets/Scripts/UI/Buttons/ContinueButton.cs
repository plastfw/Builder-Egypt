using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour
{
    const string Map = nameof(Map);

    public event Action ButtonPresed;

    public void OnButtonClick()
    {
        ButtonPresed?.Invoke();
        StartCoroutine(LoadMap());
    }

    private IEnumerator LoadMap()
    {
        float delay = 2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        SceneManager.LoadScene(Map);
    }
}
