﻿using System;

namespace RestWithASPNET.Data.VO {
  
  public class BookVO {    
    public long Id { get; set; }
    public string Author { get; set; }
    public DateTime Launch_date { get; set; }
    public double Price { get; set; }
    public string Title { get; set; }
  }
}
