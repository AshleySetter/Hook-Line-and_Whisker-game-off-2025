using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform[] fishPositionTransforms;
    [SerializeField] private LineRenderer lineRenderer;

    private void Update()
    {
        Transform activeTransform = null;
        foreach (var transform in fishPositionTransforms)
        {
            Debug.Log(transform.gameObject.activeSelf);
            if (transform.gameObject.activeSelf)
            {
                activeTransform = transform;
            }
        }
        if (activeTransform != null)
        {
            Vector3 bobberPositionInLocalSpace = this.transform.InverseTransformPoint(activeTransform.position);
            lineRenderer.SetPosition(1, bobberPositionInLocalSpace + 0.2f * Vector3.down);
        }
    }
}
