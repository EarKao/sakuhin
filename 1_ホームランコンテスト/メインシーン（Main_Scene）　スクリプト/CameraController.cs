using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�J�����R���g���[���[
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpRate;

    private Camera m_camera;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 camPos = m_camera.transform.position;
        var pos = Vector2.Lerp(camPos, target.position, lerpRate);
        m_camera.transform.position = new Vector3(pos.x, pos.y, camPos.z);// X��Y��Lerp����
    }
}
