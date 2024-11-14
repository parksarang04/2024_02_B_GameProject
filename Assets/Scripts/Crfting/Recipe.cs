using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.GuestSystem
{
    [System.Serializable]
    public class Recipe
    {
        public string recipeld;                          //레시피 고유 ID
        public IItem resultltem;                         //결과 아이템
        public int resultAmount;                         //제작 개수
        public Dictionary<int, int> requiredMaterials;  //필요 제작 <아이템 ID, 수량
        public int requireLevel;                         //요구 제작 레벨
        public float baseSuccessRate;                    //기본 성공 확률
        public float craftTime;                             // 제작 시간

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


