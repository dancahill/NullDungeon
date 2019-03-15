using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
	GameManager Manager;
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
		CharacterStats stats = Manager.player.GetComponent<Player>().Stats;
		LevelText.text = stats.Level.ToString();
		ExperienceText.text = stats.Experience.ToString();
		NextLevelText.text = stats.NextLevel().ToString();

		StrengthText.text = stats.Strength.ToString();
		DexterityText.text = stats.Dexterity.ToString();
		VitalityText.text = stats.Vitality.ToString();

		BaseLifeText.text = stats.BaseLife.ToString();
		LifeText.text = stats.Life.ToString();
		BaseManaText.text = stats.BaseMana.ToString();
	}
}
