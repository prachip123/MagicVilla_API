using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        public readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Get All Villas");
            return Ok(Villastore.ListVilla);
        }

        [HttpGet("id",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error");
                return BadRequest();
            }

            var villa = Villastore.ListVilla.FirstOrDefault(i => i.Id == id);
            if(villa==null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            if(villaDTO==null)
            {
                return BadRequest();
            }
            if (Villastore.ListVilla.FirstOrDefault(i => i.Name.ToLower() == villaDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("Custom Error", "Villa already Exists");
                return BadRequest(ModelState);
            }
            if(villaDTO.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = Villastore.ListVilla.OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            Villastore.ListVilla.Add(villaDTO);

            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla",new {id = villaDTO.Id}, villaDTO);
        }

        [HttpDelete("id",Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            VillaDTO villa=Villastore.ListVilla.FirstOrDefault(i=>i.Id==id);
            if(villa==null)
            {
                return NotFound();
            }
            Villastore.ListVilla.Remove(villa);
            return NoContent();
        }

        [HttpPut("id",Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO == null || id!=villaDTO.Id)
            {
                return BadRequest();
            }
            VillaDTO villa = Villastore.ListVilla.FirstOrDefault(i => i.Id == id);
            villa.Name = villaDTO.Name;
            villa.Id = villaDTO.Id;
            villa.Sqmeter = villaDTO.Sqmeter;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }

        [HttpPatch("id", Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchdoc)
        {
            if(patchdoc==null || id==0)
            {
                return BadRequest();
            }
            var villa=Villastore.ListVilla.FirstOrDefault(i=>i.Id==id);
            if(villa==null)
            {
                return NotFound();
            }
            patchdoc.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


        }
}
