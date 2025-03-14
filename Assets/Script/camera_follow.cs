using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime=0.25f;
    public Transform target;
    private Vector3 nextPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
   private void LateUpdate ()
    {
        nextPosition=target.position + new Vector3 (
            offset.x, 
            offset.y,
            transform.position.z
        );
        transform.position =Vector3.SmoothDamp (
            transform.position,
            nextPosition,
            ref velocity,
            smoothTime
        );
    }
}
