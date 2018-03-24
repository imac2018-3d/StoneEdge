using UnityEngine;
using System.Collections.Generic;
using System;

namespace Se {
	[Serializable]
	public class LoreItemData {
		public int Type;
		public string Text;
		public string Content;

		LoreItemData() {}
		public LoreItemData(int type, string text, string content) {
			this.Type = type;
			this.Text = text;
			this.Content = content;
		}
	}

	public static class LoreItemManager {
		public static Dictionary<int, LoreItemData> Lores = new Dictionary<int, LoreItemData>()
	    {
			{0, new LoreItemData(0, "Ohoki's letter", "The astone has some magnetics properties. When the Karpec coefficient of an object is upper than 0.2 then we call it a Karpec object. A Karpec object can be attracted by astones. Karpec object with high coefficient can have a Juiss flow which can be used for transporting astones. \nNote from the teacher Goldbaum:\n“For instance, in the future model of stone guardian, the Stone Edge, we put a system which allows the guardian to move through Juiss tracks, the bleu magnetics tracks we built with the Bekipics”\n")},
			{1, new LoreItemData(1, "Astone Mechanics", "The command of the officer was to pick up all archives we can find. The main subject of these documents was about a “Stone Edge” which seems to be last model of guardian but didn’t find it. I think it’s stored into the room we can’t open. This problem set apart, we get almost all documents written by the priest. I expect that any archive won’t be lost on the way.")},
			{2, new LoreItemData(2, "Report of Haaris", "Hi Karls !\nI think you and your partners can abandon the Nexus Pic. The Juiss tracks was really useful but without the property of astones we can’t do anything. We’re disabling the tracks from each destinations but for the moment we don’t destroy it. Who knows ? One day it will we be usable. Anyway, I expect to see you soon.\n")},
			{3, new LoreItemData(3, "Log of Oli Maroon", "I heard some rumors saying that the Bekipics are preparing their army. Against What ? I’m not sure, but I’m thinking that the Bekipics are afraid of our technologies. Nobody wants to mind me and despite my warnings, nobody wants to activate the defense system. According to them, it would be a sign of hostility. They’re not wrong but they’re not right either. I expect i’m wrong about the Bekipics…")},
			{4, new LoreItemData(3, "Log of Karls", "The downhill is really painful without the Great Downhill track. The Astone people might be selfish, they had a great fuel and they decided to lock the property of the astones only to prevent others to use it. Now nobody can use the Juiss tracks. Damn it.")},
			{5, new LoreItemData(0, "Jaron's letter", "Hi Loopik, the situation seems to be critical. I don’t know why but suddenly the Bekipics have become hostile and started to attack everybody. This is weird, they have some white mask now and seems to had losen their mind. Well, in all case, the Jaja jungle is not safe anymore. I expect you get this message at time and we’ll meet soon.")}
	    };
	}

}