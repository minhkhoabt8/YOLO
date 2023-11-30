using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/assetUnit")]
    [ApiController]
    public class AssetUnitController : Controller
    {
        private readonly IAssetUnitService _assetUnitService;

        public AssetUnitController(IAssetUnitService assetUnitService)
        {
            _assetUnitService = assetUnitService;
        }

        /// <summary>
        /// Query  Asset Unit
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetUnitReadDTO>>))]
        public async Task<IActionResult> QueryAssetUnit([FromQuery] AssetUnitQuery query)
        {
            var assetUnits = await _assetUnitService.QueryAssetUnitAsync(query);
            return ResponseFactory.PaginatedOk(assetUnits);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetUnitReadDTO>>))]
        public async Task<IActionResult> GetAllAssetUnits()
        {
            var assetUnits = await _assetUnitService.GetAllAssetUnitAsync();
            return ResponseFactory.Ok(assetUnits);
        }

        /// <summary>
        /// Get actived AssetUnits
        /// </summary>
        /// <returns></returns>
        [HttpGet("getActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetUnitReadDTO>>))]
        public async Task<IActionResult> getAllDeletedAssetUnits()
        {
            var assetUnits = await _assetUnitService.GetActivedAssetUnitAsync();
            return ResponseFactory.Ok(assetUnits);
        }

        /// <summary>
        /// Get Asset Unit Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        public async Task<IActionResult> getAssetUnit(string id)
        {
            var assetUnit = await _assetUnitService.GetAsync(id);
            return ResponseFactory.Ok(assetUnit);
        }


        /// <summary>
        /// Create new AssetUnits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetUnit(AssetUnitWriteDTO input)
        {
            var assetUnit = await _assetUnitService.CreateAssetUnitAsync(input);
            return ResponseFactory.Created(assetUnit);
        }

        /// <summary>
        ///  Create AssetUnit List
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetUnitList(IEnumerable<AssetUnitWriteDTO> input)
        {
            var assetUnit = await _assetUnitService.CreateListAssetUnitAsync(input);
            return ResponseFactory.Created(assetUnit);
        }


        /// <summary>
        /// Update AssetUnit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetUnit(string id, AssetUnitWriteDTO writeDTO)
        {
            var assetUnit = await _assetUnitService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(assetUnit);
        }

        /// <summary>
        /// Delete AssetUnit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetUnit(string id)
        {
            await _assetUnitService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
             await _assetUnitService.CheckCodeAssetUnitNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Check duplicate Name
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateName(string name)
        {
            await _assetUnitService.CheckNameAssetUnitNotDuplicate(name);
            return ResponseFactory.Accepted();
        }

        //import data from excel
        [HttpPost("import")]       
        public async Task<IActionResult> ImportAssetUnitsFromExcel(IFormFile file)
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
                var dataImport = await _assetUnitService.ImportAssetUnitFromExcelAsync(filePath);
                return Ok(new { Message = "Asset unit imported successfully", Data = dataImport });
               
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
