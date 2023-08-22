using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Script to let the main menu fade in from black
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

    /// <summary>
    /// Makes the black overlay fade away over time
    /// </summary>
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
