using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.GuestSystem
{
    [System.Serializable]
    public class Recipe
    {
        public string recipeld;                          //������ ���� ID
        public IItem resultltem;                         //��� ������
        public int resultAmount;                         //���� ����
        public Dictionary<int, int> requiredMaterials;  //�ʿ� ���� <������ ID, ����
        public int requireLevel;                         //�䱸 ���� ����
        public float baseSuccessRate;                    //�⺻ ���� Ȯ��
        public float craftTime;                             // ���� �ð�

        public Recipe(string id, IItem result, int amount)
        {
            recipeld = id;
            resultltem = result;
            resultAmount = amount;
            requiredMaterials = new Dictionary<int, int>();
            requireLevel = 1;
            baseSuccessRate = 1;
            craftTime = 0;
        }

        public void AddRequirdMaterial(int itemld, int amount)
        {
            if(requiredMaterials.ContainsKey(itemld))
            {
                requiredMaterials[itemld] += amount;
            }
            else
            {
                requiredMaterials[itemld]=amount;
            }
        }
    }
}


