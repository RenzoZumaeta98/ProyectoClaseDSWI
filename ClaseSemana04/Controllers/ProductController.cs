using ClaseSemana04.Database;
using ClaseSemana04.Database.ProductContext;
using ClaseSemana04.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ClaseSemana04.Controllers
{
    public class ProductController : Controller
    {
        //SE REALIZA INYECCION DE DEPENDENCIAS
        private readonly ProductContext _productContext;

        public ProductController(ProductContext productContext)
        {
            this._productContext = productContext;
        }

        public IActionResult List()
        {
            //No olvidar que el listResult es un objeto de base de datos y no se puede enviar asi no mas
            //Se debe crear un model que haga reflejo a ese objeto (en este caso, el ProductViewModel en la carpeta Models)
            var listResult = _productContext.Products.ToList();

            ProductListViewModel model = new ProductListViewModel();

            //Se debe mapear la informacion para que se pueda rellenar el objeto model y se pueda enviar
            model.List = (from c in listResult select new ProductViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Stock = c.Stock,
                Status = c.Status,
                Price = c.Price,

            }).ToList();

            return View(model);
        }

        public IActionResult Add()
        {
            ProductViewModel model = new ProductViewModel();
            return View(model);
        }

        //AQUI VA LA LOGICA PARA AGREGAR 
        [HttpPost]
        public IActionResult AddSaveAction(ProductViewModel model)
        {
            ProductEntity entity = new ProductEntity();
            entity.Name = model.Name;
            //Se les pone HasValue porque en PreductViewModel se puso que podian ser nulos (por conveniencia)
            entity.Price = model.Price.HasValue? model.Price.Value :0; 
            entity.Stock = model.Stock.HasValue ? model.Stock.Value : 0;
            entity.DateAdded = DateTime.Now;
            entity.Status = "A";
            _productContext.Products.Add(entity);
            //Con este ultimo metodo se llama al SaveChanges del ProductContext y se graba
            _productContext.SaveChanges();

            return RedirectToAction("List", "Product");
        }

        public IActionResult Edit(int id)   
        {
            //Se usa LinQ para extraer el modelo, siempre lo trae como lista
            //Se debe convertir SingleOrDefault trae un objeto o un objeto vacio por defecto
            var findProduct = _productContext.Products.Where(c => c.Id == id).SingleOrDefault();

            //Ahora que se trajo el modelo de la base de datos, se debe convertir al modelo en Proyecto ProductViewModel

            var model = new ProductViewModel();
            model.Id = findProduct.Id;
            model.Name = findProduct.Name;  
            model.Price = findProduct.Price;    
            model.Stock = findProduct.Stock;


            return View(model);
        }

        [HttpPost] //Es buena practica colocar el Verbo HTTP
        public IActionResult EditSaved(ProductViewModel model)
        {
            //Mismo metodo, pero de otra forma (sin poner Where)
            var findProduct = _productContext.Products.SingleOrDefault(c => c.Id == model.Id);
            if(findProduct != null)
            {
                findProduct.Name = model.Name;
                findProduct.Price = model.Price.HasValue?model.Price.Value:0; //Si tiene valor nulo, ponerle valor del formulario o cero
                findProduct.Stock = model.Stock.HasValue ? model.Stock.Value : 0;
                _productContext.SaveChanges();

                //Si se deja asi no mas, tratara de enviarlo a la ista  EditSaved
                //Se tiene que reenviar a una vista 
                return RedirectToAction("List", "Product");

            }

            // Esto puede ser una opcion si se quiere mantener en la misma vista return View("Edit", model);
            //Sino se deja solo en el model y se deriva al List con el metodo de arriba
            return View(model);
        }

        [HttpGet]
        public JsonResult DeleteProd(int id)
        {
            var findProduct = _productContext.Products.SingleOrDefault(c => c.Id == id);
            _productContext.Products.Remove(findProduct);
            _productContext.SaveChanges();
            return Json("Se elimino de manera correcta");
        }


        [HttpGet]
        public JsonResult GetProductDetail(int id)
        {
            var product = _productContext.Products.Where(c => c.Id == id).SingleOrDefault();
            return Json(new
            {
                ProductName = product.Name,
                ProductPrice = product.Price,
                ProductStock = product.Stock
            });
        }
    }
}
