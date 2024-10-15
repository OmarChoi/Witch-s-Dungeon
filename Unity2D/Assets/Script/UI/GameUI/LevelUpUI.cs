using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    const int nChoice = 3;
    const int weaponNum = (int)Define.Weapon.WeaponTypeCount + (int)Define.Projectile.ProjectileTypeCount;
    bool[] isMaxLevel = new bool[weaponNum];
    int[] weaponLevel = new int[weaponNum];
    [SerializeField] GameObject[] selectOption = new GameObject[nChoice];
    private void Awake()
    {
        Managers.Resource.LoadResourcesInFolder<Sprite>("Icon/Weapons");
    }

    private void OnEnable()
    {
        if (Managers.Player == null) return;
        Managers.UI.SetEscapeEnable(false);
        SpawnUI();
    }

    public void SpawnUI()
    {
        int nMaxWeaponCount = 0;
        for (int i = 0; i < weaponNum; ++i)
        {
            weaponLevel[i] = Managers.Player.GetComponent<PlayerController>().GetWeaponLevel(i);
            if (weaponLevel[i] == Define.MaxWeaponLevel - 1)
            {
                isMaxLevel[i] = true;
                nMaxWeaponCount += 1;
            }
        }
        if(nMaxWeaponCount == weaponNum)
        {
            Managers.UI.SetEscapeEnable(true);
            Managers.UI.DeActivateUI();
            return;
        }
        int nCanChoice = weaponNum - nMaxWeaponCount;
        bool[] alreaySet = new bool[weaponNum];
        for (int i = 0; i < selectOption.Length; ++i) 
        {
            WeaponOptionUI selectOptionUI = selectOption[i].GetComponent<WeaponOptionUI>();
            if (nCanChoice < i + 1) 
            { 
                selectOptionUI.SetUIOff();
                continue;
            }
            int maxLevelCount = 0;
            int randomWeaponIndex = 0;
            string weaponName = "";
            while (true)
            {
                if(maxLevelCount == weaponNum) { break; }
                randomWeaponIndex = Random.Range(0, weaponNum);
                if (alreaySet[randomWeaponIndex] == true) continue;
                if (isMaxLevel[randomWeaponIndex] == true) continue;
                if (weaponLevel[randomWeaponIndex] + 1 < Define.MaxWeaponLevel)
                {
                    weaponName = Define.GetWeaponName(randomWeaponIndex);
                    alreaySet[randomWeaponIndex] = true;
                    break;
                }
            }
            selectOptionUI.SetUI(weaponName, weaponLevel[randomWeaponIndex] + 1);
        }
    }
}
