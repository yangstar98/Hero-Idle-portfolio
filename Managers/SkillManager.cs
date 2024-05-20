using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private void Awake()
    {
        if (SkillManager.instance == null)
            SkillManager.instance = this;

        
    }


    [SerializeField]
    Skill_ScriptableObject earth;
    [SerializeField]
    Skill_ScriptableObject tornado;
    [SerializeField]
    Skill_ScriptableObject wind;
    [SerializeField]
    Skill_ScriptableObject meteor;
    [SerializeField]
    Skill_ScriptableObject buff;
    
    [SerializeField]
    GameObject EarthPrefab;

    [SerializeField]
    GameObject WindPrefab;

    [SerializeField]
    GameObject TornadoPrefab;

    [SerializeField]
    GameObject MeteorPrefab;

    [SerializeField]
    GameObject BigMeteorPrefab;

    [SerializeField]
    Transform playerTr;

    [SerializeField]
    Image skillimage;

    [SerializeField]
    Image skillCoolTimeGauge; // ��ų ��Ÿ�� ������ ǥ�� �̹���

    [SerializeField]
    Image skillCoolTimeGauge2; // ��ų ��Ÿ�� ������ ǥ�� �̹���

    SkillFactory skillfactory;
    SkillFactory earthFactory;
    SkillFactory windFactory;
    SkillFactory tornadoFactory;
    SkillFactory meteorFactory;
    SkillFactory BigmeteorFactory;
    //SkillFactory buffForthFactory;
    //SkillFactory buffBackFactory;
    public Transform earthPos;
    PlayerDamaged curHp;



    float tornadoSpeed = 5.0f;
    
    Vector2[] Tornadodir = { Vector2.up, Vector2.down, Vector2.right, Vector2.left, Vector2.down+Vector2.left,
                      Vector2.up+Vector2.right, Vector2.up+Vector2.left, Vector2.down+Vector2.right};

    Vector2[] Winddir = { Vector2.up + Vector2.right, Vector2.down+Vector2.left,
                      Vector2.up + Vector2.left, Vector2.down + Vector2.right }; // �� �밢�� �������� ��ų�� ������.
    float[] angles; // Wind�� ȸ������ �迭�� �Ҵ�


    public GameObject[] Slot = new GameObject[3];
    public Image[] cooltimeObject = new Image[3];
    public Button[] button = new Button[3];

    int[] skillSlot = new int[3];//��ų ���Կ� ������ ��ų ��ȣ�� ǥ��
    public Skill_ScriptableObject equipskills;
    public GameObject skillslot1;

    bool isEarthCoolTime = true;
    
    [SerializeField]
    GameObject[] slot0skillimages;
    [SerializeField]
    GameObject[] slot1skillimages;
  
    public GameObject[] slot2skillimages;
    [SerializeField]
    GameObject skillNull;
    [SerializeField]
    GameObject buffIcon;

    PlayerMoving playermove;
    public Button button1; // �ν����Ϳ� ��ų ��ư �ֱ� ���� ����
    public Button button2; // �ν����Ϳ� ��ų ��ư �ֱ� ���� ����
    public Button button3;
    Skill_Buff usedBuff;
    void Start()
    {
        earthFactory = new SkillFactory(EarthPrefab, 1); // Earth���� ���丮 ��������
        windFactory = new SkillFactory(WindPrefab, 4); // wind ���� ���丮 ��������
        tornadoFactory = new SkillFactory(TornadoPrefab, 8); // tornado ���� ���丮 ��������
        meteorFactory = new SkillFactory(MeteorPrefab, 1); 
        BigmeteorFactory = new SkillFactory(BigMeteorPrefab, 1); // meteor���� ���丮 ��������
        //buffForthFactory = new SkillFactory(BuffForthPrefab, 1);
        //buffBackFactory = new SkillFactory(BuffBackPrefab, 1);
        angleWind();

        
        skillCoolTimeGauge.fillAmount = 0f;
        skillCoolTimeGauge2.fillAmount = 0f;
        skillslot1 = GameObject.Find("skillslot1");

        //skillslot1.transform.GetChild(0).gameObject.SetActive(false);
        //skillslot1.transform.GetChild(1).gameObject.SetActive(true);
        //skillslot1.transform.GetChild(2).gameObject.SetActive(false);
        //skillslot1.transform.GetChild(3).gameObject.SetActive(false);

        

        //if (skillSlot[0] == 1)
        {
            //skillimage.sprite = earth.icon;
        }

        playermove = GameObject.FindWithTag("Player").GetComponent<PlayerMoving>();
        buffIcon.SetActive(false);

        curHp = GameObject.FindWithTag("Player").GetComponent<PlayerDamaged>();

        usedBuff = GameObject.Find("SkillManager").GetComponent<Skill_Buff>();

    }

    public int[] OutSlotnum()
    {
        return skillSlot;
    }
    
    public void ChangeSlot(int[] skillNum)
    {
        skillSlot = skillNum;
        OnClickchangedskill();

    }
    

    public void OnclickSkill(int slotNumber)
    {
        if(curHp.playerhp <=0) // �÷��̾��� HP�� 0�� �Ǿ��� �� ��ų�� ������� �ʽ��ϴ�.
        {
            return;
        }
        
        if (slotNumber == 0)
        {
           
            SkillUse(skillSlot[0]); // 0�� ����
            CoolTimeState(skillCoolTimeGauge, skillSlot[0], button[0]); // ��Ÿ���� ���ư��ϴ�(��Ÿ�� �ð��̹���, 0�� ��ų����, 0�� ��ų��ư)
        }
        else if (slotNumber == 1)
        {
            
            SkillUse(skillSlot[1]); // 1�� ����
            CoolTimeState(skillCoolTimeGauge2, skillSlot[1], button[1]);// ��Ÿ���� ���ư��ϴ�(��Ÿ�� �ð��̹���, 1�� ��ų����, 1�� ��ų��ư)
        }
        else if (slotNumber == 2)
        {
            
            SkillUse(skillSlot[2]); // 2�� ���� => ���� ��ų�� ������ �� �ִ� �����Դϴ�.
            
        }
    }

    void SkillUse(int skilNum)
    {
        switch (skilNum)
        {
            case 0:
                break;
            case 1:
                OnEarthAttack(); // Earth ��ų �ߵ� �Լ�
                break;
            case 2:
                OnTornadoAttack(); // Tornado ��ų �ߵ� �Լ�
                break;
            case 3:
                OnWindAttack(); // Wind ��ų �ߵ� �Լ�
                break;
            case 4:
                OnBigMeteorAttack(); // Meteor ��ų �ߵ� �Լ�
                break;
            case 5:
                usedBuff.ActivatedBuff(); // buff ��ų �ߵ� �Լ�
                break;
            default:
                break;



        }
    }

    public void CoolTimeState(Image skillcoolTime, int skillNum, Button button)
    {
       
        float skillCool=0;
        button.enabled = false; // ��ų �ߵ��Ǿ����� ��ư�� ��Ȱ��ȭ�Ͽ� ��Ÿ�����϶� ��ų �ߵ����� �ʰ� �Ѵ�.
        if (skillNum == 1) skillCool = earth.coolTime; // 1����ų Earth�� ��Ÿ�� ����
        else if (skillNum == 2) skillCool = tornado.coolTime; // 2����ų Tornado�� ��Ÿ�� ����
        else if (skillNum == 3) skillCool = wind.coolTime; // 3����ų wind�� ��Ÿ�� ����
        else if (skillNum == 4) skillCool = meteor.coolTime; // 4����ų meteor�� ��Ÿ�� ����
        else if (skillNum == 5) skillCool = buff.coolTime; // 5����ų buff�� ��Ÿ�� ����
        StartCoroutine(startSkillCoolTime(skillcoolTime, skillCool, button)); // ��Ÿ�� �̹����� fillAmount ȿ���� ���� �ڷ�ƾ
    }

    IEnumerator startSkillCoolTime(Image coolTimeimage, float coolTime, Button Getbutton)
    {   
        float subCoolTime = 1 / coolTime; // fillAmount '1'���� ������ ������ ��ų�� ��Ÿ�Ӱ� ��ŭ �����ش�.
        
        int i = 0; // while ���� ������ 
        coolTimeimage.fillAmount = 1; // fillAmount �� ����
        while(coolTimeimage.fillAmount>=0.001f)
        {

            coolTimeimage.fillAmount -= subCoolTime * Time.deltaTime;

            yield return new WaitForFixedUpdate();
            i++;
            if (i > 1000)
                break;
        }
        Getbutton.enabled = true; // ��Ÿ���� ������ �� ��ų�� �ٽ� ����ϱ� ���ؼ� ��ư Ȱ��ȭ

    }

    public void OnClickchangedskill()
    {
        for (int i = 0; i < slot0skillimages.Length; i++)
        {
            slot0skillimages[i].SetActive(false); // ��ų�� �������� �ʾ��� ���� �̹����� ǥ�õ��� �ʽ��ϴ�.
        }
        slot0skillimages[skillSlot[0]].SetActive(true); // ��ų�� �������� �� 0����ų ���Կ� ������ ��ų �̹����� Ȱ��ȭ �մϴ�.

        for (int i = 0; i < slot1skillimages.Length; i++)
        {
            slot1skillimages[i].SetActive(false); // ��ų�� �������� �ʾ��� ���� �̹����� ǥ�õ��� �ʽ��ϴ�.
        }
        slot1skillimages[skillSlot[1]].SetActive(true); // ��ų�� �������� �� 1����ų ���Կ� ������ ��ų �̹����� Ȱ��ȭ �մϴ�.

        for (int i = 0; i < slot2skillimages.Length; i++)
        {
            slot2skillimages[i].SetActive(false); // ��ų�� �������� �ʾ��� ���� �̹����� ǥ�õ��� �ʽ��ϴ�.
        }
        slot2skillimages[skillSlot[2]].SetActive(true); // ��ų�� �������� �� ������ų �̹����� Ȱ��ȭ �մϴ�.




        //skillNull.SetActive(false);
        //buffIcon.SetActive(true);



    }

    void angleWind() // �� �迭�� Rotation���� �Ҵ��� ���Ҵ�.
    {
        angles = new float[4];


        angles[0] = 30f;
        angles[1] = 150f;
        angles[2] = -150f;
        angles[3] = -30f;
    }


    public void OnEarthAttack()
    {
        GameObject earth = earthFactory.GetSkill();
        PlayerSound.instance.OnEarthSound();
        earth.transform.position = earthPos.position;
    }


    public void OnTornadoAttack()
    {
        
        foreach (Vector2 direction in Tornadodir)
        {
            GameObject tornado = tornadoFactory.GetSkill();
            PlayerSound.instance.OnTornadoSound();
            tornado.transform.position = playerTr.position;
            Rigidbody2D rb = tornado.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * tornadoSpeed;

        }

    }

    public void OnWindAttack()
    {
        for (int i = 0; i < Winddir.Length; i++)
        {
            GameObject wind = windFactory.GetSkill();
            PlayerSound.instance.OnWindSound();
            wind.transform.position = playerTr.position;
            wind.transform.rotation = Quaternion.Euler(0, 0, angles[i]);
        }
    }

    public void OnBigMeteorAttack()
    {
        GameObject meteor = BigmeteorFactory.GetSkill();
        PlayerSound.instance.OnMeteorSound();
        meteor.transform.position = new Vector2(7, 7);
        meteor.transform.rotation = Quaternion.Euler(0, 0, -130.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(playermove.isButtonPressed==true && button1.enabled==true)
        {
            OnclickSkill(0); // �ڵ���� ��ư�� ������ �� 0����ų ��ư�� Ȱ��ȭ �ɶ����� ������ ��ų�� �ڵ����� �����
            
        }
        else if(playermove.isButtonPressed == true && button2.enabled == true)
        {
            OnclickSkill(1); // �ڵ���� ��ư�� ������ �� 1����ų ��ư�� Ȱ��ȭ �ɶ����� ������ ��ų�� �ڵ����� �����
        }
        if(playermove.isButtonPressed == true && button3.enabled == true)
        {
            OnclickSkill(2); // �ڵ���� ��ư�� ������ �� ��ư�� Ȱ��ȭ �ɶ����� ������ų�� �ڵ����� �����
        }


    }
}
