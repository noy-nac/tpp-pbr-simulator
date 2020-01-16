using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTS {
	public class Team {

		private Random rand;

		private Pokemon[] pokemon;

		public Pokemon[] Pokemon {
			get { return pokemon; }
		}

		public Pokemon this[byte pkmn] {
			get { return pokemon[pkmn]; }
			set { pokemon[pkmn] = value; }
		}

		public Team(string name, byte size, byte level) {
			rand = new Random();

			pokemon = new Pokemon[size];

			for(byte i = 0; i < pokemon.Length; i++) {
				pokemon[i] = new Pokemon(name, (byte)(i + 1), level);
			}
		}

		public void FillType() {
			for(byte i = 0; i < pokemon.Length; i++) {
				pokemon[i].FillType(string.Empty);

				if(pokemon[i].Type[0] == Type.Error)
					i -= 2;
			}
		}

		public void FillMoves() {
			for(byte i = 0; i < pokemon.Length; i++) {
				pokemon[i].FillMoves(string.Empty);

				if(pokemon[i].Moves[0] == Type.Error)
					i -= 2;
			}
		}

		public void FillStats() {
			for(byte i = 0; i < pokemon.Length; i++) {
				pokemon[i].FillStats(string.Empty);

				if(pokemon[i].Stats[0] == ushort.MaxValue)
					i -= 2;
			}
		}
	}
}
