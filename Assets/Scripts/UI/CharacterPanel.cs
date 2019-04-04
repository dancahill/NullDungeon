using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
	public Text NameText;
	public Text ClassText;
	public Text LevelText;
	public Text ExperienceText;
	public Text NextLevelText;
	public Text GoldText;

	public Text StrengthBaseText;
	public Text StrengthNowText;
	public Button StrAddButton;
	public Text MagicBaseText;
	public Text MagicNowText;
	public Button MagAddButton;
	public Text DexterityBaseText;
	public Text DexterityNowText;
	public Button DexAddButton;
	public Text VitalityBaseText;
	public Text VitalityNowText;
	public Button VitAddButton;
	public Text UnallocatedText;

	public Text BaseLifeText;
	public Text LifeText;
	public Text BaseManaText;
	public Text ManaText;

	public Text ArmourClassText;
	public Text ToHitText;
	public Text DamageText;

	void Update()
	{
		Character stats = GameManager.instance.PlayerCharacter;
		stats.Recalculate();

		StrAddButton.gameObject.SetActive(stats.Unallocated > 0);
		MagAddButton.gameObject.SetActive(stats.Unallocated > 0);
		DexAddButton.gameObject.SetActive(stats.Unallocated > 0);
		VitAddButton.gameObject.SetActive(stats.Unallocated > 0);

		NameText.text = stats.Name.ToString();
		ClassText.text = stats.Class.ToString();
		LevelText.text = stats.Level.ToString();
		ExperienceText.text = stats.Experience.ToString();
		NextLevelText.text = stats.NextLevel().ToString();
		GoldText.text = stats.Gold.ToString();

		StrengthBaseText.text = stats.Strength.ToString();
		StrengthNowText.text = stats.Strength.ToString();
		MagicBaseText.text = stats.Magic.ToString();
		MagicNowText.text = stats.Magic.ToString();
		DexterityBaseText.text = stats.Dexterity.ToString();
		DexterityNowText.text = stats.Dexterity.ToString();
		VitalityBaseText.text = stats.Vitality.ToString();
		VitalityNowText.text = stats.Vitality.ToString();
		UnallocatedText.text = stats.Unallocated.ToString();

		BaseLifeText.text = stats.BaseLife.ToString();
		LifeText.text = Mathf.FloorToInt(stats.Life).ToString();
		BaseManaText.text = stats.BaseMana.ToString();
		ManaText.text = Mathf.FloorToInt(stats.Mana).ToString();

		ArmourClassText.text = stats.ArmourClass.ToString();
		ToHitText.text = stats.ToHitPercent.ToString() + "%";
		DamageText.text = stats.MinDamage + "-" + stats.MaxDamage;
	}

	public void AddStr()
	{
		Debug.Log("add str");
		GameManager.instance.PlayerCharacter.Strength++;
		GameManager.instance.PlayerCharacter.Unallocated--;
	}

	public void AddMag()
	{
		Debug.Log("add magic");
		GameManager.instance.PlayerCharacter.Magic++;
		GameManager.instance.PlayerCharacter.Unallocated--;
	}

	public void AddDex()
	{
		Debug.Log("add dex");
		GameManager.instance.PlayerCharacter.Dexterity++;
		GameManager.instance.PlayerCharacter.Unallocated--;
	}

	public void AddVit()
	{
		Debug.Log("add vit");
		GameManager.instance.PlayerCharacter.Vitality++;
		GameManager.instance.PlayerCharacter.Unallocated--;
	}
}
