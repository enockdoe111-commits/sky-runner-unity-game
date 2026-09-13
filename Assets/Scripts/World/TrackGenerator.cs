// TrackGenerator.cs - Continuous track generation for endless gameplay

using UnityEngine;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private GameObject trackSegmentPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int segmentsAhead = 10;
    [SerializeField] private int segmentsBehind = 5;

    private List<GameObject> activeSegments = new List<GameObject>();
    private float segmentLength = GameConstants.TRACK_SEGMENT_LENGTH;
    private float lastSegmentZ = 0f;
    private ObjectPool segmentPool;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        segmentPool = GetComponent<ObjectPool>();
        if (segmentPool == null)
            segmentPool = gameObject.AddComponent<ObjectPool>();

        segmentPool.Initialize(trackSegmentPrefab, segmentsAhead + segmentsBehind);

        // Generate initial segments
        for (int i = 0; i < segmentsAhead; i++)
        {
            GenerateSegment();
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Generate new segments ahead
        while (lastSegmentZ < playerTransform.position.z + (segmentsAhead * segmentLength))
        {
            GenerateSegment();
        }

        // Destroy segments behind player
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i].transform.position.z < playerTransform.position.z - (segmentsBehind * segmentLength))
            {
                segmentPool.ReturnObject(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }

    private void GenerateSegment()
    {
        GameObject segment = segmentPool.GetObject();
        segment.transform.position = new Vector3(0, 0, lastSegmentZ + segmentLength);
        segment.SetActive(true);
        activeSegments.Add(segment);
        lastSegmentZ += segmentLength;
    }
}
