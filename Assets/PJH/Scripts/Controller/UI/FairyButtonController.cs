using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FairyButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btn;
    public Coroutine fairySkillCoroutine;

    private float fairyHealAmount;

    private void Awake()
    {
        fairyHealAmount = 70f;
    }

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();

        fairySkillCoroutine = null;
    }

    private void OnDisable()
    {
        if (fairySkillCoroutine != null)
        {
            StopCoroutine(fairySkillCoroutine);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn.enabled)
        {
            if (!GameManager.Instance.meetEnemy)
            {
                GameManager.Instance.fc.animator.SetBool("Fly Forward", false);
                GameManager.Instance.fc.animator.Play("Fly Cast Spell 02");
                fairySkillCoroutine = StartCoroutine(FairySkill());
                GameManager.Instance.fc.animator.SetBool("Fly Forward", true);
            }
            else
            {
                GameManager.Instance.fc.animator.Play("Fly Cast Spell 02");
                fairySkillCoroutine = StartCoroutine(FairySkill());
            }
        }
    }

    IEnumerator FairySkill()
    {
        yield return new WaitForSeconds(0.5f);

        if (GameManager.Instance.ac.angelHp > 0f)
        {
            ObjectPool.Instance.PopFromPool("FairySkill").transform.position = GameManager.Instance.ac.transform.position;
        }
        if (GameManager.Instance.dc.demonicHp > 0f)
        {
            ObjectPool.Instance.PopFromPool("FairySkill").transform.position = GameManager.Instance.dc.transform.position;
        }
        if (GameManager.Instance.fc.fairyHp > 0f)
        {
            ObjectPool.Instance.PopFromPool("FairySkill").transform.position = GameManager.Instance.fc.transform.position;
        }

        SkillEffect();
    }

    private void SkillEffect()
    {
        if (GameManager.Instance.ac.angelHp > 0f)
        {
            GameManager.Instance.ac.TakeDamage(-fairyHealAmount);
        }
        if (GameManager.Instance.dc.demonicHp > 0f)
        {
            GameManager.Instance.dc.TakeDamage(-fairyHealAmount);
        }
        if (GameManager.Instance.fc.fairyHp > 0f)
        {
            GameManager.Instance.fc.TakeDamage(-fairyHealAmount);
        }
    }
}
