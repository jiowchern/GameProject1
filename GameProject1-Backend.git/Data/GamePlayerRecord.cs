using System;




namespace Regulus.Project.GameProject1.Data
{
	/// <summary>
	///     game player記錄資料
	/// </summary>

	public class GamePlayerRecord
	{

	    public Guid Id;


	    public Guid Owner;



	    public string Name;


	    public ENTITY Entity;

	    public Item[] Items;

        public GamePlayerRecord()
	    {
            this.Name = "無名" + DateTime.Now.ToLocalTime();
            Items = new Item[0];
        }


    }
}

