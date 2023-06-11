using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeInDelay;
    private float timerDelay;
    private float timerDuration;

    private void Start()
    {
        timerDelay = 0;
        timerDuration = 0;
    }

    private void Update()
    {
        timerDelay += Time.deltaTime;

        if(timerDelay > fadeInDelay)
        {
            timerDuration += Time.deltaTime;

            canvas.alpha = 1 - (timerDuration / fadeInDuration);
        }

        if(timerDuration > fadeInDuration)
        {
            Destroy(this);
        }
    }
}
