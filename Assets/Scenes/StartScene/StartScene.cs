using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

    // 버튼 클릭 시 호출할 메서드
    public void LoadSampleScene()
    {

        // SampleScene 이름에 맞춰 씬 전환
        SceneManager.LoadScene("SampleScene");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
