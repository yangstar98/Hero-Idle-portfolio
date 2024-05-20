using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;//���� �Ŵ��� �̱��� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    List<RecyclableMonster> monsterStore = new List<RecyclableMonster>();
    public Transform[] stageSpawnPoint;//�������� ���� ����Ʈ

    public Action<RecyclableMonster> something;
    //===============���� ������ ����===================
    [SerializeField]
    BabyDragon babyDragonPrefab;
    [SerializeField]
    Slime slimePrefab;
    [SerializeField]
    Bat batPrefab;
    [SerializeField]
    Spider spiderPrefab;
    [SerializeField]
    Gold_Coin coinPrefab;

    //===============�÷��̾�===================

    PlayerDamaged playerDamaged;
    public bool PlayerDeath = false;
    //===============���丮 ����=======================
    MonsterFactory babyDragonFactory; 
    MonsterFactory slimeFactory; 
    MonsterFactory batFactory; 
    MonsterFactory spiderFactory;
    CoinFactory coinFactory;

    public int allSpawnNum = 0;

    public int RestoreNum = 0;

    void Start()
    {
        babyDragonFactory = new MonsterFactory(babyDragonPrefab,5);//���� ���丮�� �巡�� �ν��Ͻ� ����
        slimeFactory = new MonsterFactory(slimePrefab, 5);//���� ���丮�� ������ �ν��Ͻ� ����
        batFactory = new MonsterFactory(batPrefab, 5);//���� ���丮�� �� �ν��Ͻ� ����
        spiderFactory = new MonsterFactory(spiderPrefab, 5);//���� ���丮�� �����̴� �ν��Ͻ� ����

        coinFactory = new CoinFactory(coinPrefab, 5);// ���� ���丮�� ���� �ν��Ͻ� ����

        StageManager.instance.StartWave += OnStartSpawn;//���̺� ���۰� �������� �̺�Ʈ ����

        playerDamaged = GameObject.FindWithTag("Player").GetComponent<PlayerDamaged>();
    }
    
    void OnMonsterDestroyed(RecyclableMonster usedMonster)
    {
        
        usedMonster.Destroyed -= OnMonsterDestroyed;
        usedMonster.ClearDestroyed -= OnMonsterDestroyed;
        usedMonster.PlayerAttack -= playerDamaged.OnPlayerDamaged;
        int monsterIndex = monsterStore.IndexOf(usedMonster);//����Ʈ�� �ε��� �� ����
        monsterStore.RemoveAt(monsterIndex);
        //monsterFactory.MonsterRestore(usedMonster);
        if (usedMonster.name == "slime(Clone)")
        {
            slimeFactory.MonsterRestore(usedMonster);
        }
        else if (usedMonster.name == "bat(Clone)") batFactory.MonsterRestore(usedMonster);
        else if (usedMonster.name == "spider(Clone)") spiderFactory.MonsterRestore(usedMonster);
        else if (usedMonster.name == "baby_dragon(Clone)") babyDragonFactory.MonsterRestore(usedMonster);
        RestoreNum++;
        if (RestoreNum == allSpawnNum)
        {

            StageManager.instance.OnStageWin();
        }
    }
    //void OnSlimeDestroyed(RecyclableMonster usedBabyDragon)
    //{
    //    usedBabyDragon.Destroyed -= OnSlimeDestroyed;
    //}
    //void OnBatDestroyed(RecyclableMonster usedBabyDragon)
    //{
    //    usedBabyDragon.Destroyed -= OnBatDestroyed;
    //}
    //void OnSpiderDestroyed(RecyclableMonster usedBabyDragon)
    //{
    //    usedBabyDragon.Destroyed -= OnSpiderDestroyed;
    //}

    public void OnStartSpawn(StageScData stageData, int stageNum)
    {
        allSpawnNum = stageData.Stages[stageNum].slimeNum + stageData.Stages[stageNum].batNum + stageData.Stages[stageNum].spiderNum + stageData.Stages[stageNum].babyDragonNum;
        if (stageData.Stages[stageNum].slimeNum > 0)
        {
            StartCoroutine(SpawnDelay(stageData.Stages[stageNum].slimeNum, slimeFactory));
        }
        if (stageData.Stages[stageNum].batNum > 0)
        {
            StartCoroutine(SpawnDelay(stageData.Stages[stageNum].batNum, batFactory));
        }
        if (stageData.Stages[stageNum].spiderNum > 0)
        {
            StartCoroutine(SpawnDelay(stageData.Stages[stageNum].spiderNum, spiderFactory));
        }
        if (stageData.Stages[stageNum].babyDragonNum > 0)
        {
            StartCoroutine(SpawnDelay(stageData.Stages[stageNum].babyDragonNum, babyDragonFactory));
        }




    }

    IEnumerator SpawnDelay(int MaxMonsterTypeNum, MonsterFactory monsterFactory)
    {
        int spawnNum = MaxMonsterTypeNum;
        int A = 0;
       
        while (spawnNum > 0)
        {
            int randomInt = UnityEngine.Random.Range(0, 8);
     
            yield return new WaitForSeconds(60f/(float)MaxMonsterTypeNum);//��ȯ�� ������ �� �п� 30 �� ���� ��ȯ/60->1�ʿ� �Ѹ��� 20->3�ʾ� �Ѹ���
        
            
            RecyclableMonster monster = monsterFactory.GetMonster();//���� Ȱ��ȭ
            spawnNum--;
            monster.Activate(stageSpawnPoint[randomInt].position);//���� ��ġ ����
            monster.Destroyed += OnMonsterDestroyed;
            monster.ClearDestroyed += OnMonsterDestroyed;
            monster.PlayerAttack += playerDamaged.OnPlayerDamaged;
            monster.MonDeath += OnSpawnCoin;
            monsterStore.Add(monster);//���� �����ϱ� ���� ��Ƶδ� ����Ʈ


            A++;
            if(MaxMonsterTypeNum == A)
                Debug.Log($"{monster.name} {A} ���� ��� ��ȯ��");
            if (A > 1000)
                break;
        }

    }

    public void OnSpawnCoin(RecyclableMonster UsedMonster)
    {
        UsedMonster.MonDeath -= OnSpawnCoin;
        Gold_Coin coin = coinFactory.GetCoin();
        coin.SetTransform(UsedMonster.transform);
        coin.CoinDrop();
        StartCoroutine(DelayRestoreCoin(coin, UsedMonster));
    }

    IEnumerator DelayRestoreCoin(Gold_Coin UsedCoin, RecyclableMonster UsedMonster)
    {
        float randIndex = UnityEngine.Random.Range(3.5f, 3.7f);
        yield return new WaitForSeconds(randIndex);
        coinFactory.CoinRestore(UsedCoin);
        Resource.instance.GetResource(UsedMonster.coinValue, 0);

    }

    public void OnDestroyAllMonster()
    {
        //foreach (var monster in monsterStore)
        //{
        //    monster.OnStageClearMonDestroy();
        //}
        StopAllCoroutines();
        for (int i = monsterStore.Count -1; i >= 0; i--)
        {
            monsterStore[i].OnStageClearMonDestroy();
        }
    }

    public void OnDamagedAllMonster(int Damage)
    {
        //foreach (var monster in monsterStore)
        //{
        //    monster.OnMonDamaged(3);
        //}
        for (int i = monsterStore.Count - 1; i >= 0; i--)
        {
            monsterStore[i].OnMonDamaged(Damage);
        }
    }

    void Update()
    {


        //=======================�׽�Ʈ�� ��ȯ======================================
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    RecyclableMonster babyDragon = babyDragonFactory.GetMonster();
        //    babyDragon.Activate(spawnPoint[0].position);
        //    babyDragon.Destroyed += OnBabyDragonDestroyed;
        //    //GameManager.instance.playerAttack.monAttack += babyDragon.MonDamaged;//���� �ǰ� �̺�Ʈ ����

        //}
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    RecyclableMonster slime = slimeFactory.GetMonster();
        //    slime.Activate(spawnPoint[1].position);
        //    slime.Destroyed += OnSlimeDestroyed;

        //}
        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    RecyclableMonster bat = batFactory.GetMonster();
        //    bat.Activate(spawnPoint[2].position);
        //    bat.Destroyed += OnBatDestroyed;

        //}
        //if (Input.GetKeyDown(KeyCode.RightControl))
        //{
        //    RecyclableMonster spider = spiderFactory.GetMonster();
        //    spider.Activate(spawnPoint[3].position);
        //    spider.Destroyed += OnSpiderDestroyed;

           

        //}

    }
}
