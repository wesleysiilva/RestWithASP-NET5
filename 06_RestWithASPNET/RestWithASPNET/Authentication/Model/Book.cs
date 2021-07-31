﻿using RestWithASPNET.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace RestWithASPNET.Model {
  [Table("books")]
  public class Book : BaseEntity {

    [Column("author")]
    public string Author { get; set; }

    [Column("launch_date")]
    public DateTime LaunchDate { get; set; }

    [Column("price")]
    public double Price { get; set; }

    [Column("title")]
    public string Title { get; set; }
  }
}
