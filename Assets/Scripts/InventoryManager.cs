using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//��� �������� �⺻ �������̽� interface Ŭ����
//�޼ҵ�, �̺�Ʈ, �ε���, ������Ƽ
//������ ������ Puvlic���� ����
//������ x
public interface IItem
{
    string Name { get; }
    int ID { get; }
    void Use();
}

//��ü���� ������ Ŭ���� (Weapon)
public class Weapon : IItem
{
    public string Name { get; private set; }
    public int ID { get; private set; }
    public int Damage { get; private set; }

    public Weapon(string name, int id, int damage)
    {
        Name = name;
        ID = id;
        Damage = damage;
    }
    public void Use()
    {
        UnityEngine.Debug.Log($"Using weapon {Name} with damage {Damage}");
    }
}

//��ü���� ������ Ŭ���� (HealthPotion)
public class HealthPotion : IItem
{
    public string Name { get; private set; }
    public int ID { get; private set; }
    public int HealthAmount { get; private set; }

    public HealthPotion(string name, int id, int healthAmount)
    {
        Name = name;
        ID = id;
        HealthAmount = healthAmount;
    }
    public void Use()
    {
        UnityEngine.Debug.Log($"Using weapon {Name} with damage {HealthAmount}");
    }
}

//���׸� �κ��丮 Ŭ����
public class Inventory<T> where T : IItem
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
        UnityEngine.Debug.Log($"Add{item.Name} to inventory");
    }

    public void UseItem(int index)
    {
        if(index >= 0 && index < items.Count)
        {
            items[index].Use();
        }
        else
        {
            UnityEngine.Debug.Log("Invalid item index");

        }
    }

    public void ListItems()
    {
        foreach(var item in items)
        {
            UnityEngine.Debug.Log($"Item : {item.Name},ID : {item.ID}");
        }
    }
}

//�κ��丮 Manager
public class InventoryManager : MonoBehaviour
{
    private Inventory<IItem> playerInventory;

    private void Start()
    {
        playerInventory = new Inventory<IItem>();

        playerInventory.AddItem(new Weapon("Sword", 1, 10));
        playerInventory.AddItem(new HealthPotion("Small Potion", 2, 20));
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerInventory.ListItems();        //�κ��丮 ���� ���
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInventory.UseItem(0);         //������ ���
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerInventory.AddItem(new Weapon("Sword",1,10));      //������ ����
        }   
    }


}