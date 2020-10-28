using UnityEngine;

public class MirrorPositionAndRotation : MonoBehaviour
{
    public Transform mirrorTransform;
    public Transform mirrorPoint;

    // Update is called once per frame
    void Update()
    {
        float distanceToMirror = mirrorPoint.position.x - mirrorTransform.position.x;
        transform.position = mirrorTransform.position + new Vector3(2 * distanceToMirror, 0, 0);
        transform.rotation = Quaternion.Euler(180,0,0) *  mirrorTransform.rotation;
    }
}
