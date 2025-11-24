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
            if (hook.GetHookState() == FishingHook.HookState.Fighting)
            {
                active = true;
            }
        }
        if (!catchBarWarning.activeSelf && active) {
            catchBarWarning.SetActive(active);
        }
        if (catchBarWarning.activeSelf && !active)
        {
            catchBarWarning.SetActive(active);
        }
    }
}
