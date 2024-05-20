using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spider : RecyclableMonster
{
    [SerializeField]
    MonsterData spiderData;
    //==================����=========================
    [SerializeField]
    string monName;
    [SerializeField]
    int hp;
    [SerializeField]
    int damage;
    [SerializeField]
    int defense;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float attackSpeed;
    [SerializeField]
    float attackMotionSpeed;

    const float cCtime = 0.8f;//�������� ���� �ð�

    bool istargetDetected = true;

    [SerializeField]
    GameObject DamageTextPreFab;//������ �ؽ�Ʈ ������
    [SerializeField]
    AudioStorage soundStorage;//��������
    [SerializeField]
    StageScData stageData;


    public float spiderAttackMovementSpeed = 5.0f;

    private void OnEnable()//Ȱ��ȭ �� �ʱ�ȭ
    {
        monName = spiderData.monsterName;
        hp = spiderData.hp + StageManager.instance.StageNum * spiderData.hp;
        damage = spiderData.damage + (int)(StageManager.instance.StageNum * spiderData.damage * 0.5f);
        defense = spiderData.defense + (int)(StageManager.instance.StageNum * spiderData.defense * 0.2f);
        moveSpeed = spiderData.moveSpeed;
        attackDistance = spiderData.attackDistance;
        attackSpeed = spiderData.attackSpeed;
        attackMotionSpeed = spiderData.attackMotionSpeed;
        coinValue = spiderData.coinValue + (int)(StageManager.instance.StageNum * spiderData.coinValue * 0.5f);
        if (MyRenderer != null) SetAlpa();
        if (Mycollider2D != null) Mycollider2D.enabled = true;
        Init();//�θ𿡼� �ʱ�ȭ
        StartCoroutine(AttackPlayer());//���� �Լ� ������Ʈ 0.2�� ����
    }

    void Start()
    {
        gameObject.tag = "monster";
        anim = GetComponent<Animator>();
        MyRenderer = gameObject.GetComponent<Renderer>();
        MyAudioSource = gameObject.GetComponent<AudioSource>();
        Mycollider2D = gameObject.GetComponent<CircleCollider2D>();
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50);
        collRange = 0.3f;

    }

    public override void OnMonDamaged(int PlayerDamage)//�÷��̾��� ���� �̺�Ʈ�� ���� �Լ�
    {
        
        hp = MonDamaged(hp, defense, PlayerDamage);
        GameObject damageText = Instantiate(DamageTextPreFab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        damageText.GetComponent<DamageText>().damage = MonDamagedTextCal(spiderData.defense, PlayerDamage);
        if (hp <= 0)
        {
            MyAudioSource.PlayOneShot(soundStorage.SoundSrc[1].SoundFile, 0.4f);
            Mycollider2D.enabled = false;
            isDead = true;
            MonDeath?.Invoke(this);//���λ���, �׾����� �ﰢ �̺�Ʈ
            StartCoroutine(DelayDeath());
        }
        else
        {
            MyAudioSource.PlayOneShot(soundStorage.SoundSrc[0].SoundFile, 0.4f);
            if (!isCCTime)//�������� ���� �ð��� ������
            {
                isDamaged = true;
                isCCTime = true;
                StartCoroutine(DelayDamaged(0.25f));
                Invoke("SetisCCtimeF", cCtime);
            }
        }
    }

    void SetisCCtimeF()
    {
        isCCTime = false;
    }

    IEnumerator DelayDeath()//ȸ�� �� �״� �ִϸ��̼� ��� �ð� Ȯ��
    {
        yield return new WaitForSeconds(1f);
        Destroyed?.Invoke(this);//���� ���� �̺�Ʈ
    }

    //===============���� ���¿� ���� �ִϸ����� �Ķ���� �� ����==============

    public override void AttackState()
    {
        base.AttackState();
        if(istargetDetected)
        {
            istargetDetected = false;
            ReDetected();
            StartCoroutine(ReDetectedDelay(spiderData.attackMotionSpeed));
        }
       
        anim.SetInteger("STATE", 2);
        transform.position = Vector2.Lerp(transform.position, Ppos + dir* 1.5f, Time.deltaTime * spiderAttackMovementSpeed);
        
         
    }
    Vector3 Ppos = Vector3.zero;//�Ͻ��� �÷��̾� ��ġ ����
    Vector3 dir = Vector3.zero;//�Ͻ��� �÷��̾� ���� ����
    void ReDetected()
    {
        Ppos = targetPosition.position;//��ġ ���
        dir = (targetPosition.position - transform.position).normalized;//���� ���
    }

    IEnumerator ReDetectedDelay(float time)//���� �� ������ �Լ�
    {
        yield return new WaitForSeconds(time);
        istargetDetected = true;
    }
    public override void IdleState()
    {
        base.IdleState();
        anim.SetInteger("STATE", 0);
    }
    public override void TraceState(Vector3 playerPos, float moveSpeed)
    {
        base.TraceState(playerPos, moveSpeed);
        anim.SetInteger("STATE", 1);
    }
    public override void DamagedState()
    {
        base.DamagedState();
        anim.SetInteger("STATE", 3);
        if (!isShake)
        {
            transform.DOShakePosition(0.3f, 0.1f, 90, 180, false, false);
            isShake = true;
            Invoke("DelayShake", 0.31f);
        }
    }
    void DelayShake() { isShake = false; }

    public override void DieState()
    {
        base.DieState();
        anim.SetInteger("STATE", 4);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float f = 1;
        while (f > 0)
        {
            f -= 0.1f;
            Color ColorAlhpa = MyRenderer.material.color;
            ColorAlhpa.a = f;
            MyRenderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.02f);
        }
    }

    void SetAlpa()
    {
        float f = 1;
        Color ColorAlhpa = MyRenderer.material.color;
        ColorAlhpa.a = f;
        MyRenderer.material.color = ColorAlhpa;
    }

    IEnumerator AttackPlayer()//�÷��̾� ���� �̺�Ʈ �߻� �Լ�
    {
        while (!isDead)
        {
            if (isAttacking)//���Ͱ� ���� ��
            {
                Collider2D recognitionPlayer = Physics2D.OverlapCircle(transform.position + Vector3.up * -0.1f, collRange, layermask, -100.0f, 100.0f);
                if (recognitionPlayer != null)
                {
                    PlayerAttack?.Invoke(damage);//����->�÷��̾� ���� �̺�Ʈ
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        
    }

    private void OnDrawGizmos()//���� �浹 ���� ������
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position +Vector3.up*-0.1f, collRange);

    }

    // Update is called once per frame
    void Update()
    {
        LookPlayer(targetPosition.position);
        MonsterState(targetPosition.position, spiderData.attackDistance, spiderData.attackSpeed, spiderData.attackMotionSpeed);
        UpdateState(targetPosition.position, spiderData.moveSpeed);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
}
