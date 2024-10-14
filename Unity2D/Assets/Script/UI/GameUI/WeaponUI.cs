using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Text[] weaponTexts = null;

    private void Awake()
    {
        foreach (var weaponLevel in weaponTexts)
        {
            weaponLevel.text = "0";
        }
    }

    public void ChangeWeaponLevel(int index, int level)
    {
        weaponTexts[index].text = $"{level + 1}";
    }
}
