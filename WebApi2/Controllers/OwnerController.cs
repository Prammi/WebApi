using AutoMapper;
using Contracts;
using Entities.DTO;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2.ActionFilters;

namespace WebApi2.Controllers
{

    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {

            var owners = await _repository.Owner.GetAllOwnersAsync();
            var ownerResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
            // throw new Exception("Exception while fetching all the students from the storage.");
           // throw new AccessViolationException("Violation Exception while accessing the resource.");
            _logger.LogInfo($"Returned all owners from database.");
            return Ok(owners);


        }

        [HttpGet("{id}", Name = "OwnerById")]
        //[ServiceFilter(typeof(ValidateIdFilterAttribute))]
        public async Task<IActionResult> GetOwnerById(string id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned owner with id: {id}");
                var ownerResult = _mapper.Map<OwnerDto>(owner);
                return Ok(ownerResult);
            }

        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetOwnerWithDetails(string id)
        {
            try
            {
                var owner = await _repository.Owner.GetOwnerWithDetailsAsync(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");
                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreationDto owner)
        {
           var ownerEntity = _mapper.Map<Owner>(owner);
            ownerEntity.Id = Guid.NewGuid().ToString();
            _repository.Owner.CreateOwner(ownerEntity);
            await _repository.SaveAsync();
            var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);
            return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(string id, [FromBody] OwnerForUpdateDto owner)
        {

            if (owner == null)
            {
                _logger.LogError("Owner object sent from client is null.");
                return BadRequest("Owner object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid owner object sent from client.");
                return BadRequest("Invalid model object");
            }
            var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);
            if (ownerEntity == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            _mapper.Map(owner, ownerEntity);
            _repository.Owner.UpdateOwner(ownerEntity);
            await _repository.SaveAsync();
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(string id)
        {

            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            if (_repository.Account.AccountsByOwner(id).Any())
            {
                _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
            }
            _repository.Owner.DeleteOwner(owner);
            await _repository.SaveAsync();
            return NoContent();

        }

        [HttpGet("GetOwnerById2/{id}", Name = "OwnerById2")]       
        public async Task<IActionResult> GetOwnerById2(string id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned owner with id: {id}");
                var ownerResult = _mapper.Map<OwnerDto>(owner);
                return Ok(ownerResult);
            }

        }
    }
}
