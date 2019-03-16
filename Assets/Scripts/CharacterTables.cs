
public class CharacterTables
{
	public class Level
	{
		public int Experience;
		public int Increase;
	}

	public static Level[] Levels = new Level[] {
		new Level { Experience=    0, Increase=2000 },
		new Level { Experience= 2000, Increase=2620 },
		new Level { Experience= 4620, Increase=3420 },
		new Level { Experience= 8040, Increase=4449 },
		new Level { Experience=12489, Increase=5769 },
	};
}
