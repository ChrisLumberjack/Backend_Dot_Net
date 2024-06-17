using Microsoft.AspNetCore.Mvc;
using Backend_Net.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;
using Backend_Net.Utils;

namespace Backend_Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly IFirestoreService _firestoreService;

        public UserController(IFirestoreService firestoreService)
        {
            _firestoreService = firestoreService;
        }

        // POST api/user
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromQuery] string collectionName, [FromBody] JsonElement userData)
        {
            try
            {
                var userDataDes = JsonUtils.ConvertJsonElement(userData) as Dictionary<string, object>;

                await _firestoreService.AddUserAsync(collectionName, userDataDes);  // null para generar ID aleatorio
                return Ok("User added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/user/{collectionName}/{documentId}
        [HttpGet("{collectionName}/{documentId}")]
        public async Task<IActionResult> GetUserAsync(string collectionName, string documentId)
        {
            try
            {
                var user = await _firestoreService.ReadUserAsync(collectionName, documentId);
                return Ok(user); // Devuelve los datos del usuario como respuesta HTTP OK (200)
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        // DELETE api/user/{collectionName}/{documentId}
        [HttpDelete("{collectionName}/{documentId}")]
        public async Task<IActionResult> DeleteUserAsync(string collectionName, string documentId)
        {
            try
            {
                await _firestoreService.DeleteUserAsync(collectionName, documentId);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/user/{collectionName}/{documentId}
        [HttpPut("{collectionName}/{documentId}")]
        public async Task<IActionResult> UpdateUserAsync(string collectionName, string documentId, [FromBody] JsonElement updatedData)
        {
            try
            {

                var userDataDes = JsonUtils.ConvertJsonElement(updatedData) as Dictionary<string, object>;
                await _firestoreService.UpdateUserAsync(collectionName, documentId, userDataDes);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
