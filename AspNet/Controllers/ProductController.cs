﻿using AspNet.DataAccess;
using AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ProductController(ApplicationContext context) 
        {
            _context = context;
        }

        [HttpGet("Products")]
        [Authorize(Roles = "user,admin")]

        public IActionResult Get()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        [Authorize(Roles = "user,admin")]

        public IActionResult GetProduct([FromRoute]int id) 
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) { 
            return NotFound();
            }
            return Ok(product);
        }


        [HttpPost("Products")]
        [Authorize(Roles = "admin")]

        public IActionResult Save([FromBody]Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("Products")]
        [Authorize(Roles = "admin")]

        public IActionResult Update([FromBody]Product product) 
        {   
            var result = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);
            if(result == null)
            {
                return NotFound();
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Products")]
        [Authorize(Roles = "admin")]

        public IActionResult Delete([FromQuery]int id)
        {
            var deleteProduct = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (deleteProduct == null)
            {
                return NotFound();
            }

            _context.Products.Remove(deleteProduct);
            _context.SaveChanges();

            return Ok();
        }
    }
}
