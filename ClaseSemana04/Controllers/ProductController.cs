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
            //Se debe crear un model que haga reflejo a ese objeto
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

        public IActionResult AddSaveAction(ProductViewModel model)
        {
            //AQUI VA LA LOGICA PARA AGREGAR 

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

        public IActionResult Edit()
        {
            var model = new ProductViewModel();
            return View(model);
        }

       
    }
}
