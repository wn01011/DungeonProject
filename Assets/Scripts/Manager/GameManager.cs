using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private SpawnManager spawnManager = null;
    private GameObject boss = null;
    private GameObject bossSprite = null;
    private int wave = 1;
    private bool restartOn = false;
    
    void Start()
    {
        StartCoroutine(bossFinder());
        StartCoroutine(WaveCheck());
    }

    void Update()
    {

    }

    private IEnumerator bossFinder()
    {
        while(!boss)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            yield return new WaitForFixedUpdate();
        }
        bossSprite = boss.GetComponentsInChildren<Transform>()[1].gameObject;
        Debug.Log(bossSprite.name);
    }
    public IEnumerator WaveCheck()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => boss != null && bossSprite == null);
        Debug.Log("Boss Die!");
        ++wave;
        restartOn = true;
        yield return new WaitForSeconds(1.0f);
        spawnManager.Restart();
        yield return new WaitForSeconds(1.0f);
        levelManager.Restart();
        yield return new WaitForSeconds(1.0f);
        ReStart();
        restartOn = false;
    }
    public void ReStart()
    {
        StopAllCoroutines();
        for (int i = 0; i < GetComponentsInChildren<Transform>().Length - 2; ++i)
        {
            Destroy(GetComponentsInChildren<Transform>()[i].gameObject);
        }
        Start();
    }

}
