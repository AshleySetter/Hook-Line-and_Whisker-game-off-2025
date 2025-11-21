using System;
using UnityEngine;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    private GameObject activeWindow;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnEscapeAction += GameInput_OnEscapeAction;
    }

    public void SetActiveWindow(GameObject window)
    {
        activeWindow = window;
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
