using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private List<Transform> fishPositionTransforms;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        if (fishPositionTransforms == null)
        {
            fishPositionTransforms = new List<Transform>();
        }
    }

    public void SetFishPositionTransforms(Transform[] fishPositionTransforms)
    {
        Debug.Log(fishPositionTransforms);
        this.fishPositionTransforms = fishPositionTransforms.ToList();
    }

    private void Update()
    {
        Transform activeTransform = null;
        foreach (var transform in fishPositionTransforms)
        {
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
