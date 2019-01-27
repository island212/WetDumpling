using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject FadeIn;
    public GameObject StartFade;

    void Start()
    {
        StartCoroutine(StartFadeAnim());
    }

    public IEnumerator FadeAnim()
    {
        GameObject tempFade = Instantiate(FadeIn, transform);
        yield return new WaitForSeconds(3.0f);
        Destroy(tempFade);
    }

    IEnumerator StartFadeAnim()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(StartFade);
    }
}
