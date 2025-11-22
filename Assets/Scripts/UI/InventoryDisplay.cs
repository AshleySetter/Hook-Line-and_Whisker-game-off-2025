using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject inventoryFishCanvasImagePrefab;
    [SerializeField] private GameObject inventorySlotCanvasImagePrefab;
    [SerializeField] private Transform inventorySlotDisplayContainer;

    private readonly List<GameObject> spawnedIcons = new();
    private readonly List<GameObject> spawnedSlots = new();
    
    private void Start()
    {
        Inventory.Instance.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshSlots();
        RefreshFish();
    }

    private void RefreshFish()
    {
        // Clear previous displays
        foreach (var go in spawnedIcons)
            Destroy(go);
        spawnedIcons.Clear();

        // Create new ones
        int slotIndex = 0;
        foreach (var fish in Inventory.Instance.GetFish())
        {
            Transform slot = spawnedSlots[slotIndex].transform;
            GameObject icon = Instantiate(inventoryFishCanvasImagePrefab, slot);
            icon.GetComponent<Image>().sprite = fish.fishSprite;
            spawnedIcons.Add(icon);
            slotIndex++;
        }
    }

    private void RefreshSlots()
    {
        // Clear previous displays
        foreach (var go in spawnedSlots)
            Destroy(go);
        spawnedSlots.Clear();

        // Create new ones
        for(var i = 0; i < Inventory.Instance.GetCapacity() ; i++)
        {
            GameObject icon = Instantiate(inventorySlotCanvasImagePrefab, inventorySlotDisplayContainer);
            spawnedSlots.Add(icon);
        }
    }
}
