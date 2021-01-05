using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class FollowingCamera : MonoBehaviour
{
    Transform player;
    Vector3 position;
    Transform _transform;
    [SerializeField] Vector3 offset = Vector3.zero;
    UniversalAdditionalCameraData cameraData;
    void Start()
    {
        cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        player = Player.Instance.transform;

        _transform = transform;
        position = _transform.position;
    }
    void Update()
    {
        _transform.position = player.position + offset;
    }
    int postProcessingRequests = 0;
    public void setPostProcessing(bool value)
    {
        postProcessingRequests += value ? 1 : -1;
        if (postProcessingRequests == 0 && !value) cameraData.renderPostProcessing = false;
        if (postProcessingRequests == 1 && value) cameraData.renderPostProcessing = true;
    }
}
