using System;
using SQLite;

[Table("Organization")]
public class Organization
{
    [PrimaryKey]
    public int organizationID { get; set; }
    public String organizationName { get; set; }

	public Organization()
	{
	}
}
