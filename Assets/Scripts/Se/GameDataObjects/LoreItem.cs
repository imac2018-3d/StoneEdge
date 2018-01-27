using UnityEngine;
using System.Collections.Generic;
using System;

namespace Se {
	[Serializable]
	public class LoreItemData {
		public int Type;
		public string Text;

		LoreItemData() {}
		public LoreItemData(int type, string text) {
			this.Type = type;
			this.Text = text;
		}
	}

	public static class LoreItemManager {
		public static Dictionary<int, LoreItemData> Lores = new Dictionary<int, LoreItemData>()
	    {
			{0, new LoreItemData(0, "Ohoki's letter")},
	     	{1, new LoreItemData(1, "Astone Mechanics")},
	     	{2, new LoreItemData(2, "Report of Haaris")},
	     	{3, new LoreItemData(3, "Log of Oli Maroon")},
	     	{4, new LoreItemData(3, "Log of Karls")},
	     	{5, new LoreItemData(0, "Jaron's letter")}
	    };
	}

}