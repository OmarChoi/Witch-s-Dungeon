using Unity.VisualScripting;
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

    private Sprite[] weaponIcons;
    private int weaponIndex;
    private int nextLevel = 0;

    public void SetUI(string name, int level)
    {
        weaponIndex = Define.GetWeaponIndex(name);
        Sprite icon = Managers.Resource.GetResource<Sprite>(name + "Icon");
        weaponImage.sprite = icon;
        weaponName.text = name;
        nextLevel = level + 1;
        currentLevel.text = $"Level {nextLevel}";
        weaponDescripter.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Managers.Player.GetComponent<PlayerController>().SetWeaponLevel(weaponIndex, nextLevel);
            Managers.Scene.ChangeWeaponLevel(weaponIndex, nextLevel);
            LevelUpUI.SetActive(false);
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
