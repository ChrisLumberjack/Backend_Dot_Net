using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
namespace Backend_Net.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly FirebaseAuth _firebaseAuth;

        // Agrega este bloque al principio de tu archivo AuthController.cs
        public class VerifyTokenRequest
        {
            public string IdToken { get; set; }
        }

        public AuthController()
        {
            FirebaseApp app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = "backendnet-c442c"
            });

            _firebaseAuth = FirebaseAuth.GetAuth(app);
        }

        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenRequest request)
        {
            try
            {
                var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(request.IdToken);
                var uid = decodedToken.Uid;
                var user = await _firebaseAuth.GetUserAsync(uid);

                // Aquí puedes procesar la información del usuario y responder con éxito
                return Ok(user);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest($"Error al verificar el token: {ex.Message}");
            }
        }
    }
}
