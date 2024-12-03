using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesStor.Models;
using ShoesStor.Services;
using ShoesStor.Interfaces;

namespace ShoesStor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyShoesController : ControllerBase
    {                                                                                                                                                                                                                                              
         IShoesService MyShoesService;
        public MyShoesController(IShoesService MyShoesService)
        {
            this.MyShoesService = MyShoesService;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<MyShoes>> Get() {
          return MyShoesService.GetAll(int.Parse(User.FindFirst("id")?.Value!));
        }
           

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<MyShoes> Get(int id)
        {
            var Shoes = MyShoesService.GetById(id);

            if (Shoes == null)
                return NotFound();

            return Shoes;
        }

        [HttpPost] 
        [Authorize(Policy = "User")]
        public ActionResult Post(MyShoes newShoes)
        {
            var newId=MyShoesService.Add(newShoes,int.Parse(User.FindFirst("id")?.Value!));


            return CreatedAtAction("Post", new {id=newId}, MyShoesService.GetById(newId));

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult put(int id, MyShoes newShoes)
        {
            var result = MyShoesService.Update(id, newShoes,int.Parse(User.FindFirst("id")?.Value!));
            if ( ! result){
                return BadRequest();
                }
            return NoContent();
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {


            bool result = MyShoesService.Delete(id);
        if (!result)
            return NotFound();
        return NoContent();
        }
    }
}