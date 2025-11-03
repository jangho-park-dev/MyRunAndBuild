using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonicController : MonoBehaviour
{
    private bool onceForCoroutine;
    private int layerMask;

    public Coroutine demonicAttackTimer;
    public Coroutine demonicDeadTimer;
    public Animator animator;
    public float maxHp = 300f;
    public float demonicHp = 300f;
    public float demonicPower = 30f;
    public Vector3 basePosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("Run", true);

        demonicAttackTimer = null;
        layerMask = 1 << LayerMask.NameToLayer("Enemy");    // enemy 레이어만 충돌 체크

        basePosition = new Vector3(2f, 0f, 16.34f);

        transform.position = basePosition;
    }

    private void OnEnable()
    {
        onceForCoroutine = false;
        demonicHp = maxHp;
    }

    private void OnDisable()
    {
        if (demonicAttackTimer != null)
        {
            StopCoroutine(demonicAttackTimer);
        }
        if (demonicDeadTimer != null)
        {
            StopCoroutine(demonicDeadTimer);
        }
    }

    void Update()
    {
        if (GameManager.Instance.meetEnemy && !onceForCoroutine)
        {
            animator.SetBool("Run", false);
            demonicAttackTimer = StartCoroutine(PlayerAttackTimer());
            onceForCoroutine = true;
        }
    }

    IEnumerator PlayerAttackTimer()
    {
        while (true)
        {
            LookAtEnemy();
            yield return new WaitForSeconds(0.5f);
            TargetToEnemy();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void LookAtEnemy()
    {
        GameManager.Instance.enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        if (GameManager.Instance.enemyArray.Length == 0)
            return;

        int saveNumber = 0;
        float minDistance = DistanceToEnemy(GameManager.Instance.enemyArray[0]);
        for (int i = 1; i < GameManager.Instance.enemyArray.Length; ++i)
        {
            if (minDistance >= DistanceToEnemy(GameManager.Instance.enemyArray[i]))
            {
                minDistance = DistanceToEnemy(GameManager.Instance.enemyArray[i]);
                saveNumber = i;
            }
        }
        transform.LookAt(GameManager.Instance.enemyArray[saveNumber].transform);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Left Attack 01"))
        {
            if (demonicHp > 0f)
                animator.Play("Right Punch Attack");
        }
    }

    private void TargetToEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.75f, 0f), transform.forward, out hit, 10f, layerMask))
        {
            //Debug.DrawRay(transform.position + new Vector3(0f, 0.75f, 0f), transform.forward * 10f, Color.red, 10f);

            if (hit.collider.gameObject.name.Substring(0, 4) == "Enem")
                hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(demonicPower);
            else
                hit.collider.gameObject.GetComponent<BossController>().TakeDamage(demonicPower);
        }
    }

    private float DistanceToEnemy(GameObject enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position);
    }

    public void TakeDamage(float damage)
    {
        demonicHp -= damage;

        GameManager.Instance.dHpBarImage.fillAmount = demonicHp / maxHp;

        CheckToDead();
    }

    private void CheckToDead()
    {
        if (demonicHp <= 0f)
        {
            animator.Play("Die");
            demonicDeadTimer = StartCoroutine(CheckToDeadTimer());
        }
    }

    IEnumerator CheckToDeadTimer()
    {
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.dBtn.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Initialize()        // 플레이어 앞 적이 다 죽으면 실행할 메소드
    {
        GameManager.Instance.meetEnemy = false;
        onceForCoroutine = false;
        StopCoroutine(demonicAttackTimer);
    }
}
