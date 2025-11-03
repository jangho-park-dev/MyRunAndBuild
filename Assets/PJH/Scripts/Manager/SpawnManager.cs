using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : Singleton<SpawnManager>
{
    private string treeName = "Tree";
    private string enemyName = "Enemy";
    private int treeCount = 3;
    private int enemyCount = 4;
    private Vector3 treeSpawnOffSet;
    private Vector3 enemySpawnOffSet;

    public Coroutine treeSpawningCoroutine;
    public Coroutine monsterSpawningCoroutine;
    public bool treeSpawnFlag;

    // 오브젝트가 출현할 위치를 담을 배열
    public Transform[] spawnPoints;

    void Start()
    {
        // Hierarchy View의 Spawn Point를 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        spawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        treeSpawningCoroutine = StartCoroutine(StartTreeSpawning());
        monsterSpawningCoroutine = StartCoroutine(StartMonsterSpawning());

        treeSpawnFlag = true;

        treeSpawnOffSet = new Vector3(0f, 1f, 0f);
        enemySpawnOffSet = new Vector3(0f, 1.21f, 0f);
    }

    void Update()
    {
        if (GameManager.Instance.meetEnemy && treeSpawnFlag)
        {
            StopCoroutine(treeSpawningCoroutine);
            StopCoroutine(monsterSpawningCoroutine);
            treeSpawnFlag = false;
        }
    }

    public bool GetTreeSpawnFlag()
    {
        return treeSpawnFlag;
    }

    public void MethodForStartingTreeSpawn()
    {
        treeSpawningCoroutine = StartCoroutine(StartTreeSpawning());
    }

    public void MethodForStartingMonsterSpawn()
    {
        monsterSpawningCoroutine = StartCoroutine(StartMonsterSpawning());
    }

    public IEnumerator StartTreeSpawning()
    {
        while (true)
        {
            foreach (var point in spawnPoints)
            {
                if (point.name.Substring(0, 4) == "tree")
                {
                    int randNum = Random.Range(0, treeCount);
                    switch (randNum)
                    {
                        case 0:
                            treeName += "01";
                            break;
                        case 1:
                            treeName += "02";
                            break;
                        case 2:
                            treeName += "03";
                            break;
                    }
                    GameObject tree = ObjectPool.Instance.PopFromPool(treeName);
                    tree.transform.position = point.position + treeSpawnOffSet;
                    treeName = "Tree";
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator StartMonsterSpawning()
    {
        yield return new WaitForSeconds(2f);
        foreach (var point in spawnPoints)
        {
            if (point.name.Substring(0, 6) == "enemyP")    // 몹
            {
                switch (Random.Range(0, enemyCount))
                {
                    case 0:
                        enemyName += "01";
                        break;
                    case 1:
                        enemyName += "02";
                        break;
                    case 2:
                        enemyName += "03";
                        break;
                    case 3:
                        enemyName += "04";
                        break;
                }
                GameObject enemy = ObjectPool.Instance.PopFromPool(enemyName);
                enemy.transform.position = point.position + enemySpawnOffSet;
                enemyName = "Enemy";
            }
            else if (GameManager.Instance.gameLevel == 3 && point.name.Substring(0, 6) == "enemyB")    // 보스
            {
                switch(SceneManager.GetActiveScene().name)
                {
                    case "FirstStage":
                        {
                            GameObject boss = ObjectPool.Instance.PopFromPool("FirstBoss");
                            boss.transform.position = point.position + enemySpawnOffSet;
                            break;
                        }
                    case "MiddleStage":
                        {
                            GameObject boss = ObjectPool.Instance.PopFromPool("MiddleBoss");
                            boss.transform.position = point.position + enemySpawnOffSet;
                            break;
                        }
                    case "FinalStage":
                        {
                            GameObject boss = ObjectPool.Instance.PopFromPool("FinalBoss");
                            boss.transform.position = point.position + enemySpawnOffSet;
                            break;
                        }
                }
            }
        }
    }
}
