using UnityEngine;

public class FishBucket : MonoBehaviour
{
    public static FishBucket Instance { get; private set; }

    private bool withinInteractDistance;

    private void Awake()
    {
        Instance = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("on trigger enter");
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Debug.Log("player within distance");
            withinInteractDistance = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("on trigger exit");
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Debug.Log("player without distance");
            withinInteractDistance = false;
        }
    }

    public bool GetWithinInteractDistance()
    {
        return withinInteractDistance;
    }

}
