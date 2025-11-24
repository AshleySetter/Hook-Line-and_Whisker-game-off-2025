using UnityEngine;

public class CatchBarWarning : MonoBehaviour
{
    [SerializeField] private GameObject catchBarWarning;
    private bool active;

    private void Update()
    {
        active = false;
        foreach (var hook in FishingHook.AllHooks)
        {
            Debug.Log(hook.GetHookState());
            if (hook.GetHookState() == FishingHook.HookState.Fighting)
            {
                active = true;
            }
        }
        if (!catchBarWarning.activeSelf) {
            catchBarWarning.SetActive(active);
        }
    }
}
