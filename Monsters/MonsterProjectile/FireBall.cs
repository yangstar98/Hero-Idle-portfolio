using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : RecyclableMonster
{
    public GameObject player;
    float angle;
    int Damage = 0;
    bool isShoot = false;
    [SerializeField]
    float moveSpeed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnEnable()
    {
        StageManager.instance.StageClear += OnListenStageClear;
        player = GameObject.FindWithTag("Player");
        isShoot = false;
        isDead = true;

    }
    private void OnDisable()
    {
        StageManager.instance.StageClear -= OnListenStageClear;
        isShoot = false;
    }

    void OnListenStageClear()
    {
        FireBallDestroyed?.Invoke(this);//��Ȱ��ȭ �̺�Ʈ �߻�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FireBallDestroyed?.Invoke(this);
            player.GetComponent<PlayerDamaged>().OnPlayerDamaged(Damage);
        }
    }

    public void SetDamage(int Damage)
    {
        this.Damage = Damage;
    }

    private void OnBecameInvisible()//ȭ�� ������ ���� ��
    {
        isActivated = false;
        FireBallDestroyed?.Invoke(this);//��Ȱ��ȭ �̺�Ʈ �߻�
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShoot)//���� �ʾ�����
        {
            angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//���̿� ���� �÷��̾� �ٶ󺸱�
            isShoot = true;
        }
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);//���̾� �� ���ư���
    }
}
