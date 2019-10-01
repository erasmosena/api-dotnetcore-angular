using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Dominio.Model;
using ProAgil.Repositorio.Interfaces;

namespace ProAgil.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
	public class PalestrantesController:ControllerBase
    {
		private readonly IRepositorio _proAgilRepositorio;
        public PalestrantesController(IRepositorio proAgilRepositorio)
		{
			_proAgilRepositorio = proAgilRepositorio;
		}

		
		 // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
			try
			{
				var result = await _proAgilRepositorio.GetAllPalestranteAsync();
				return Ok(result);	
			}
			catch (System.Exception ex )
			{
				
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
            
        }

		
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult>  Get(int id)
        {
			try
			{
				var result = await _proAgilRepositorio.GetPalestranteAsync(id);
				return Ok(result);	
			}
			catch (System.Exception ex )
			{
				
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
        }

		

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Palestrante model)
        {
			try
			{
				_proAgilRepositorio.Add(model);
				if( await _proAgilRepositorio.SaveChangesAsync() ){
					return Created($"/api/evento/{model.PalestranteId}",model);
				}
			}
			catch (System.Exception ex )
			{
				
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
			return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Palestrante model)
        {
			try
			{
				var old = await _proAgilRepositorio.GetPalestranteAsync(id);
				if( old == null ){
					return NotFound();
				}
				_proAgilRepositorio.Update(model);
				if( await _proAgilRepositorio.SaveChangesAsync() ){
					return Created($"/api/evento/{model.PalestranteId}",model);
				}
			}
			catch (System.Exception ex )
			{
				
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
			return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
			try
			{
				var old = await _proAgilRepositorio.GetPalestranteAsync(id);
				if( old == null ){
					return NotFound();
				}
				_proAgilRepositorio.Delete(old);
				if( await _proAgilRepositorio.SaveChangesAsync() ){
					return Ok();
				}
			}
			catch (System.Exception ex )
			{
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
			return BadRequest();
        }


    }
}