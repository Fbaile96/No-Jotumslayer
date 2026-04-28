using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject levelUpPanel;
    public Transform buttonsContainer;
    public GameObject upgradeButtonPrefab;

    [Header("Armas")]
    public GameObject spinAura;
    public GameObject shurikenOrbit;
    public BombLauncher bombLauncher;

    // Mejoras de stats siempre disponibles
    private string[] statUpgrades = {
        "Daño +20%",
        "Velocidad +15%",
        "Vida máxima +25",
        "Cadencia de ataque +20%",
        "Rango de ataque +20%"
    };

    // Armas desbloqueables
    private Dictionary<string, bool> weaponUnlocks = new Dictionary<string, bool>
    {
        { "Desbloquear Aura", false },
        { "Desbloquear Shuriken", false },
        { "Desbloquear Bombas", false }
    };

    void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpMenu()
    {
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);

        foreach (Transform child in buttonsContainer)
            Destroy(child.gameObject);

        // Construye lista de opciones disponibles
        List<string> available = new List<string>();

        // Agrega armas no desbloqueadas
        foreach (var weapon in weaponUnlocks)
        {
            if (!weapon.Value)
                available.Add(weapon.Key);
        }

        // Agrega mejoras de stats
        available.AddRange(statUpgrades);

        // Mezcla la lista
        Shuffle(available);

        // Toma las primeras 3
        int count = Mathf.Min(3, available.Count);
        for (int i = 0; i < count; i++)
        {
            string option = available[i];
            GameObject btn = Instantiate(upgradeButtonPrefab, buttonsContainer);

            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, 80 - (i * 110));

            btn.GetComponentInChildren<TextMeshProUGUI>().text = option;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectUpgrade(option);
            });
        }
    }

    void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    void SelectUpgrade(string upgrade)
    {
        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        SwordWeapon sword = GameObject.FindWithTag("Player").GetComponentInChildren<SwordWeapon>();
        SpinAura aura = GameObject.FindWithTag("Player").GetComponentInChildren<SpinAura>(true);
        ShurikenOrbit shuriken = GameObject.FindWithTag("Player").GetComponentInChildren<ShurikenOrbit>(true);

        switch (upgrade)
        {
            // ── Desbloqueos de armas ──
            case "Desbloquear Aura":
                spinAura.SetActive(true);
                weaponUnlocks["Desbloquear Aura"] = true;
                break;

            case "Desbloquear Shuriken":
                shurikenOrbit.SetActive(true);
                weaponUnlocks["Desbloquear Shuriken"] = true;
                break;

            case "Desbloquear Bombas":
                bombLauncher.enabled = true;
                weaponUnlocks["Desbloquear Bombas"] = true;
                break;

            // ── Mejoras de stats ──
            case "Daño +20%":
                sword.damage *= 1.2f;
                if (aura != null && aura.gameObject.activeSelf) aura.damage *= 1.2f;
                if (shuriken != null && shuriken.gameObject.activeSelf) shuriken.damage *= 1.2f;
                if (bombLauncher != null && bombLauncher.enabled)
                {
                    AreaBomb bombTemplate = bombLauncher.bombPrefab.GetComponent<AreaBomb>();
                    if (bombTemplate != null) bombTemplate.damage *= 1.2f;
                }
                break;

            case "Velocidad +15%":
                stats.GetComponent<PlayerMovement>().speed *= 1.15f;
                break;

            case "Vida máxima +25":
                stats.maxHealth += 25f;
                stats.currentHealth += 25f;
                break;

            case "Cadencia de ataque +20%":
                sword.attackRate *= 1.2f;
                if (bombLauncher != null) bombLauncher.cooldown *= 0.8f;
                break;

            case "Rango de ataque +20%":
                sword.detectionRange *= 1.2f;
                if (aura != null) aura.transform.localScale *= 1.2f;
                break;
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}