using Firebase.Auth;
using Firebase.Storage;
using FireBase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FireBase.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            
            Stream archivoASubir = archivo.OpenReadStream();

            
            string email = "rodrigo.monterrosa@catolica.edu.sv";
            string clave = "Sonic2002";
            string ruta = "prueba-dba43.appspot.com";
            string api_key = "AIzaSyBZhr-ye38uR6FnjSygen4Mo8Vph7s_HdU";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);
            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            
            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                        new FirebaseStorageOptions
                                                        {
                                                            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                                            ThrowOnCancel = true
                                                        }
                                                        ).Child("Archivos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);

            
            var urlArchivoCargado = await tareaCargarArchivo;

            
            return RedirectToAction("VerImagen", new { urlImagen = urlArchivoCargado });
        }


       

        [HttpGet]
        public ActionResult VerImagen(string urlImagen)
        {
            
            return View((object)urlImagen); 
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
