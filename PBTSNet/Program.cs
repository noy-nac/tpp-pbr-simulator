using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Newtonsoft.Json;

namespace PBTSNet {
	public class Program {

		public static WebClient api;
		public static dynamic match;

		//public static Team blue;
		//public static Team red;

		public static void Main(string[] args) {

			api = new WebClient();

			Refresh();

			Pkmn p = new Pkmn(0, match.teams[0][1]);

			Console.ReadLine();

		}


		public static void Refresh() {
			match = JsonConvert.DeserializeObject(api.DownloadString("https://twitchplayspokemon.tv/api/current_match"));
		}

	}
}
