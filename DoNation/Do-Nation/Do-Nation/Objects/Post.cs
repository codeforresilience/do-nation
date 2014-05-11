using System;
using SQLite;

[Table("Post")]
public class Post
{
    [PrimaryKey, AutoIncrement]
    public int postID { get; set; }
    public String Location { get; set; }
    int longitude { get; set; }
    int latitude { get; set; }
    public String status { get; set; }
    public DateTime datePosted {get; set;}
    public String specialInstruction { get; set; }
    public String userID{get; set;}
    public String availableTime { get; set; }
    public String volunteerID { get; set; }
    public Boolean notifForDonator { get; set; }
    public Boolean notifForVolunteer { get; set; }

	public Post()
	{
	}
}
