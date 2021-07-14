
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TopSecretLibrary;
using Microsoft.Extensions.Configuration;

namespace TopSecret.Controllers
{
    [Route("api/TopSecret")]
    [ApiController]
    public class TopSecretController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] List<SatelliteSplit> listSat)
        {
            try
            {
                if (listSat != null && listSat.Count == 3) {

                    string x, y;
                    x = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["kenobiX"];
                    y = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["kenobiY"];
                    Position posKenobi = new Position(Convert.ToDouble(x), Convert.ToDouble(y));

                    x = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["skywalkerX"];
                    y = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["skywalkerY"];
                    Position posSkywalker = new Position(Convert.ToDouble(x), Convert.ToDouble(y));

                    x = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["satoX"];
                    y = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Satellites")["satoY"];
                    Position posSato = new Position(Convert.ToDouble(x), Convert.ToDouble(y));

                    TopSecretLibrary.TopSecret objTS = new TopSecretLibrary.TopSecret(posKenobi, posSkywalker, posSato);

                    Position posSpaceship = objTS.GetLocation(listSat[0].distance, listSat[1].distance, listSat[2].distance);
                    if (posSpaceship != null){
                        string Message = objTS.GetMessage(listSat[0].message, listSat[1].message, listSat[2].message);
                        if (Message != ""){
                            Response res = new Response();
                            res.message = Message;
                            res.position = new Position(posSpaceship.X, posSpaceship.Y);

                            return Ok(res);
                        }
                        else
                            return NotFound("No se pudo descifrar el mensaje");
                    }
                    else
                        return NotFound("No se pudo determinar la posición de la nave");
                }
                else
                    return NotFound("No hay suficiente información de los satelites para proceder");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
