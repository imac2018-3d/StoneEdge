using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Se {
	public class LoreItem {
		public int Type = 0;
		public string Text = "default";

		public LoreItem(int type, string text) {
			this.Type = type;
			this.Text = text;
		}
	}

	public static class LoreItemManager {
		public static Dictionary<int, LoreItem> Lores = new Dictionary<int, LoreItem>()
	    {
			{0, new LoreItem(0, "Ohoki's letter")},
	     	{1, new LoreItem(1, "Astone Mechanics")},
	     	{2, new LoreItem(2, "Report of Haaris")},
	     	{3, new LoreItem(3, "Log of Oli Maroon")},
	     	{4, new LoreItem(3, "Log of Karls")},
	     	{5, new LoreItem(0, "Jaron's letter")}
	    };
	}

}