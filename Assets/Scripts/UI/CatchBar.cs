using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class CatchBar : MonoBehaviour
{
    public static CatchBar Instance { get; private set; }

    [SerializeField] private Image CatchBarLeft;
    [SerializeField] private Image CatchBarRight;
    [SerializeField] private Image ReelMarker;
    [SerializeField] private float percentageDebug;

    private float maxReelMarkerMovement = 404;
    private float reelValue;
    private TweenInstance tweenInstance;
    private float reelPercentage;
    private float boostPercentage = 5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FloatTween reelTween = new FloatTween
        {
            duration = 1f,
            usePingPong = true,
            pingPongInterval = 0f,
            repeatInterval = 0f,
            isInfinite = true,
            // easeType = EaseType.QuadInOut,
            from = -1,
            to = 1,
            onUpdate = (_, value) => {
                reelValue = value;
                ReelMarker.transform.localPosition = new Vector3(
                    value * maxReelMarkerMovement,
                    ReelMarker.transform.localPosition.y,
                    ReelMarker.transform.localPosition.z);
            },
            // onUpdate = (instance, value) =>
            // {
            //     Debug.Log("Tween has updated");
            // },
            onEnd = (instance) =>
            {
                Debug.Log("Tween has ended");
            },
            onFinally = (instance) =>
            {
                Debug.Log("Tween has ended or has been cancelled");
            },
        };
        tweenInstance = ReelMarker.gameObject.AddTween(reelTween);
    }

    public void SetReelPercentage(float reelPercentage)
    {
        CatchBarLeft.fillAmount = (100 - reelPercentage) / 100 / 2;
        CatchBarRight.fillAmount = (100 - reelPercentage) / 100 / 2;
        this.reelPercentage = reelPercentage;
    }

    public bool GetCatchBarInGreen()
    {
        return Mathf.Abs(reelValue) * 100 < reelPercentage;
    }

    public bool GetCatchBarInBoost()
    {
        return Mathf.Abs(reelValue) * 100 < boostPercentage;
    }

    public void Pause()
    {
        tweenInstance.isPaused = true;
    }

    private void Update()
    {
        SetReelPercentage(percentageDebug);
    }
}
