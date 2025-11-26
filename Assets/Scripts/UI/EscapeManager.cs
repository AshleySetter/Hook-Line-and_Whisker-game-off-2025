using System;
using UnityEngine;
using UnityEngine.UI;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button settingsButton;
    private GameObject activeWindow;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnEscapeAction += GameInput_OnEscapeAction;
        settingsButton.onClick.AddListener(() =>
        {
            pauseMenu.SetActive(true);
            activeWindow = pauseMenu;
            Time.timeScale = 0f;
        });
    }

    public void SetActiveWindow(GameObject window)
    {
        activeWindow = window;
    }

    public GameObject GetActiveWindow()
    {
        return activeWindow;
    }

    private void GameInput_OnEscapeAction(object sender, EventArgs e)
    {
        if (activeWindow != null)
        {
            activeWindow.SetActive(false);
            activeWindow = null;
            Time.timeScale = 1f;
        } else
        {
            pauseMenu.SetActive(true);
            activeWindow = pauseMenu;
            Time.timeScale = 0f;
        }
    }
}
