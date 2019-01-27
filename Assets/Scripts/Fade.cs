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

    public GameObject FadeFunc()
    {
        return Instantiate(FadeIn, transform);
    }

    IEnumerator StartFadeAnim()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(StartFade);
    }
}
