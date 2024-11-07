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
  // ������ ��� ����Ʈ�� �����ϴ� ��ųʸ�
    private Dictionary<string, Quest> allQuest = new Dictionary<string, Quest>(); //������ ��� ����Ʈ�� �����ϴ� ��ųʸ�
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>(); //���� ���� ���� ����Ʈ�� �����ϴ� ��ųʸ�
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>(); //�Ϸ�� ����Ʈ�� �����ϴ� ��ųʸ�

    public event Action<Quest> OnQuestStarted;      //����Ʈ ���� �� �߻��ϴ� �̺�Ʈ
    public event Action<Quest> OnQuestCompleted;    //����Ʈ �Ϸ� �� �߻��ϴ� �̺�Ʈ
    public event Action<Quest> OnQuestFailed;       //����Ʈ ���� �� �߻��ϴ� �̺�Ʈ


    public void Start()
    {
        InitiallzeQuests();
    }

    //�⺻ ����Ʈ�� �����ϰ� ����ϴ� �޼���
    private void InitiallzeQuests()
    {
        //�� ��� ����Ʈ ���� ����
        var retHuntQuest = new Quest(
            "Q001",
            "Rat Problem",
            "Clear the basement of rate",
            MyGame.GuestSystem.QuestType.Kill,
            1);
        retHuntQuest.AddCondition(new KillQuuestCondition("Rat", 5));
        retHuntQuest.AddReward(new ExperienceReward(100));
        retHuntQuest.AddReward(new ItemReward("Gold",50));

        //���� ���� ����Ʈ
        var herbQuest = new Quest(
            "Q002",
            "Herb Collection", 
            "Collect herbs for the healer",
            MyGame.GuestSystem.QuestType.Collection,
            1);
        herbQuest.AddCondition(new CollectionQuestCondition("Herb", 3));
        herbQuest.AddReward(new ExperienceReward(50));

        //����Ʈ �Ŵ����� ����Ʈ �߰�
        allQuest.Add(retHuntQuest.Id, retHuntQuest);
        allQuest.Add(herbQuest.Id, herbQuest);

        //�׽�Ʈ ���ؼ� �ٷ� ���� (StartQuest �Լ�)
        StartQuest("Q001");
        StartQuest("Q002");


    }
    public bool CanStartQuest(string questld)   //Ư�� ����Ʈ�� ������ �� �ִ��� �˻��ϴ� �޼���
    {
        if(!allQuest.TryGetValue(questld, out Quest quest)) return false;
        if(activeQuests.ContainsKey(questld)) return false;
        if(completedQuests.ContainsKey(questld)) return false;

        //���� ����Ʈ �Ϸ� ���� Ȯ��
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

    //����Ʈ�� �����ϴ� �޼���
    public void StartQuest(string questld)
    {
        if(!CanStartQuest(questld)) return;

        var quest = allQuest[questld];
        quest.Start();
        activeQuests.Add(questld, quest);
        OnQuestStarted?.Invoke(quest);
    }

    //����Ʈ ���� ��Ȳ�� ������Ʈ �ϴ� �޼���
    public void UpdateQuestProgress(string questld)
    {
        if (!activeQuests.TryGetValue(questld, out Quest quest)) return;

        if(quest.CheckCompletion())
        {
            CompleteQuest(questld);
        }
    }

    //����Ʈ�� �Ϸ� ó�� �ϴ� �޼���
    private void CompleteQuest(string questld)
    {
        if(!activeQuests.TryGetValue(questld,out Quest quest)) return;

        //�÷��̾� ã�� �����ص� ����Ʈ�� �Ϸ�
        var player = GameObject.FindGameObjectWithTag("Player");
        quest.Complete(player);     //player�� null�̿��� ������

        activeQuests.Remove(questld);
        completedQuests.Add(questld, quest);
        OnQuestCompleted?.Invoke(quest);

        Debug.Log($"Quest completed : {quest.Title}");
    }

    //���� ������ ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetAvailableQuests()
    {
        return allQuest.Values.Where(q => CanStartQuest(q.Id)).ToList();
    }

    //���� ���� ���� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetActiveQuest()
    {
        return activeQuests.Values.ToList();
    }
    //�Ϸ�� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetCompletedQuest()
    {
        return completedQuests.Values.ToList();
    }

    //�� óġ �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    public void OnEnemyKilled(string enemyType)
    {
        //Ȱ�� ����Ʈ ���纻�� ���� ���
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

    //���� �� ȣ�� �Ǵ� �̺�Ʈ ��鷯
    public void OnItemCollected(string itemid)
    {
        //Ȱ�� ����Ʈ ���纻�� ���� ���
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
