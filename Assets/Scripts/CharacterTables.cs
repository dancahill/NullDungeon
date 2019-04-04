
public class CharacterTables
{
	public class Level
	{
		public int Experience;
		public int Increase;
	}

	public readonly static Level[] Levels = new Level[] {
		new Level { Experience=    0, Increase= 2000 },
		new Level { Experience= 2000, Increase= 2620 },
		new Level { Experience= 4620, Increase= 3420 },
		new Level { Experience= 8040, Increase= 4449 },
		new Level { Experience=12489, Increase= 5769 },

		new Level { Experience=18258, Increase= 7454 },
		new Level { Experience=25712, Increase= 9597 },
		new Level { Experience=35309, Increase=12313 },
		new Level { Experience=47622, Increase=15742 },
		new Level { Experience=63364, Increase=20055 },
	};
}
