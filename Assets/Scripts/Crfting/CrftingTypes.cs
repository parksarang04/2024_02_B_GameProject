using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace MyGame.GuestSystem
{
    //아이템 품질 등급

    public enum ItemQuality
    {
        Poor,
        Common,
        Uncommon,
        Rate,
        Epic,
        Legendary
    }

    //제작 결과 상태
    public enum CrftingResult
    {
        Success,
        Failure,
        InsufficientMaterials,
        InvalidRecipe,
        LowSkillLevel
    }

    //기존 IItem 인터페이스 확장
    public interface ICraftable
    {
        ItemQuality Quality { get; }
        bool isStackable {  get; }
        int MaxStackSize {  get; }

    }
}

