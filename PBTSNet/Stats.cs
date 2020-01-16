using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTSNet {
	public struct Stats {

		public enum Index : byte {
			Health = 0,

			Attack,
			Defense,

			SpAttack,
			SpDefense,

			Speed
		}

		//

		public const short NULL_HP = 225;

		public const short NULL_ATK = 175;
		public const short NULL_DEF = 175;

		public const short NULL_SPATK = 175;
		public const short NULL_SPDEF = 175;

		public const short NULL_SPEED = 150;

		//

		private readonly short health;
		private readonly short attack, defense;
		private readonly short spattack, spdefense;
		private readonly short speed;

		//

		// for mutable hp use Pkmn.hp
		public short MaxHP {
			get { return health; }
		}

		public short Attack {
			get { return attack; }
		}

		public short Defense {
			get { return defense; }
		}

		public short SpAttack {
			get { return spattack; }
		}

		public short SpDefense {
			get { return spdefense; }
		}

		public short Speed {
			get { return speed; }
		}

		//

		public Stats(dynamic branch) {
			try {
				health = (short)branch.hp;

				attack = (short)branch.atk;
				defense = (short)branch.def;

				spattack = (short)branch.spA;
				spdefense = (short)branch.spD;

				speed = (short)branch.spe;
			}
			catch { // "???"
				health = NULL_HP;

				attack = NULL_ATK;
				defense = NULL_DEF;

				spattack = NULL_SPATK;
				spdefense = NULL_SPDEF;

				speed = NULL_SPEED;
			}
		}

	}
}
