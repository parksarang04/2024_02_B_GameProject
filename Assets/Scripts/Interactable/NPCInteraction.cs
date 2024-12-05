using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt() => "´ëÈ­ÇÏ±â";
    public float GetInteractionDistance() => 2f;

    public bool CanInteract(GameObject player) => true;
    public void OnInteract(GameObject player)
    {
        FloatingTextManager.Instance.ShowFloatingText("¾È³çÇÏ¼¼¿ä ! ", transform.position);
    }
}