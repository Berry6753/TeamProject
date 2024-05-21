using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject playScreen;

    [SerializeField] private Image panel;
    float time = 0.0f;
    float F_time = 1.0f;

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }

    private IEnumerator FadeFlow()
    {
        panel.gameObject.SetActive(true);
        time = 0.0f;
        Color alpha = panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            panel.color = alpha;
            yield return null;
        }

        time = 0.0f;

        yield return new WaitForSeconds(1.0f);
        playScreen.SetActive(true);
        start.SetActive(false);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            panel.color = alpha;
            yield return null;
        }

        panel.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        startMenu.SetActive(false);

        yield return null;
    }
}
