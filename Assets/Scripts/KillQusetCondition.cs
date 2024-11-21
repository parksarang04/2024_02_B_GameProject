using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace MyGame.GuestSystem
{
    public class KillQuestCondition : IQuestCondition
    {
        //처치해야 할 적의 유형
        private string enemyType;
        //처치해야 할 총 적의 수
        private int reqiredKills;
        //현재까지 처치한 적의 수
        private int currentKills;

        //처치 퀘스트 조건 초기화 생성자
        public KillQuestCondition(string enemyType, int reqiredKills)
        {
            this.enemyType = enemyType;
            this.reqiredKills = reqiredKills;
            this.currentKills = 0;
        }

        //목표 처치 수를 달성 했는지 확인
        public bool IsMet() => currentKills >= reqiredKills;   //목표 처치 수를 당성 했는지 확인
        public void Initialize() => currentKills = 0;  //처치 수를 0으로 초기화
        public float GetProgress() => (float)currentKills / reqiredKills;  //현재 처치 진행도를 퍼센트로 변환

        public string GetDescription() => $"Defeat {reqiredKills} {enemyType} ({currentKills}/{reqiredKills})";    //퀘스트 조건 설명을 문자열로 변환

        public void EnemyKilled(string enemyType)   //적 처치 싯 호출되는 메서드
        {
            if(this.enemyType == enemyType)
            {
                currentKills++;
            
            }
        }
    }
}
