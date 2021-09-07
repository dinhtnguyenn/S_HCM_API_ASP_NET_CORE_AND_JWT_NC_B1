using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("/api/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly dinhntco_studywithmeContext db;

        public ProductsController(dinhntco_studywithmeContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("all-product")]
        public IActionResult GetProducts()
        {
            return Ok(db.Products.ToList());
        }

        [HttpGet]
        // [Route("detail-product")]
        [Route("detail-product/{id}")]
        public IActionResult GetProductDetailByID(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.No == id);
            if (product == null)
            {
                return Ok(new Product());
                //return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("detail-product-by-name")]
        public IActionResult GetProductDetailByName(string name)
        {
            Product product = db.Products.SingleOrDefault(p => p.ProductName == name);
            if (product == null)
            {
                return Ok(new Product());
                // return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("detail-product-by-multi-param")]
        public IActionResult GetProductDetailByMultiParam(int id, string name)
        {
            Product product = db.Products.SingleOrDefault(p => p.ProductName == name && p.No == id);
            if (product == null)
            {
                return Ok(new Product());
                // return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("add-product")]
        public IActionResult AddProduct(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new Message(0, "Thêm sản phẩm không thành công. Vui lòng thử lại"));
                }

                db.Products.Add(product);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Ok(new Message(0, "Thêm sản phẩm không thành công. Vui lòng thử lại"));
            }

            return Ok(new Message(1, "Thêm sản phẩm thành công"));
        }
    }
}
