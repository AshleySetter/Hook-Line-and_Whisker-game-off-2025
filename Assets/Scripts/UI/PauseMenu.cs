using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            EscapeManager.Instance.SetActiveWindow(null);
            Time.timeScale = 1f; 
        });
    }
}
