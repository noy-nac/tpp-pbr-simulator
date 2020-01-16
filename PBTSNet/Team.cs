using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTSNet {
	public class Team {

		private string name;

		private byte id;

		private Pkmn[] pokemon;

		//

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public byte ID {
			get { return id; }
			set { id = value; }
		}

		public Pkmn this[byte id] {
			get { return pokemon[id]; }
			set { pokemon[id] = value; }
		}

		// for pokemon HP
		public short this[byte id, short delta] {
			get { return pokemon[id].HP; }
			set { pokemon[id].HP += delta; }
		}

		//

		public Team(string name, byte id, dynamic branch) {
			this.name = name;

			this.id = id;

			pokemon = new Pkmn[branch.Count];

			for(byte i = 0; i < pokemon.Length; i++) {
				pokemon[i] = new Pkmn(i, branch[i]);
			}
		}

	}
}
