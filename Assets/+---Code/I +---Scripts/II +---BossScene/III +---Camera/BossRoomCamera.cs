using UnityEngine;
using Unity.Cinemachine;

public class WaypointCameraControl : MonoBehaviour
{
    public float fixedXRotation = -30f; // 원하는 고정 x 회전 각도 설정 (예: -30도)

    void LateUpdate()
    {
        // 현재 카메라의 회전을 가져와서 x 축만 고정한 회전으로 업데이트
        Vector3 rotation = transform.eulerAngles;
        rotation.x = fixedXRotation; // x 축을 고정된 각도로 설정
        transform.eulerAngles = rotation;
    }
}
