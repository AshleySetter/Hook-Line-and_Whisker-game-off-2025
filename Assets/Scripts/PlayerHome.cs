using UnityEngine;

public class PlayerHome : MonoBehaviour
{
    public static PlayerHome Instance { get; private set; }
    private bool withinInteractDistance;

    private void Awake()
    {
        Instance = this;
        withinInteractDistance = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            withinInteractDistance = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            withinInteractDistance = false;
        }
    }

    public bool GetWithinInteractDistance()
    {
        return withinInteractDistance;
    }
}
