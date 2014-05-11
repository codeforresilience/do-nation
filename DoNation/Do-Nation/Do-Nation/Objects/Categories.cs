using System;
using SQLite;

[Table("Categories")]
public class Categories
{
    public int categoryID { get; set; }
    public String categoryName { get; set; }

	public Categories()
	{
	}
}
