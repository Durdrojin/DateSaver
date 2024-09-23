﻿using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace DateSaver
{
    [Table("date")]
    public class Date
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("date_name")]
        public string DateName { get; set; }

        [AllowNull]
        [Column("description")]
        public string Description { get; set; }

        [Column("date_saved")]
        public DateTime DateSaved { get; set; }
    }
}
