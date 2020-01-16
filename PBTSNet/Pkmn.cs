
//

using System;

namespace PBTSNet {
	public class Pkmn {

		public static readonly string NULL_DATA = "???";

		private byte id;

		private string name;

		private short hp;
		private byte level;

		private Stats stats;

		private Type.Name[] types;
		private Move[] moves;

		//

		public byte ID {
			get { return id; }
			set { id = value; }
		}

		public short HP {
			get { return hp; }
			set { hp = value; }
		}

		public byte Level {
			get { return level; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public Stats Stats {
			get { return stats; }
			set { stats = value; }
		}

		public Type.Name[] Types {
			get { return types; }
			set { types = value; }
		}

		public Move[] Moves {
			get { return moves; }
			set { moves = value; }
		}

		public Move this[byte mid] {
			get { return moves[mid]; }
			set { moves[mid] = value; }
		}

		//

		public Pkmn(byte id, dynamic branch) {
			this.id = id;

			name = (string)branch.species.name;

			hp = (short)branch.curr_hp;
			level = (byte)branch.level;

			stats = new Stats(branch.stats);

			types = new Type.Name[branch.species.types.Count];
			moves = new Move[branch.moves.Count];

			for(byte i = 0; i < types.Length; i++) {
				types[i] = (Type.Name)Enum.Parse(typeof(Type.Name), (string)branch.species.types[i]);
			}
			for(byte i = 0; i < moves.Length; i++) {
				moves[i] = new Move(branch.moves[i]);
			}

		}

	}
}
