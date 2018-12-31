using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedStarter.API.DataContract.Note;
using RedStarter.Business.DataContract.Note;

namespace RedStarter.API.Controllers.Note
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INoteManager _manager;

        public NoteController(IMapper mapper, INoteManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostNote(NoteCreateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(400);

            int identityClaimNum = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var dto = _mapper.Map<NoteCreateDTO>(request);
            dto.DateCreated = DateTime.Now;
            dto.OwnerId = identityClaimNum;

            if(await _manager.CreateNote(dto))
                return StatusCode(201);

            throw new Exception();
        }
    }
}