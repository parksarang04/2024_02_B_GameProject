using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.GuestSystem
{
    public class ExperienceReward : IQuestReward    //����ġ ������ �����ϴ� Ŭ����
    {
        private int experienceAmount;       //������ ������ ����ġ��

        public ExperienceReward(int amount)
        {
            this.experienceAmount = amount;
    }

        public void Grant(GameObject player)
        {
            Debug.Log($"Granted {experienceAmount}experience");
        }
        public string GetDescription() => $"{experienceAmount}ExperiencePoints";        //���� ������ ���ڿ��� ��ȯ
    }

    
}

