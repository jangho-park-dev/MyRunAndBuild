using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 dir;
    private bool onceForCoroutine;
    private Image hpBarImage;
    private GameObject hb;

    public const float MAX_HP = 100f;
    public const float SPEED = 25f;
    public float enemySpeed;
    public float enemyHp;
    public float enemyPower;

    public Animator animator;
    public bool meetPlayer;
    public bool attackPlayer;
    public Coroutine enemyAttackTimer;
    public Coroutine enemyDeadTimer;

    public Vector3 hpBarOffset = new Vector3(0f, 2.2f, 0f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enemySpeed = SPEED;
        enemyHp = MAX_HP;
        enemyPower = 10f;

        enemyAttackTimer = null;
    }

    private void OnEnable()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);

        // 재사용 시 초기화되어야 할 것들
        dir = new Vector3(0f, 0f, -1f);
        transform.Rotate(new Vector3(0f, 180f, 0f));
        meetPlayer = false;
        attackPlayer = false;
        onceForCoroutine = false;
        enemyHp = MAX_HP;
        enemySpeed = SPEED;

        SetHpBar();
    }

    private void OnDisable()
    {
        if (enemyAttackTimer != null)
        {
            StopCoroutine(enemyAttackTimer);
        }
        if (enemyDeadTimer != null)
        {
            StopCoroutine(enemyDeadTimer);
        }

        if (hb != null)
        {
            if (hb.transform.parent.name == "UI Canvas for enemy")
            {
                hb.transform.parent = ObjectPool.Instance.gameObject.transform;
                ObjectPool.Instance.PushToPool(hb.name, hb);
            }
            else
            {
                ObjectPool.Instance.PushToPool(hb.name, hb);
            }
        }
    }

    private void Update()
    {
        if (DistanceToPlayer() <= 20f && !meetPlayer && !attackPlayer)
        {
            if (Time.timeScale != 0)
                enemySpeed -= 0.04f;
        }

        if (DistanceToPlayer() <= 5f && !meetPlayer && !attackPlayer)
        {
            meetPlayer = true;
            attackPlayer = true;
            animator.SetBool("Run", false);

            GameManager.Instance.meetEnemy = true;
        }

        if (!meetPlayer && !attackPlayer)
        {
            MoveToPlayer();
        }
        
        if (attackPlayer && !onceForCoroutine)
        {
            enemyAttackTimer = StartCoroutine(EnemyAttackTimer());
            onceForCoroutine = true;
        }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.transform.position);
    }

    private void MoveToPlayer()
    {
        transform.LookAt(GameManager.Instance.fc.transform);
        transform.position += dir * enemySpeed * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        enemyHp -= damage;
        
        if (enemyHp > 0f)
            animator.Play("Take Damage");

        hpBarImage.fillAmount = enemyHp / MAX_HP;

        CheckToDead();
    }

    private void CheckToDead()
    {
        if (enemyHp <= 0f)
        {
            animator.Play("Die");
            enemyDeadTimer = StartCoroutine(CheckToDeadTimer());
        }
    }

    IEnumerator CheckToDeadTimer()
    {
        yield return new WaitForSeconds(1.0f);

        DataManager.Instance.UserData.money += 100;
        GameManager.Instance.gameMoney += 100;
        GameManager.Instance.moneyText.text = GameManager.Instance.gameMoney.ToString();
        ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
    }

    private void DamageDistribution()
    {
        if (GameManager.Instance.ac && GameManager.Instance.ac.angelHp > 0f)
        {
            GameManager.Instance.ac.TakeDamage(enemyPower);
        }
        else if (GameManager.Instance.dc && GameManager.Instance.dc.demonicHp > 0f)
        {
            GameManager.Instance.dc.TakeDamage(enemyPower);
        }
        else if (GameManager.Instance.fc && GameManager.Instance.fc.fairyHp > 0f)
        {
            GameManager.Instance.fc.TakeDamage(enemyPower);
        }
    }

    private void SetHpBar()
    {
        hb = ObjectPool.Instance.PopFromPool("EnemyHpBar");
        hb.transform.parent = GameManager.Instance.uiCanvas.GetComponent<Transform>();
        hpBarImage = hb.GetComponentsInChildren<Image>()[1];
        hpBarImage.fillAmount = MAX_HP;

        var hpBar = hb.GetComponent<EnemyHpBarController>();
        hpBar.targetTransform = gameObject.transform;
        hpBar.offset = hpBarOffset;
    }

    IEnumerator EnemyAttackTimer()
    {
        while (true)
        {
            if (enemyHp > 0f)
            {
                if (transform.name == "Enemy01")
                {
                    animator.Play("Melee Left Attack 01");
                    DamageDistribution();
                }
                else if (transform.name == "Enemy02")
                {
                    animator.Play("Melee Right Attack 02");
                    DamageDistribution();
                }
                else if (transform.name == "Enemy03")
                {
                    animator.Play("Melee Right Attack 03");
                    DamageDistribution();
                }
                else if (transform.name == "Enemy04")
                {
                    animator.Play("Melee Right Attack 01");
                    DamageDistribution();
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
}
