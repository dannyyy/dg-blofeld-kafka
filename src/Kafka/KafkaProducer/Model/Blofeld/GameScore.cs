// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Blofeld
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class GameScore : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"GameScore\",\"namespace\":\"Blofeld\",\"fields\":[{\"name\":\"Game" +
				"Id\",\"type\":\"int\"},{\"name\":\"UserId\",\"type\":\"int\"},{\"name\":\"Score\",\"type\":\"int\"}]}" +
				"");
		private int _GameId;
		private int _UserId;
		private int _Score;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return GameScore._SCHEMA;
			}
		}
		public int GameId
		{
			get
			{
				return this._GameId;
			}
			set
			{
				this._GameId = value;
			}
		}
		public int UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				this._UserId = value;
			}
		}
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.GameId;
			case 1: return this.UserId;
			case 2: return this.Score;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.GameId = (System.Int32)fieldValue; break;
			case 1: this.UserId = (System.Int32)fieldValue; break;
			case 2: this.Score = (System.Int32)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
