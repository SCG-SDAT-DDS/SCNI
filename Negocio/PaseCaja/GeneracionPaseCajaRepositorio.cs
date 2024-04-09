using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Negocio.PaseCaja
{
    public class GeneracionPaseCajaRepositorio
    {
        public async Task<bool> ValidarCaptcha(string secret, string captcharesponse)
        {
            using (var cliente = new HttpClient())
            {
                var response =
                    await cliente.GetAsync(
                        "https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" +
                        captcharesponse);
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic resultado = JsonConvert.DeserializeObject(responseString);
                return (bool)resultado.success;
            }
        }
    }
}
