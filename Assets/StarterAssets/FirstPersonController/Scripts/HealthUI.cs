using UnityEngine;
using TMPro; // TextMeshPro 사용

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // 체력 텍스트

    // 체력 값을 업데이트하는 메서드
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = $"HP {currentHealth} / {maxHealth}"; // "현재 체력 / 최대 체력" 형식
    }
}