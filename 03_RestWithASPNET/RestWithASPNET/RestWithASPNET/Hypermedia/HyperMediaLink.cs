using System;
using System.Text;

namespace RestWithASPNET.Hypermedia {
  public class HyperMediaLink {
    public String Rel { get; set; }
    public String Type { get; set; }
    public String Action { get; set; }
    public String Href {
      get {
        object _lock = new object();
        lock (_lock) {
          StringBuilder sb = new StringBuilder(href);
          return sb.Replace("%2f", "/").ToString();
        }
      }
      set {
        Href = value;
      }
    }
    private String href;
  }
}