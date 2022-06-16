using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControllerSuper : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float m_lerpRate = 0.5f;

    [SerializeField]
    private int top = 89;

    [SerializeField]
    private int bottom = -10;

    [SerializeField]
    private int left = 10;

    [SerializeField]
    private int right = 560;

    private Camera m_camera;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        var camPos = m_camera.transform.position;
        var pos = Vector2.Lerp(camPos, target.transform.position, m_lerpRate);
        
        m_camera.transform.position = new Vector3(
            Mathf.Clamp(pos.x, left, right), 
            Mathf.Clamp(pos.y, bottom, top), 
            camPos.z);
    }
}
