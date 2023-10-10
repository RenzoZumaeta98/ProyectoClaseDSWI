using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClaseSemana04.Models
{
    //Sub clase para hacer un listado de la clase principal
    public class ProductListViewModel
    {
        public List<ProductViewModel> List { get; set; }

        public ProductListViewModel() { 
            List = new List<ProductViewModel>();
        }
    }
    public class ProductViewModel
    {
        [DisplayName("Codigo")]
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string Status { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Precio Unitario")]

        //El simbolo de ? es para que admita valores nulos
        //Los corchetes Display es para que tengan nombre por defecto diferentes a sus atributos originales
        public double? Price { get; set; }
        [DisplayName("Disponibilidad")] 
        public int? Stock { get; set; }
       

    }
}
