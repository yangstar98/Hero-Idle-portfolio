using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecyclableMonster : MonoBehaviour
{
    protected enum STATE { IDEL, TRACE, ATTACK,DAMAGED, DIE }
    //트리거 0 : idle, 1 : walk, 2 : attack, 3 : hurt, 4 : dead, 5 : dragonfall
    //================선언=============================
    protected Transform targetPosition;//플레이어의 위치
    protected bool isDead = false;
    protected bool isActivated = false;
    protected bool isCanAttack = true;
    protected bool isAttacking = false;
    protected bool isDamaged = false;
    protected bool isTrace = true;
    protected bool isOnceDieState = false;
    protected bool isShake = false;
    protected bool isCCTime = false;
    protected float lastAttackTime = 0;
    protected float DamagedTime = 0.3f;
    protected float collRange = 1f;
    public int coinValue;
    protected STATE state { get; set; }
    protected LayerMask layermask;
    protected int playerLayer;

    protected AudioSource MyAudioSource;
    protected Animator anim;
    protected Renderer MyRenderer;
    protected Collider2D Mycollider2D;

    //================이벤트==========================
    public Action<RecyclableMonster> MonDeath;
    public Action<RecyclableMonster> Destroyed;
    public Action<RecyclableMonster> ClearDestroyed;
    public Action<RecyclableMonster> FireBallDestroyed;
    public Action<int> FireBallAttackCollPlayer;
    public Action<int> PlayerAttack;


    //================================================

    private void Awake()
    {
        targetPosition = GameObject.FindWithTag("PlayerFoot").transform;
    }

    protected void Init()
    {
        
        isDead = false;
        isActivated = false;
        isCanAttack = true;
        isAttacking = false;
        isDamaged = false;
        isTrace = true;
        isOnceDieState = false;
        isShake = false;
        isCCTime = false;
        lastAttackTime = 0;
        DamagedTime = 0.3f;
        targetPosition = GameObject.FindWithTag("PlayerFoot").transform;
        state = STATE.TRACE;
        playerLayer = LayerMask.NameToLayer("player");
        gameObject.tag = "monster";
        layermask = 1 << playerLayer;
    }
    

    public void OnStageClearMonDestroy()
    {
        ClearDestroyed?.Invoke(this);
    }

    //몬스터 생성 위치 설정
    public virtual void Activate(Vector3 spawnPos)
    {
        isActivated = true;//활성 플래그 참
        transform.position = spawnPos;//스폰 포인트로 위치 전환
    }

    //플레이어의 방향으로 y축 회전
    public virtual void LookPlayer(Vector3 playerPos)
    {

        if (playerPos.x >= transform.position.x)//플레이어가 몬스터보다 오른쪽에 있으면
        {
            if (transform.rotation.y == 180)//y값이 180일때 오른쪽을 보는 몬스터의 경우
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f, transform.rotation.z);
            else//y값이 0일때 오른쪽을 보는 몬스터의 경우
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else//플레이어가 몬스터보다 왼쪽에 있으면
        {
            if (transform.rotation.y == 180)
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            else
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f, transform.rotation.z);
        }



    }


    //몬스터 사정거리까지 이동 함수
    public virtual void MonsterState(Vector3 playerPos, float attackDistance, float attackSpeed, float motionSpeed)
    {
        
        float distance = Vector3.Distance(playerPos, transform.position);

        

        if (!isDead)//죽지 않았다면
        {
            if (!isDamaged)//공격을 받은 상태가 아니라면
            {
                if (!isAttacking)//공격중이 아니면
                {
                    if (distance <= attackDistance)//사정거리보다 가깝다면
                    {
                        if (Mathf.Abs(transform.position.x) < 8.5f && Mathf.Abs(transform.position.y) < 4.5f)//화면 밖에 있을 때
                        {
                            if (isCanAttack)//공격 가능 상태라면
                            {
                                state = STATE.ATTACK;//공격 상태
                                StartCoroutine(DelayAttack(attackSpeed));//공격속도 시간 후 공격가능
                                StartCoroutine(DelayAttackMotion(motionSpeed));//공격속도 시간 후 공격가능
                                isTrace = false;//공격후 움직임 제한
                                isAttacking = true;//공격중
                                isCanAttack = false;//이미 공격 중이므로 공격 불가
                            }
                            else//공격 가능 상태가 아니라면
                            {
                                state = STATE.IDEL;
                            }
                        }
                        else
                        {
                            state = STATE.TRACE;
                        }
                    }
                    else if (distance > attackDistance && isTrace)
                    {
                        state = STATE.TRACE;
                    }
                    else
                        state = STATE.IDEL;
                }
                else
                    state = STATE.ATTACK;
            }
            else
            {
                state = STATE.DAMAGED;
            }
        }
        else
        {

            state = STATE.DIE;
        }
    }


    public virtual void OnMonDamaged(int PlayerDamage) { }

    public virtual int MonDamaged(int MonHp,int MonDef,int PlayerDamage)//몬스터 피격 함수 계산 후 Hp 배출
    {
        
        return MonHp - (PlayerDamage >= MonDef ? PlayerDamage - MonDef : 0);
    }

    public virtual int MonDamagedTextCal(int MonDef, int PlayerDamage)//몬스터피격텍스트  함수 계산 후 배출
    {

        return PlayerDamage - MonDef;
    }

    public virtual IEnumerator DelayDamaged(float DamagedTime)
    {
        yield return new WaitForSeconds(DamagedTime);
        isDamaged = false;
    }

    IEnumerator DelayAttack(float attackSpeed)//공격 딜레이 코루틴
    {
        yield return new WaitForSeconds(attackSpeed);
        isTrace = true;
        isCanAttack = true;
        
    }

    IEnumerator DelayAttackMotion(float Motion)//공격 딜레이 코루틴
    {
        yield return new WaitForSeconds(Motion);
        
        isAttacking = false;
    }

    public virtual void UpdateState(Vector3 playerPos, float moveSpeed)
    {
        switch (state)
        {
            case STATE.IDEL:
                IdleState();
                break;
            case STATE.TRACE:
                TraceState(playerPos, moveSpeed);
                break;
            case STATE.ATTACK:
                AttackState();
                break;
            case STATE.DAMAGED:
                DamagedState();
                break;
            case STATE.DIE:
                DieState();
                break;
            default:
                break;
        }

    }

    public virtual void IdleState()
    {

    }
    public virtual void TraceState(Vector3 playerPos, float moveSpeed)
    {
        MonsterMovement(playerPos, moveSpeed);
    }
    public virtual void AttackState()
    {

    }
    public virtual void DamagedState()
    {

    }
    public virtual void DieState()
    {

    }

    public virtual void MonsterMovement(Vector3 playerPos, float moveSpeed)//몬스터 이동
    {
        Vector3 dir = (playerPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }



}