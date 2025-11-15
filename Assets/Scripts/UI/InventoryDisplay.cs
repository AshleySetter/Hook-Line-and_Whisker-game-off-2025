using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject inventoryFishCanvasImagePrefab;
    [SerializeField] private Transform inventoryDisplayContainer;

    private readonly List<GameObject> spawnedIcons = new();
    
    private void Start()
    {
        Inventory.Instance.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void RefreshUI()
    {
        // Clear previous displays
        foreach (var go in spawnedIcons)
            Destroy(go);
        spawnedIcons.Clear();

        // Create new ones
        foreach (var fish in Inventory.Instance.GetFish())
        {
            GameObject icon = Instantiate(inventoryFishCanvasImagePrefab, inventoryDisplayContainer);
            icon.GetComponent<Image>().sprite = fish.fishSprite;
            spawnedIcons.Add(icon);
        }
    }
}
