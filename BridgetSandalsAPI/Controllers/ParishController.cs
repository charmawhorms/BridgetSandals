using BridgetSandalsAPI.Data;
using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParishController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ParishController(ApplicationDbContext db)
        {
            _db = db;
        }


        //Method to get all parishes
        [HttpGet]
        public IActionResult GetParishes()
        {
            var parishes = _db.Parishes.ToList();
            if (parishes == null)
            {
                return NotFound();
            }

            return Ok(parishes);
        }


        //Method to get a parish by id
        [HttpGet("{id}")]
        public IActionResult GetParishById(int id)
        {
            var parish = _db.Parishes.FirstOrDefault(x => x.Id == id);
            if (parish == null)
            {
                return NotFound();
            }

            return Ok(parish);
        }


        //Method to add a student to the db
        [HttpPost]
        public IActionResult CreateParish([FromBody] Parish parish)
        {
            _db.Parishes.Add(parish);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetParishById), new { id = parish.Id }, parish);
        }


        //Method to update a parish by id
        [HttpPut("{id}")]
        public IActionResult UpdateParish(int id, [FromBody] Parish parish)
        {
            if (id != parish.Id)
            {
                return NotFound();
            }

            _db.Parishes.Update(parish);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetParishById), new { id = parish.Id }, parish);
        }


        //Method to delete a parish by id
        [HttpDelete("{id}")]
        public IActionResult DeleteParish(int id)
        {
            var parish = _db.Parishes.FirstOrDefault(x => x.Id == id);
            if (parish == null)
            {
                return NotFound();
            }
            _db.Parishes.Remove(parish);
            _db.SaveChanges();

            return Ok(parish);
        }
    }
}
