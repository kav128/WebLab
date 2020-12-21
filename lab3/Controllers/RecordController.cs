#nullable enable
using System.Collections.Generic;
using lab3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace lab3.Controllers
{
    [ApiController]
    [Route("")]
    public class RecordController : ControllerBase
    {
        private readonly ILogger<RecordController> _logger;
        private readonly IStorageService _storageService;

        public RecordController(ILogger<RecordController> logger, IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet]
        public JObject? Get()
        {
            if (_storageService.IsEmpty()) return null;

            JObject jObject = new();
            IEnumerable<(string, JToken)> tuples = _storageService.GetAll();
            foreach (var (key, token) in tuples) jObject.Add(key, token);

            return jObject;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(JObject jObject)
        {
            if (!_storageService.IsEmpty())
                return BadRequest();
                
            foreach ((string key, JToken? value) in jObject)
                _storageService.PutToken(key, value);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(JObject jObject)
        {
            if (_storageService.IsEmpty())
                return BadRequest();
            
            _storageService.Clear();
            
            foreach ((string key, JToken? value) in jObject)
                _storageService.PutToken(key, value);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Patch(JObject jObject)
        {
            if (_storageService.IsEmpty())
                return BadRequest();
            
            foreach ((string key, JToken? value) in jObject)
                _storageService.PutToken(key, value);

            return Ok();
        }
    }
}
