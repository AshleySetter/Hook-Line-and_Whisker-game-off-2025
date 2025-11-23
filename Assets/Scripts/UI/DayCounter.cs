using TMPro;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayCount;

    void Update()
    {
        dayCount.text = GameManager.Instance.GetDay().ToString();
    }
}
