using System.Collections.Generic;

namespace RestWithASPNET.Data.Converter.Contract {
  
  //O - Origem | D - Destino
  public interface IParser<O, D> {
    D Parse(O origin);
    List<D> Parse(List<O> origin);
  }
}
