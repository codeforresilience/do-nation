using System;
using SQLite;

[Table("OrganizationKeyList")]
public class OrganizationKeyList
{
    [PrimaryKey]
    public String volunteerID { get; set; }
    public int organizationID { get; set; }

	public OrganizationKeyList()
	{
	}
}
