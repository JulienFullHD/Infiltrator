using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BotSounds : MonoBehaviour

{
    [SerializeField] private Animator animator;
    [SerializeField] private AK.Wwise.Event EnemyFootsteps;
    private bool EnemyFootstepIsPlaying = false;
    private float lastEnemyFootstepTime = 0;



    void Update()
    {
        if (animator.GetBool("Attack") == true)
        {
            if (!EnemyFootstepIsPlaying && lastEnemyFootstepTime > 0.50)
            {
                EnemyFootsteps.Post(gameObject);
                lastEnemyFootstepTime = 0;
            }
            lastEnemyFootstepTime += Time.deltaTime;
            EnemyFootstepIsPlaying = false;

        }
    }
}
