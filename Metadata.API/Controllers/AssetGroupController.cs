using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/assetGroup")]
    [ApiController]
    public class AssetGroupController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;

        public AssetGroupController(IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        /// <summary>
        /// Get all AssetGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> GetAllAssetGroups()
        {
            var assetGroups = await _assetGroupService.GetAllAssetGroupsAsync();
            return ResponseFactory.Ok(assetGroups);
        }

        /// <summary>
        /// Get all Actived AssetGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> GetAllDeletedAssetGroups()
        {
            var assetGroups = await _assetGroupService.GetAllActivedAssetGroupAsync();
            return ResponseFactory.Ok(assetGroups);
        }

        /// <summary>
        /// Get Asset Group Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        public async Task<IActionResult> GetAssetGroup(string id)
        {
            var assetGroup = await _assetGroupService.GetAssetGroupAsync(id);
            return ResponseFactory.Ok(assetGroup);
        }

        /// <summary>
        /// Create new AssetGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetGroup(AssetGroupWriteDTO input)
        {
            var assetGroup = await _assetGroupService.CreateAssetGroupAsync(input);
            return ResponseFactory.Created(assetGroup);
        }

        /// <summary>
        /// Create List AssetGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetGroups(IEnumerable<AssetGroupWriteDTO> input)
        {
            var assetGroups = await _assetGroupService.CreateAssetGroupsAsync(input);
            return ResponseFactory.Created(assetGroups);
        }

        /// <summary>
        /// Update AssetGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetGroup(string id, AssetGroupWriteDTO writeDTO)
        {
            var assetGroup = await _assetGroupService.UpdateAssetGroupAsync(id, writeDTO);
            return ResponseFactory.Ok(assetGroup);
        }

        /// <summary>
        /// Delete AssetGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetGroup(string id)
        {
            await _assetGroupService.DeleteAssetGroupAsync(id);
            return ResponseFactory.NoContent();
        }


        /// <summary>
        /// Check Duplicate Name
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateName(string name)
        {
             await _assetGroupService.CheckNameAssetGroupNotDuplicate(name);
            return ResponseFactory.Accepted();
        }


        /// <summary>
        /// Check Duplicate Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
            await _assetGroupService.CheckCodeAssetGroupNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  Asset Group
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> QueryAssetGroup([FromQuery] AssetGroupQuery query)
        {
            var assetGroups = await _assetGroupService.QueryAssetGroupAsync(query);
            return ResponseFactory.PaginatedOk(assetGroups);
        }

        //import data from excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportAssetGroups(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string filePath = Path.GetTempFileName();

            // Save the uploaded file to a temporary file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                var dataImport = await _assetGroupService.ImportAssetGroupsFromExcelAsync(filePath);

                return Ok(new { Message = "Asset groups imported successfully", Data = dataImport });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
