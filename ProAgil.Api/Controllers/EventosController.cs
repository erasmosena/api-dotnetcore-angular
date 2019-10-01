using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Api.DTO;
using ProAgil.Dominio.Model;
using ProAgil.Repositorio.Interfaces;

namespace ProAgil.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventosController : ControllerBase
	{
		private readonly IRepositorio _proAgilRepositorio;
		private readonly IMapper _mapper;
		public EventosController(IRepositorio proAgilRepositorio, IMapper mapper)
		{
			_proAgilRepositorio = proAgilRepositorio;
			_mapper = mapper;

		}

		// GET api/values
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var result = await _proAgilRepositorio.GetAllEventoAsync();
				var mapped = _mapper.Map<IList<EventoDTO>>(result);
				return Ok(mapped);
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}


		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				var result = await _proAgilRepositorio.GetEventoAsyncById(id);
				var mapped = _mapper.Map<EventoDTO>(result);
				return Ok(mapped);
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		// GET api/values/5
		[HttpGet("getByTema/{tema}")]
		public async Task<IActionResult> Get(string tema)
		{
			try
			{
				var result = await _proAgilRepositorio.GetAllEventoAsyncByTema(tema);
				var mapped = _mapper.Map<IList<EventoDTO>>(result);
				return Ok(mapped);
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}



		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] EventoDTO dto)
		{
			try
			{
				Evento model = _mapper.Map<Evento>(dto);
				_proAgilRepositorio.Add(model);
				if (await _proAgilRepositorio.SaveChangesAsync())
				{
					return Created($"/api/evento/{model.EventoId}", model);
				}
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
			return BadRequest();
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] Evento model)
		{
			try
			{
				var old = await _proAgilRepositorio.GetEventoAsyncById(id);
				if (old == null) return NotFound();
				_mapper.Map(model, old);
				_proAgilRepositorio.Update(model);
				if (await _proAgilRepositorio.SaveChangesAsync())
				{
					return Created($"/api/evento/{model.EventoId}", _mapper.Map<EventoDTO>(old));
				}
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
			return BadRequest();
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var old = await _proAgilRepositorio.GetEventoAsyncById(id);
				if (old == null)
				{
					return NotFound();
				}
				_proAgilRepositorio.Delete(old);
				if (await _proAgilRepositorio.SaveChangesAsync())
				{
					return Ok();
				}
			}
			catch (System.Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
			return BadRequest();
		}



		// POST api/values
		[HttpPost("{action}")]
		public async Task<IActionResult> Upload()
		{
			try
			{
				var file = Request.Form.Files[0];
				var folderName = Path.Combine("Resources", "Images");
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(),folderName);
				if(file.Length > 0 ){
					var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
					var fullPath = Path.Combine(pathToSave,filename.Replace("\""," ").Trim());
					using( var stream = new FileStream(fullPath,FileMode.Create)){
						file.CopyTo(stream);
					}
				}
				return Ok();
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
			return BadRequest("Erro ao tentar fazer upload");
		}
	}
}
