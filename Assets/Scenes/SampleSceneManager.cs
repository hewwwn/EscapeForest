using UnityEngine;

public class SampleSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 마우스 커서 숨기기 및 잠금
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false; // 마우스 커서를 숨김
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
