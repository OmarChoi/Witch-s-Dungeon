using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    const int nChoice = 3;
    const int weaponNum = (int)Define.Weapon.WeaponTypeCount + (int)Define.Projectile.ProjectileTypeCount;
    [SerializeField] GameObject[] selectOption = new GameObject[nChoice];
    private void Awake()
    {
        Managers.Resource.LoadResourcesInFolder<Sprite>("Icon/Weapons");
    }

    public void SpawnLevelUpUI()
    {
        bool[] alreaySet = new bool[weaponNum];
        foreach (var item in selectOption)
        {
            WeaponOptionUI selectOptionUI = item.GetComponent<WeaponOptionUI>();
            int randomWeaponIndex = 0;
            int currentWeaponLevel = 0;
            string weaponName = "";
            while (true)
            {
                randomWeaponIndex = Random.Range(0, weaponNum);
                if (alreaySet[randomWeaponIndex] == true)
                    continue;
                alreaySet[randomWeaponIndex] = true;
                weaponName = Define.GetWeaponName(randomWeaponIndex);
                currentWeaponLevel = Managers.Player.GetComponent<PlayerController>().GetWeaponLevel(randomWeaponIndex);
                if (currentWeaponLevel + 1 < Define.MaxWeaponLevel) 
                    break;
            }
            selectOptionUI.SetUI(weaponName, currentWeaponLevel + 1);
        }
    }
}
