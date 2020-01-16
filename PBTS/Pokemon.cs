using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTS {
	public class Pokemon {

		public enum Stat : byte {
			HP = 0,
			Attack,
			Defense,
			SpAttack,
			SpDefense,
			Speed,
		}

		public const byte TYPEMAX = 2;
		public const byte MOVEMAX = 4;

		public const byte STATSIZE = 6;

		private Type[] type;
		private Type[] moves;

		private ushort[] stats;

		private string team;
		private byte id;

		private byte level;

		public byte Level {
			get { return level; }
		}

		public string Team {
			get { return team; }
		}

		public byte ID {
			get { return id; }
		}

		public Type[] Type {
			get { return type; }
			set { type = value; }
		}

		public Type[] Moves {
			get { return moves; }
			set { moves = value; }
		}

		public ushort[] Stats {
			get { return stats; }
			set { stats = value; }
		}

		public Pokemon(string team, byte id, byte level) {
			this.team = team;
			this.id = id;

			this.level = level;
		}

		public void FillType(string head) {
			if(!head.Equals(null) && !head.Equals(string.Empty))
				Console.WriteLine(head);

			type = new Type[Program.tree.teams[team == "BLUE" ? 0 : 1][id - 1].species.types.Count];

			for(byte i = 0; i < type.Length; i++) {
				type[i] = (Type)Enum.Parse(typeof(Type), (string)Program.tree.teams[team == "BLUE" ? 0 : 1][id - 1].species.types[i]);
			}

			/*Console.Write("*  {0}  *  {1}  *  {2}   *  \t>", team, "PKMN #" + id, "TYPE");

			string get = Console.ReadLine();
			string[] getv = get.Split(new char[] { ' ', ',', '.', '/', '\\', '`' }, StringSplitOptions.RemoveEmptyEntries);

			type = new Type[getv.Length];

			if(type.Length > TYPEMAX || type.Length == 0) {
				FillType("\n*  0 < INPUTS <= "+TYPEMAX+"  * !");
				return;
			}
			for(byte i = 0; i < type.Length; i++) {
				try {
					type[i] = (Type)Convert.ToByte(getv[i]);
				}
				catch {
					if(getv[i] == "redo") {
						type[0] = PBTS.Type.Error;
						return;
					}
					else {
						FillType("*  INPUT TYPE ERROR  * !");
						return;
					}
				}
			}*/
		}

		public void FillMoves(string head) {
			if(!head.Equals(null) && !head.Equals(string.Empty))
				Console.WriteLine(head);

			moves = new Type[Program.tree.teams[team == "BLUE" ? 0 : 1][id - 1].moves.Count];

			for(byte i = 0; i < moves.Length; i++) {
				try {
					moves[i] = (Type)Enum.Parse(typeof(Type), (string)Program.tree.teams[team == "BLUE" ? 0 : 1][id - 1].moves[i].type);
				}
				catch {
					moves[i] = 0;
				}
			}

			/*Console.Write("*  {0}  *  {1}  *  {2}  *  \t>", team, "PKMN #" + id, "MOVES");

			string get = Console.ReadLine();
			string[] getv = get.Split(new char[] { ' ', ',', '.', '/', '\\', '`' }, StringSplitOptions.RemoveEmptyEntries);

			moves = new Type[getv.Length];

			if(moves.Length > MOVEMAX || moves.Length == 0) {
				FillMoves("\n*  0 < INPUTS <= "+MOVEMAX+"  * !");
				return;
			}
			for(byte i = 0; i < moves.Length; i++) {
				try {
					moves[i] = (Type)Convert.ToByte(getv[i]);
				}
				catch {
					if(getv[i] == "redo") {
						moves[0] = PBTS.Type.Error;
						return;
					}
					else {
						FillMoves("*  INPUT TYPE ERROR  * !");
						return;
					}
				}
			}*/
		}

		public void FillStats(string head) {
			if(!head.Equals(null) && !head.Equals(string.Empty))
				Console.WriteLine(head);

			//stats = new ushort[Program.tree.teams[team == "BLUE" ? 0 : 1][id].stats];

			dynamic branch = Program.tree.teams[team == "BLUE" ? 0 : 1][id - 1].stats;

			stats = new ushort[] { (ushort)branch.hp, (ushort)branch.atk, (ushort)branch.def, (ushort)branch.spA, (ushort)branch.spD, (ushort)branch.spe };

			/*for(byte i = 0; i < stats.Length; i++) {
				moves[i] = (Type)Enum.Parse(typeof(Type), (string)Program.tree.teams[team == "BLUE" ? 0 : 1][id].stats[i]);
			}/*

			/*Console.Write("*  {0}  *  {1}  *  {2}  *  \t>", team, "PKMN #" + id, "STATS");

			string get = Console.ReadLine();

			string[] getv = get.Split(new char[] { ' ', ',', '.', '/', '\\', '`' }, StringSplitOptions.RemoveEmptyEntries);

			stats = new ushort[getv.Length];

			if(stats.Length != STATSIZE) {
				FillStats("*  INPUTS = " + STATSIZE + "  * !");
				return;
			}
			for(byte i = 0; i < stats.Length; i++) {
				try {
					stats[i] = Convert.ToUInt16(getv[i]);
				}
				catch {
					if(getv[i] == "redo") {
						stats[0] = ushort.MaxValue;
						return;
					}
					else {
						FillStats("*  INPUT TYPE ERROR  * !");
						return;
					}
				}
			}*/
		}

	}
}
