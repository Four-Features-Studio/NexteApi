using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Common;
using NexteAPI.Configs;
using NexteAPI.DTO.AuthRequestes;
using NexteAPI.DTO.AuthResponses;
using NexteAPI.DTO.FilesRequestes;
using NexteAPI.DTO.FilesResponse;
using NexteAPI.Entity;
using NexteAPI.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NexteAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IOptions<SystemConfig> options;
        ILogger<FilesController> logger;
        IFileService fileService;

        public FilesController(IOptions<SystemConfig> _options, ILogger<FilesController> _logger, IFileService _fileService)
        {
            options = _options;
            logger = _logger;
            fileService = _fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Profiles()
        {
            var profiles = fileService.Profiles.Values.ToArray();
            return Ok(profiles);
        }

        [HttpPost]
        public async Task<IActionResult> AssetsIndex([FromBody]AssetsIndexRequest assetsIndex)
        {
            if (assetsIndex == null)
                return BadRequest();

            if (!fileService.AssetsIndexes.ContainsKey(assetsIndex.Version))
                return NotFound();

            var index = fileService.AssetsIndexes[assetsIndex.Version];

            return Ok(index);
        }

        [HttpPost]
        public async Task<IActionResult> Client([FromBody]ClientFilesRequest clientFiles)
        {
            if(clientFiles == null)
                return BadRequest();

            if (string.IsNullOrEmpty(clientFiles.ProfileId))
                return BadRequest();

            if (!fileService.Clients.ContainsKey(clientFiles.ProfileId))
                return NotFound();

            var client = fileService.Clients[clientFiles.ProfileId];

            var result = new ClientFilesResponse()
            {
                TypeHash = ChecksumMethod.SHA1,
                Files = client
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]CheckUpdateRequest checkUpdate)
        {
            var TEST = new UpdateInfoResponse()
            {
                IsOutdated = false,
                Size = 1000
            };
            return Ok(TEST);
        }
    }
}
