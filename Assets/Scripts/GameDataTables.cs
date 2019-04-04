
public class GameDataTables
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

	public class EnemyStats
	{
		public string Name;
		public int Level;
		public int MinHP;
		public int MaxHP;
		public int ArmourClass;
		public int ToHitPercent;
		public int MinDamage;
		public int MaxDamage;
		public int BaseExp;

		public static EnemyStats Find(string name)
		{
			foreach (EnemyStats stats in Enemies)
				if (stats.Name == name) return stats;
			return null;
		}
	}
	public readonly static EnemyStats[] Enemies = new EnemyStats[] {
		new EnemyStats { Name="Zombie",   Level=1, MinHP=2, MaxHP=3, ArmourClass=5, ToHitPercent=10, MinDamage=2, MaxDamage=5, BaseExp=54 },
		new EnemyStats { Name="Skeleton", Level=1, MinHP=1, MaxHP=2, ArmourClass=0, ToHitPercent=20, MinDamage=1, MaxDamage=4, BaseExp=64 },
	};
}
