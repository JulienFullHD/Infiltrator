using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] public static LevelManager Instance;

    [Header("Level Details")]
    [SerializeField] private int levelIdx;
    [SerializeField] private string levelName;

    [Header("Enemy Details")]
    [ReadOnly, SerializeField] private int enemyCount; // Takes count on level Start()

    [Header("Win Event")]
    [SerializeField] private bool isWon;
    [SerializeField] private GameObject playerCanvasObject;     // Fade out on win
    [SerializeField] private CanvasGroup playerCanvasGroup;     // Fade out on win
    [SerializeField] private GameObject winCanvasObject; // Fade  in on win
    [SerializeField] private CanvasGroup winCanvasGroup; // Fade  in on win
    [SerializeField] private float fadeMaxTime;
    [ReadOnly, SerializeField] private float fadeTimer;

    private void Start()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        isWon = false;
        EnemyCount = FindObjectsOfType(typeof(AI_HPSystem)).Count();
    }

    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
            if(enemyCount == 0)
            {
                Win();
            }
        }
    }

    public void Win()
    {
        isWon = true;
        winCanvasObject.SetActive(true);
        fadeTimer = 0;
    }

    private void Update()
    {
        if (isWon)
        {
            fadeTimer += Time.deltaTime;

            playerCanvasGroup.alpha = 1 - (fadeTimer / fadeMaxTime);
            winCanvasGroup.alpha = fadeTimer / fadeMaxTime;


        }
    }
}
