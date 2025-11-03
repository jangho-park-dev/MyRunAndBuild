using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 dir;
    float treeSpeed = 25f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dir = new Vector3(0f, 0f, -1f);
        treeSpeed = 25f;
    }

    private void OnEnable()
    {
        treeSpeed = 25f;
    }

    private void OnBecameInvisible()
    {
        ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
    }

    private void Update()
    {
        if (GameManager.Instance.enemyArray.Length > 0)
        {
            if (GameManager.Instance.SearchEnemy() >= 0 &&
                GameManager.Instance.DistanceToEnemy(GameManager.Instance.enemyArray[GameManager.Instance.SearchEnemy()]) <= 20f &&
                !GameManager.Instance.meetEnemy)
            {
                treeSpeed -= 0.04f;
            }
        }

        if (!GameManager.Instance.meetEnemy)
        {
            transform.position += dir * treeSpeed * Time.deltaTime;
        }
    }
}
