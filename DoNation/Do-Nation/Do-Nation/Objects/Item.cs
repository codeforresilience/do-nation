using SQLite;
using System;

[Table("Item")]
public class Item
{
    [PrimaryKey]
    public int itemID { get; set; }
    public String category { get; set; }
    public String quantity { get; set; }
    public int postID { get; set; }

	public Item()
	{
	}
}
