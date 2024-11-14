using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//��� �������� �⺻ �������̽� interface Ŭ���� 
//�޼ҵ�,�̺�Ʈ,�ε���,������Ƽ 
//����� ������ Public���� ���� �ȴ�. 
//�����ΰ� ����. 


public interface IItem 
{
    string Name { get; }
    int ID { get; }
    void Use();
}

//CraftingMaterial Ŭ���� �߰�
public class CraftingMaterial : IItem
{
    public string Name { get; private set; }
    public int ID { get; private set; }

    public CraftingMaterial(string name, int id)
    {
        Name = name;
        ID = id;
    }

    public void Use()
    {
        Debug.Log($"This is a crafting material : {Name}");
    }

    //��ü���� ������ Ŭ���� (Weapon)
    public class Weapon : IItem
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public int Damage { get; private set; }

        public Weapon(string name, int id, int damage)  //������
        {
            Name = name;
            ID = id;
            Damage = damage;
        }
        public void Use()
        {
            Debug.Log($"Using weapon {Name} with damage {Damage}");
        }
    }

    //��ü���� ������ Ŭ���� (HealthPotion)
    public class HealthPotion : IItem
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public int HealAmount { get; private set; }

        public HealthPotion(string name, int id, int healAmount)  //������
        {
            Name = name;
            ID = id;
            HealAmount = healAmount;
        }
        public void Use()
        {
            Debug.Log($"Using weapon {Name} with damage {HealAmount}");
        }
    }

    //���׸� �κ��丮 Ŭ���� 
    public class Inventory<T> where T : IItem
    {
        private List<T> items = new List<T>();

        public void AddItem(T item)
        {
            items.Add(item);
            Debug.Log($"Add {item.Name} to inventory");
        }

        public void UseItem(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                items[index].Use();
            }
            else
            {
                Debug.Log("Invalid item index");
            }
        }
        public void ListItems()
        {
            foreach (var item in items)
            {
                Debug.Log($"Item: {item.Name} , ID : {item.ID}");
            }
        }

        public void RemoveItem(int itemld, int amount)
        {
            int remoned = 0;
            for(int i = items.Count - 1; i >= 0; i--)       //�迭���� ������ ���� �� �Ųٷ� ����� �Ѵ�
            {
                if (items[i].ID == itemld)
                {
                    items.RemoveAt(i);
                    remoned++;
                    if(remoned >= amount)
                        break;
                }
            }
        }

        public bool HasEnough(int itemld, int amount)       //�������� ������� �˻�
        {
            return GetItemCount(itemld) >= amount; ;
        }

        public int GetItemCount(int itemld)                 //������ ī��Ʈ �Լ�
        {                   
            return items.Count(item => item.ID == itemld);
        }
    }

    //�κ��丮 Manager 
    public class InventoryManager : MonoBehaviour
    {
        private Inventory<IItem> playerInventory = new Inventory<IItem>();
        public int UseBagIndex;

        void Start()
        {
            playerInventory = new Inventory<IItem>();

            //������ �߰� 
            playerInventory.AddItem(new Weapon("Sword", 1, 10));
            playerInventory.AddItem(new HealthPotion("Small Potion", 2, 20));

            playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));   //ID 101 : ö �ֱ�
            playerInventory.AddItem(new CraftingMaterial("Iron Ingot", 101));   //ID 101 : ö �ֱ�
            playerInventory.AddItem(new CraftingMaterial("Wood", 102));         //ID 102 : ����

            playerInventory.AddItem(new CraftingMaterial("Herb", 102));         //ID 201 : ����
            playerInventory.AddItem(new CraftingMaterial("Herb", 102));         //ID 201 : ����
            playerInventory.AddItem(new CraftingMaterial("Water", 202));        //ID 202 : ��
        }

        //�κ��丮 ������ �޼��� �߰�
        public Inventory<IItem> GetInvertory()
        {
            return playerInventory;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerInventory.ListItems();            //�����丮 ���� ���
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerInventory.UseItem(UseBagIndex);             //������ ���
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerInventory.AddItem(new Weapon("Sword", 1, 10));        //������ ���� 
            }
        }
    }
}
