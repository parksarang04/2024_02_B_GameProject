using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace MyGame.GuestSystem
{
    //������ ǰ�� ���

    public enum ItemQuality
    {
        Poor,
        Common,
        Uncommon,
        Rate,
        Epic,
        Legendary
    }

    //���� ��� ����
    public enum CrftingResult
    {
        Success,
        Failure,
        InsufficientMaterials,
        InvalidRecipe,
        LowSkillLevel
    }

    //���� IItem �������̽� Ȯ��
    public interface ICraftable
    {
        ItemQuality Quality { get; }
        bool isStackable {  get; }
        int MaxStackSize {  get; }

    }
}

