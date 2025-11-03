using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNormalAttackBullet : MonoBehaviour
{
    private float bulletSpeed;
    private Vector3 bulletDir;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpeed = 7.5f;
    }

    private void OnEnable()
    {
        bulletDir = GameManager.Instance.bossDir;
        transform.position = GameManager.Instance.bossTra.position + new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += bulletDir * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.name.Substring(0, 5) == "Angel")
                GameManager.Instance.ac.TakeDamage(GameManager.Instance.rangedBossNormalAttackDamage);
            else if (other.gameObject.name.Substring(0, 5) == "Demon")
                GameManager.Instance.dc.TakeDamage(GameManager.Instance.rangedBossNormalAttackDamage);
            else if (other.gameObject.name.Substring(0, 5) == "Fairy")
                GameManager.Instance.fc.TakeDamage(GameManager.Instance.rangedBossNormalAttackDamage);

            GameObject obj = ObjectPool.Instance.PopFromPool("BulletEffect");
            obj.transform.position = gameObject.transform.position;
            ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
        }
    }
}
