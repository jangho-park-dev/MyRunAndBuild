using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyNormalAttackBullet : MonoBehaviour
{
    private float bulletSpeed;
    private Vector3 bulletDir;

    void Start()
    {
        bulletSpeed = 7.5f;
    }

    private void OnEnable()
    {
        if (GameManager.Instance.fc)
        {
            bulletDir = GameManager.Instance.fc.fairyDir;
            transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
        }
    }

    void Update()
    {
        if (GameManager.Instance.fc)
            transform.position += bulletDir * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && GameManager.Instance.fc)
        {
            if (other.gameObject.name.Substring(0, 4) == "Enem")
                other.gameObject.GetComponent<EnemyController>().TakeDamage(GameManager.Instance.fc.fairyPower);
            else
                other.gameObject.GetComponent<BossController>().TakeDamage(GameManager.Instance.fc.fairyPower);

            GameObject obj = ObjectPool.Instance.PopFromPool("BulletEffect");
            obj.transform.position = gameObject.transform.position;
            ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
        }
    }
}
