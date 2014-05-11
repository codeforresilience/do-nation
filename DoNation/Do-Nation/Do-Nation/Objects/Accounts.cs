﻿using System;
using SQLite;
[Table("Accounts")]
public class Accounts
{
    [PrimaryKey]
    public string id { get; set; }

    public string email { get; set; }

    public string password { get; set; }

    public string name { get; set; }

    public string address { get; set; }

    public string contactnumber { get; set; }

    public Boolean isvolunteer { get; set; }

    public string volunteerid { get; set; }

    public string orgid { get; set; }
}
