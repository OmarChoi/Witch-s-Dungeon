using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponOptionUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image weaponImage;
    [SerializeField] Text weaponName;
    [SerializeField] Text currentLevel;
    [SerializeField] Text weaponDescripter;
    [SerializeField] GameObject LevelUpUI;

    private int weaponIndex;
    private int nextLevel = 0;

    public void SetUI(string name, int nextlv)
    {
        this.gameObject.SetActive(true);
        weaponIndex = Define.GetWeaponIndex(name);
        weaponImage.sprite = Managers.Resource.GetResource<Sprite>(name + "Icon");
        weaponName.text = name;
        nextLevel = nextlv;
        currentLevel.text = $"Level {nextlv + 1}";
        bool IsFirst = (nextLevel == 1) ? true : false;
        string descriptor = Managers.Data.GetWeaponDescriptor(name, IsFirst); ;
        descriptor = descriptor.Replace("/", "\n").Trim();
        weaponDescripter.text = descriptor;
    }

    public void SetUIOff()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Managers.Player.GetComponent<PlayerController>().SetWeaponLevel(weaponIndex, nextLevel);
            Managers.Scene.GameScene.ChangeWeaponLevel(weaponIndex, nextLevel);
            Managers.UI.SetEscapeEnable(true);
            Managers.UI.DeActivateUI();
            Time.timeScale = 1.0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = Vector3.one;
    }
}
