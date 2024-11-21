using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace MyGame.GuestSystem
{
    public class KillQuestCondition : IQuestCondition
    {
        //óġ�ؾ� �� ���� ����
        private string enemyType;
        //óġ�ؾ� �� �� ���� ��
        private int reqiredKills;
        //������� óġ�� ���� ��
        private int currentKills;

        //óġ ����Ʈ ���� �ʱ�ȭ ������
        public KillQuestCondition(string enemyType, int reqiredKills)
        {
            this.enemyType = enemyType;
            this.reqiredKills = reqiredKills;
            this.currentKills = 0;
        }

        //��ǥ óġ ���� �޼� �ߴ��� Ȯ��
        public bool IsMet() => currentKills >= reqiredKills;   //��ǥ óġ ���� �缺 �ߴ��� Ȯ��
        public void Initialize() => currentKills = 0;  //óġ ���� 0���� �ʱ�ȭ
        public float GetProgress() => (float)currentKills / reqiredKills;  //���� óġ ���൵�� �ۼ�Ʈ�� ��ȯ

        public string GetDescription() => $"Defeat {reqiredKills} {enemyType} ({currentKills}/{reqiredKills})";    //����Ʈ ���� ������ ���ڿ��� ��ȯ

        public void EnemyKilled(string enemyType)   //�� óġ �� ȣ��Ǵ� �޼���
        {
            if(this.enemyType == enemyType)
            {
                currentKills++;
            
            }
        }
    }
}
