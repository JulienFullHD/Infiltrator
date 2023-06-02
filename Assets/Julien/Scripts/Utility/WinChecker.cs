using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChecker : MonoBehaviour
{
    [SerializeField]private List<Transform> enemyList = new List<Transform>();
    [SerializeField]private Transform winText;
    private int enemyCount;

    private void Start() 
    {
        enemyCount = enemyList.Count;
    }
    public void EnemyKilled()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            winText.gameObject.SetActive(true);
            StartCoroutine(WaitAndLoadScene(5.0f));
        }
    }
    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
