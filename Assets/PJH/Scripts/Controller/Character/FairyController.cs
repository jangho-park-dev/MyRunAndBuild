using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyController : MonoBehaviour
{
    private bool onceForCoroutine;
    private int layerMask;

    public Coroutine fairyAttackTimer;
    public Coroutine fairyDeadTimer;
    public Animator animator;
    public float maxHp = 300f;
    public float fairyHp = 300f;
    public float fairyPower = 40f;
    public Vector3 fairyDir;
    public Vector3 basePosition;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("Fly Idle", true);
        animator.SetBool("Fly Forward", true);

        fairyAttackTimer = null;
        layerMask = 1 << LayerMask.NameToLayer("Enemy");    // enemy 레이어만 충돌 체크

        fairyDir = new Vector3(0f, 0f, 0f);
        basePosition = new Vector3(0f, 0f, 13.79f);

        transform.position = basePosition;
    }

    private void OnEnable()
    {
        onceForCoroutine = false;
        fairyHp = maxHp;
    }

    private void OnDisable()
    {
        if (fairyAttackTimer != null)
        {
            StopCoroutine(fairyAttackTimer);
        }
        if (fairyDeadTimer != null)
        {
            StopCoroutine(fairyDeadTimer);
        }
    }

    void Update()
    {
        if (GameManager.Instance.meetEnemy && !onceForCoroutine)
        {
            animator.SetBool("Fly Forward", false);
            fairyAttackTimer = StartCoroutine(PlayerAttackTimer());
            onceForCoroutine = true;
        }
    }

    IEnumerator PlayerAttackTimer()
    {
        while (true)
        {
            TargetToEnemy();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Cast Spell 02"))
            {
                if (fairyHp > 0f)
                    animator.Play("Fly Cast Spell 01");
            }
            yield return new WaitForSeconds(2);
        }
    }

    private void TargetToEnemy()
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
        // 첫번째 공격 이후 타겟이 바뀌는데 확인해보니 제일 거리 가까운 애한테 공격하는거였음
        // 왜 첫번째 타겟 정할 때랑 두번째 이후부터랑 타겟 거리가 달라지는지 모르겠음
        // --> 첫번째로 검사할 때랑 다음으로 검사할 때 프레임 및 이동 시작 지점에 따른 거리 차이가 있는 듯 함
        transform.LookAt(GameManager.Instance.enemyArray[saveNumber].transform);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.75f, 0f), transform.forward, out hit, 10f, layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0f, 0.75f, 0f), transform.forward * 10f, Color.red, 10f);
            fairyDir = transform.forward;       // bullet의 방향을 위한 변수
            Invoke("ShootTheBullet", 0.5f);
        }
    }

    private void ShootTheBullet()
    {
        ObjectPool.Instance.PopFromPool("FairyBullet");
    }

    private float DistanceToEnemy(GameObject enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position);
    }

    public void TakeDamage(float damage)
    {
        fairyHp -= damage;

        GameManager.Instance.fHpBarImage.fillAmount = fairyHp / maxHp;

        CheckToDead();
    }

    private void CheckToDead()
    {
        if (fairyHp <= 0f)
        {
            animator.Play("Fly Die");
            fairyDeadTimer = StartCoroutine(CheckToDeadTimer());
        }
    }

    IEnumerator CheckToDeadTimer()
    {
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.fBtn.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Initialize()        // 플레이어 앞 적이 다 죽으면 실행할 메소드
    {
        GameManager.Instance.meetEnemy = false;
        onceForCoroutine = false;
        StopCoroutine(fairyAttackTimer);
    }
}
