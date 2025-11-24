using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject shopUI;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            shopUI.SetActive(false);
            EscapeManager.Instance.SetActiveWindow(null);
        });
    }
}
