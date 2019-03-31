using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
	GameManager Manager;

	Text NameText;
	Text ClassText;

	Text LevelText;
	Text ExperienceText;
	Text NextLevelText;

	Text StrengthText;
	Text DexterityText;
	Text VitalityText;

	Text BaseLifeText;
	Text LifeText;
	Text BaseManaText;

	void Start()
	{
		Text findtext(string s)
		{
			string p = "ScrollArea/Content/" + s + "Box/Text";
			return transform.Find(p).GetComponent<Text>();
		}
		Manager = GameManager.instance;

		NameText = findtext("Name");
		ClassText = findtext("Class");

		LevelText = findtext("Level");
		ExperienceText = findtext("Experience");
		NextLevelText = findtext("NextLevel");

		StrengthText = findtext("Strength");
		DexterityText = findtext("Dexterity");
		VitalityText = findtext("Vitality");

		BaseLifeText = findtext("BaseLife");
		LifeText = findtext("Life");
		BaseManaText = findtext("BaseMana");
	}

	void Update()
	{
		CharacterStats stats = Manager.PlayerStats;
		stats.Recalculate();

		NameText.text = stats.Name.ToString();
		ClassText.text = stats.Class.ToString();

		LevelText.text = stats.Level.ToString();
		ExperienceText.text = stats.Experience.ToString();
		NextLevelText.text = stats.NextLevel().ToString();

		StrengthText.text = stats.Strength.ToString();
		DexterityText.text = stats.Dexterity.ToString();
		VitalityText.text = stats.Vitality.ToString();

		BaseLifeText.text = stats.BaseLife.ToString();
		LifeText.text = stats.Life.ToString("0");
		BaseManaText.text = stats.BaseMana.ToString();
	}
}
