using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory
{//���� ��Ȱ�� ��ũ��Ʈ

    List<RecyclableMonster> pool = new List<RecyclableMonster>();//���� Ǯ ����Ʈ ����
    RecyclableMonster monsterPrefab;//���� ������
    int defaultMonsterNumber;//ó�� pool�� ���� ���� ��

    //������ ���� ���� ����
    public MonsterFactory(RecyclableMonster monsterPrefab, int defaultMonsterNumber = 5)
    {
        this.monsterPrefab = monsterPrefab;//�ܺο��� ���� ������ ���� 
        this.defaultMonsterNumber = defaultMonsterNumber;//�ܺο��� ���� ���� �� ����
        Debug.Assert(this.monsterPrefab != null, "���� ���丮�� ���� ������ ����");
    }


    //������Ʈ ����
    void CreatePool()
    {
        for (int i = 0; i < defaultMonsterNumber; i++)
        {//��Ȱ������ ������ ���� ����
            RecyclableMonster obj = GameObject.Instantiate(monsterPrefab) as RecyclableMonster;
            obj.gameObject.SetActive(false);//���� �� �ٷ� ��Ȱ��ȭ
            pool.Add(obj);//pool ����Ʈ�� ����
        }
    }

    //���� �ҷ�����
    public RecyclableMonster GetMonster()
    {
        if(pool.Count == 0) CreatePool();//pool�� �����ִ� ���Ͱ� ���ٸ� ���� ����
        int lastIndex = pool.Count - 1;// pool����Ʈ�� ������ �ε���
        RecyclableMonster obj = pool[lastIndex];//pool����Ʈ ������ ���͸� obj�� ����
        pool.RemoveAt(lastIndex);//��Ȱ�� ������ ����Ʈ ������ ���͸� ����Ʈ���� ����(��� ������ ����Ʈ���� �����ϱ� ����)
        obj.gameObject.SetActive(true);//����� ���� ���� Ȱ��ȭ
        return obj;//���� 
 

       
    }

    //�ݳ��Լ� ��Ȱ���ϱ� ����
    public void MonsterRestore(RecyclableMonster obj)
    {
        Debug.Assert(obj != null, "�ƹ��͵� ���� ������Ʈ�� ��ȯ�Ǿ�� �մϴ�");
        obj.gameObject.SetActive(false);//���� ���� ��Ȱ��ȭ
        pool.Add(obj);//��Ȱ���ϱ� ���� �ٽ� pool�� �߰�
    }
}
