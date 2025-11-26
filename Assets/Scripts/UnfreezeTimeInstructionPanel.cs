using UnityEngine;

public class UnfreezeTimeInstructionPanel : MonoBehaviour
{
    public static UnfreezeTimeInstructionPanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}

