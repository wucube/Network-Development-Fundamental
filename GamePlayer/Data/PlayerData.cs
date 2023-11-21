using System.Collections.Generic;
namespace GamePlayer
{
	public class PlayerData : BaseData
	{
		public int id;
		public float atk;
		public bool sex;
		public long lev;
		public List<int[] arrays;
		public List<int> list;
		public Dictionary<int, string> dict;
		public HeroType heroType;
	}
}