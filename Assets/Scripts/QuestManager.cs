using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.QuestSystem;
using System.Linq;
using System;
using MyGame.GuestSystem;
using System.Reflection;

public class QuestManager : Singleton<QuestManager>
{
  // 게임의 모든 퀘스트를 저장하는 딕셔너리
    private Dictionary<string, Quest> allQuest = new Dictionary<string, Quest>(); //게임의 모든 퀘스트를 저장하는 딕셔너리
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>(); //현재 진행 중인 퀘스트를 저장하는 딕셔너리
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>(); //완료된 퀘스트를 저장하는 딕셔너리

    public event Action<Quest> OnQuestStarted;      //퀘스트 시작 시 발생하는 이벤트
    public event Action<Quest> OnQuestCompleted;    //퀘스트 완료 시 발생하는 이벤트
    public event Action<Quest> OnQuestFailed;       //퀘스트 실패 시 발생하는 이벤트


    public void Start()
    {
        InitiallzeQuests();
    }

    //기본 퀘스트를 생성하고 등록하는 메서드
    private void InitiallzeQuests()
    {
        //쥐 사냥 퀘스트 생성 예시
        var retHuntQuest = new Quest(
            "Q001",
            "Rat Problem",
            "Clear the basement of rate",
            MyGame.GuestSystem.QuestType.Kill,
            1);
        retHuntQuest.AddCondition(new KillQuuestCondition("Rat", 5));
        retHuntQuest.AddReward(new ExperienceReward(100));
        retHuntQuest.AddReward(new ItemReward("Gold",50));

        //약초 수집 퀘스트
        var herbQuest = new Quest(
            "Q002",
            "Herb Collection", 
            "Collect herbs for the healer",
            MyGame.GuestSystem.QuestType.Collection,
            1);
        herbQuest.AddCondition(new CollectionQuestCondition("Herb", 3));
        herbQuest.AddReward(new ExperienceReward(50));

        //퀘스트 매니저에 퀘스트 추가
        allQuest.Add(retHuntQuest.Id, retHuntQuest);
        allQuest.Add(herbQuest.Id, herbQuest);

        //테스트 위해서 바로 시작 (StartQuest 함수)
        StartQuest("Q001");
        StartQuest("Q002");


    }
    public bool CanStartQuest(string questld)   //특정 퀘스트를 시작할 수 있는지 검사하는 메서드
    {
        if(!allQuest.TryGetValue(questld, out Quest quest)) return false;
        if(activeQuests.ContainsKey(questld)) return false;
        if(completedQuests.ContainsKey(questld)) return false;

        //선행 퀘스트 완료 여부 확인
        foreach(var perrequisiteld in quest.GetType().GetField("prerequisteQuestlds")?.GetValue(quest)as List<string>??new List<string>())
        {
            if(!completedQuests.ContainsKey(perrequisiteld)) return false;
        }
        return true;

        //Type questType = quest.GetType();
        //FieldInfo perrequisiteldsField = questType.GetField("prerequisiteQuestlds");
        //object perrequisiteldsValue = perrequisiteldsField?.GetValue(quest);
        //List<string> perrequisite
    }

    //퀘스트를 시작하는 메서드
    public void StartQuest(string questld)
    {
        if(!CanStartQuest(questld)) return;

        var quest = allQuest[questld];
        quest.Start();
        activeQuests.Add(questld, quest);
        OnQuestStarted?.Invoke(quest);
    }

    //퀘스트 진행 상황을 업데이트 하는 메서드
    public void UpdateQuestProgress(string questld)
    {
        if (!activeQuests.TryGetValue(questld, out Quest quest)) return;

        if(quest.CheckCompletion())
        {
            CompleteQuest(questld);
        }
    }

    //퀘스트를 완료 처리 하는 메서드
    private void CompleteQuest(string questld)
    {
        if(!activeQuests.TryGetValue(questld,out Quest quest)) return;

        //플레이어 찾기 실패해도 퀘스트는 완료
        var player = GameObject.FindGameObjectWithTag("Player");
        quest.Complete(player);     //player가 null이여도 진행함

        activeQuests.Remove(questld);
        completedQuests.Add(questld, quest);
        OnQuestCompleted?.Invoke(quest);

        Debug.Log($"Quest completed : {quest.Title}");
    }

    //시작 가능한 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetAvailableQuests()
    {
        return allQuest.Values.Where(q => CanStartQuest(q.Id)).ToList();
    }

    //현재 진행 중인 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetActiveQuest()
    {
        return activeQuests.Values.ToList();
    }
    //완료된 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetCompletedQuest()
    {
        return completedQuests.Values.ToList();
    }

    //적 처치 시 호출되는 이벤트 핸들러
    public void OnEnemyKilled(string enemyType)
    {
        //활성 퀘스트 복사본을 만들어서 사용
        var activeQuestsList = activeQuests.Values.ToList();

        foreach(var quest in activeQuestsList)
        {
            foreach(var condition in quest.GetConditions())
            {
                if(condition is KillQuuestCondition killCondition)
                {
                    killCondition.EnemyKilled(enemyType);
                    UpdateQuestProgress(quest.Id);
                }
            }
        }
    }

    //수집 시 호출 되는 이벤트 헨들러
    public void OnItemCollected(string itemid)
    {
        //활성 퀘스트 복사본을 만들어서 사용
        var activeQuestsList = activeQuests.Values.ToList();

        foreach (var quest in activeQuestsList)
        {
            foreach (var condition in quest.GetConditions())
            {
                if (condition is CollectionQuestCondition collectionCondition)
                {
                    collectionCondition.ItemCollected(itemid);
                    UpdateQuestProgress(quest.Id);
                }
            }
        }
    }
}
