/*using UnityEngine;
using System.Collections;

public class SmoothTranslate : MonoBehaviour 
{
    public Transform target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public float rotationDamping = 10.0f;

    void Update()
    {
        Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }

        else transform.LookAt(target, target.up);
    }
}*/

using UnityEngine;
using System.Collections;

public class SmoothTranslate : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float height;
    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Camera.main.orthographicSize = 0.77f + (this.transform.position.y - (-2.1f)) / 3f;
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y + height, target.position.z));
            Vector3 delta = new Vector3(target.position.x, target.position.y + height, target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;


            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
}

