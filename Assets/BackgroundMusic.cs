using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;  // 배경음악 Clip
    private AudioSource audioSource;   // AudioSource 컴포넌트
    public float volume = 0.5f;        // 배경음악의 볼륨 (0.0f - 1.0f)

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // 배경음악 설정
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;  // 반복 재생 설정
            audioSource.volume = volume;  // 배경음악 볼륨 설정
            audioSource.Play();       // 음악 재생 시작
        }
    }
}
