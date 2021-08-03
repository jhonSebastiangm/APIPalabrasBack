using APIPalabras.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APIPalabras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalabrasController : ControllerBase
    {
        [HttpGet("{frase}")]
        public IEnumerable<Palabras> GetByFrase(string frase)
        {
            IEnumerable<string> PalabrasObtenidas = ObtenerListaDePalabras(frase);
            var resultadoPalabrasObtenidas = PalabrasObtenidas.GroupBy(x => x).Select(group => new {
                Palabra = group.Key,
                Cantidad = group.Count()
            }).OrderByDescending(x => x.Cantidad).FirstOrDefault();
            return Enumerable.Range(0, 1).Select(index => new Palabras
            {
                palabra = resultadoPalabrasObtenidas.Palabra,
                cantidadRepetida = resultadoPalabrasObtenidas.Cantidad,
                fraseString = frase
            })
            .ToArray();
        }
        private static IEnumerable<string> ObtenerListaDePalabras(string frase)
        {
            string nuevaFrase = Regex.Replace(frase, @"[^\w\@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            return nuevaFrase.Split(' ').Where(st => !st.Equals(""));
        }
    }
}
