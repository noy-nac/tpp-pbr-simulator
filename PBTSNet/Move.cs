using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTSNet {
	public struct Move { 

		public enum DmgType : byte {
			Null = 0,

			Physical,
			Special,
			Status
		}

		// camelCase b/c that's how the API is
		// and we want to parse a string to the enum
		public enum UseType : byte {
			Null = 0,

			normal,
			allAdjacentFoes,

			self,
			allySide,

			all,
		}

		public static readonly string NULL_NAME = "NULL";

		public const byte NULL_POWER = 0;
		public const byte NULL_ACC = 0;

		public const byte MAX_ACC = 100;

		public const byte NULL_POINTS = 0;

		//

		private readonly string name;

		private readonly byte power;
		private readonly byte accuracy;

		private readonly Type.Name type;
		private readonly DmgType dmgtype;

		private readonly UseType usetype;

		// can change
		private byte points;

		//

		public string Name {
			get { return name; }
		}

		public byte Power {
			get { return power; }
		}

		public byte Accuracy {
			get { return accuracy; }
		}

		public Type.Name Type {
			get { return type; }
		}

		public DmgType DamageType {
			get { return dmgtype; }
		}

		public UseType Use {
			get { return usetype; }
		}

		public byte Points {
			get { return points; }
			set { points = value; }
		}

		//

		public Move(dynamic branch) {
			try { 
				name = (string)branch.name;

				power = (byte)branch.power;
				accuracy = (byte)((branch.accuracy != null) ? branch.accuracy : NULL_ACC);

				type = (branch.type != null ? (Type.Name)Enum.Parse(typeof(Type.Name), (string)branch.type));
				dmgtype = (DmgType)Enum.Parse(typeof(DmgType), (string)branch.category);

				usetype = (UseType)Enum.Parse(typeof(UseType), (string)branch.target);

				points = (byte)branch.pp;
			}
			catch {
				name = NULL_NAME;

				power = NULL_POWER;
				accuracy = NULL_ACC;

				type = PBTSNet.Type.Name.Null;
				dmgtype = DmgType.Null;

				usetype = UseType.Null;

				points = NULL_POINTS;
			}
		}

	}
}
