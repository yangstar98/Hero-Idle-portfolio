using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFactory
{
    //���� ��Ȱ�� ��ũ��Ʈ

    List<Gold_Coin> pool = new List<Gold_Coin>();//���� Ǯ ����Ʈ ����
    Gold_Coin coinPrefab;//���� ������
    int defaultCoinNumber;//ó�� pool�� ���� ���� ��

    //������ ���� ���� ����
    public CoinFactory(Gold_Coin coinPrefab, int defaultCoinNumber = 5)
    {
        this.coinPrefab = coinPrefab;//�ܺο��� ���� ������ ���� 
        this.defaultCoinNumber = defaultCoinNumber;//�ܺο��� ���� ���� �� ����
        Debug.Assert(this.coinPrefab != null, "���� ���丮�� ���� ������ ����");
    }


    //������Ʈ ����
    void CreatePool()
    {
        for (int i = 0; i < defaultCoinNumber; i++)
        {//��Ȱ������ ������ ���� ����
            Gold_Coin obj = Gold_Coin.Instantiate(coinPrefab) as Gold_Coin;
            obj.gameObject.SetActive(false);//���� �� �ٷ� ��Ȱ��ȭ
            pool.Add(obj);//pool ����Ʈ�� ����
        }
    }

    //���� �ҷ�����
    public Gold_Coin GetCoin()
    {
        if (pool.Count == 0) CreatePool();//pool�� �����ִ� ���Ͱ� ���ٸ� ���� ����
        int lastIndex = pool.Count - 1;// pool����Ʈ�� ������ �ε���
        Gold_Coin obj = pool[lastIndex];//pool����Ʈ ������ ���͸� obj�� ����
        pool.RemoveAt(lastIndex);//��Ȱ�� ������ ����Ʈ ������ ���� ����Ʈ���� ����(��� ������ ����Ʈ���� �����ϱ� ����)
        obj.gameObject.SetActive(true);//����� ���� ���� Ȱ��ȭ
        return obj;//���� 



    }

    //�ݳ��Լ� ��Ȱ���ϱ� ����
    public void CoinRestore(Gold_Coin obj)
    {
        Debug.Assert(obj != null, "�ƹ��͵� ���� ������Ʈ�� ��ȯ�Ǿ�� �մϴ�");
        obj.gameObject.SetActive(false);//���� ���� ��Ȱ��ȭ
        pool.Add(obj);//��Ȱ���ϱ� ���� �ٽ� pool�� �߰�
    }
}
