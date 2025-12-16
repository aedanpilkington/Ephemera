using System.Collections.Generic;
using UnityEngine;

public class CaterpillarFollow : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int baseSegmentCount = 5;
    public float segmentSpacing = 0.5f;
    public float followSpeed = 10f;

    private List<Transform> segments = new List<Transform>();

    private int extraSegments = 0;

    void Start()
    {
        Transform previous = transform;

        // spawn base body (never removed)
        for (int i = 0; i < baseSegmentCount; i++)
        {
            GameObject segment = Instantiate(segmentPrefab);
            segment.transform.position = previous.position - previous.forward * segmentSpacing;
            segment.transform.rotation = previous.rotation;

            segments.Add(segment.transform);
            previous = segment.transform;
        }
    }

    void Update()
    {
        Transform previous = transform;

        foreach (Transform segment in segments)
        {
            Vector3 targetPos = previous.position - previous.forward * segmentSpacing;

            segment.position = Vector3.Lerp(
                segment.position,
                targetPos,
                followSpeed * Time.deltaTime
            );

            segment.rotation = Quaternion.Lerp(
                segment.rotation,
                previous.rotation,
                followSpeed * Time.deltaTime
            );

            previous = segment;
        }
    }

    // called when apple is collected
    public void AddSegment()
    {
        Transform last = segments[segments.Count - 1];

        GameObject segment = Instantiate(segmentPrefab);
        segment.transform.position = last.position - last.forward * segmentSpacing;
        segment.transform.rotation = last.rotation;

        segments.Add(segment.transform);
        extraSegments++;
    }

    // remove ONLY apple-based segments
    public void RemoveSegments(int amount)
    {
        int toRemove = Mathf.Min(amount, extraSegments);

        for (int i = 0; i < toRemove; i++)
        {
            Transform last = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);
            Destroy(last.gameObject);
            extraSegments--;
        }
    }

    public int GetExtraSegmentCount()
    {
        return extraSegments;
    }
}
