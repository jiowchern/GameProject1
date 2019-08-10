using System;


using Regulus.Remote;




namespace Regulus.Project.GameProject1.Data
{
	public interface IGameRecorder
	{
		Value<GamePlayerRecord> Load(Guid account_id);

		void Save(GamePlayerRecord game_player_record);
	}
}
