using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Slider healthBar;

    public void SetBoss(BossEnemy boss)
    {
        healthBar.value = 1f;
    }

    public void UpdateHealth(float current, float max)
    {
        healthBar.value = current / max;
    }
}