using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    [SerializeField]
    List<Item_ScriptableObject> item_ScriptableObject = new List<Item_ScriptableObject>(); // item������ ����Ʈ
    [SerializeField]
    List<Sprite> backgrounds = new List<Sprite>();// ��޺� ������ ���� ��׶��� ��������Ʈ ����Ʈ
    [SerializeField]
    List<Sprite> slots = new List<Sprite>(); // ��޺� ������ ������ ���� ��������Ʈ ����Ʈ

   
    public Sprite defaultBackGround; // �󽽷� ��׶���
    public Sprite defaultSlot;

    public List<Sprite> defaultItemIcon = new List<Sprite>();

    public List<Item> items = new List<Item>(); // ������ ������ ��ü�� ���� ����Ʈ
    public List<Item> gainedItems = new List<Item>(); // �÷��̾ ���� ������ ��ü�� ���� ����Ʈ

    public Item[] equipments = new Item[6];

    public UnityEvent ChangeEqument;
    
    private void Awake()
    {
        if(instance == null)//�̱���
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void CreatItem()
    {  
        int eVaues = Enum.GetValues(typeof(Item.ItemGrade)).Length;

        List<Item> itemdatas = new List<Item>();

        for (int i=0; i < item_ScriptableObject.Count; i++)
        {
           
            for (int a = 0; a < eVaues; a++)
            {
                Item item = new Item()
                {
                    itemData = item_ScriptableObject[i],
                    itemGrade = (Item.ItemGrade)a,
                    ItemLv = 1,
                };

                item.Setting(backgrounds[a], slots[a]);

                itemdatas.Add(item);
            }
        }

        items = itemdatas.OrderBy(data => data.itemData.itemType)
                .ThenBy(data => data.itemData.itemPow)
                .ThenBy(data =>data.itemGrade)
                .ToList();

    }

    public void LoadItem(SaveData data)
    {
        for (int i = 0; i < data.items.Count; i++)
        {
            Item item = new Item();
            item = data.items[i].OutData();
            items.Add(item);
        }

        for (int i = 0; i < data.gained_Items.Count; i++)
        {
            Item gainitem = new Item();
            gainitem = data.gained_Items[i].OutData();
            gainedItems.Add(gainitem);
        }

        for (int i = 0; i < data.equiped_Item.Length; i++)
        {
           

            if(data.equiped_Item[i].statUPType != "")
            {
                equipments[i] = data.equiped_Item[i].OutData();
             
            }
            else
            {
                equipments[i] = null;
            }
        }
        
    }

    
    public void GetItem(int itemnum)//�÷��̾ �������� ������� �����ϴ� �Լ�
    {
        gainedItems.Add(items[itemnum]);// �������� gainedItems����Ʈ�� ����
        items.RemoveAt(itemnum); // items ����Ʈ���� ����
    }

    public void OnEquipItem(Item item)
    {
        equipments[(int)item.itemData.itemType] = item;
        ChangeEqument?.Invoke();
    }

    public int GetItemPow(int stat)
    {
        int status = 0;

      
        switch (stat)
        {
            case 0://HP ���, �Ź�
                status = equipments[2]?.Cal_LevelupPow(equipments[2].ItemLv) ?? 0
                    + equipments[4]?.Cal_LevelupPow(equipments[4].ItemLv) ?? 0; 
                break;
            case 1://����
                status = equipments[0]?.Cal_LevelupPow(equipments[0].ItemLv) ?? 0;
                break;
            case 2://����, ����
                status = equipments[1]?.Cal_LevelupPow(equipments[1].ItemLv) ?? 0
                    + equipments[3]?.Cal_LevelupPow(equipments[3].ItemLv) ?? 0;
                break;

            case 3://�Ǽ�����
                status = equipments[2]?.Cal_LevelupPow(equipments[2].ItemLv) ?? 0;
                break;
        }

        return status;
        
    }
   
}
