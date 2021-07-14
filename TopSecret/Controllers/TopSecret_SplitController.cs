
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TopSecretLibrary;

namespace TopSecret.Controllers
{
    [Route("api/TopSecret_Split")]
    [ApiController]
    public class TopSecret_SplitController : ControllerBase
    {
        [HttpPost("{satellite}")]
        public IActionResult TopSecret_Split(string satellite,[FromBody] SatelliteSplit objSatelliteSplit)
        {
            try
            {
                satellite = satellite.Trim().ToLower();

                if (satellite.Trim() == "")
                    return BadRequest("Debe indicar el nombre del satelite");

                if (objSatelliteSplit == null)
                    return BadRequest("Debe indicar la información del satelite");

                if (objSatelliteSplit.distance <= 0)
                    return BadRequest("La distancia debe ser un valor mayor a cero");

                if(satellite.ToLower() != "kenobi" && satellite.ToLower() != "skywalker" && satellite.ToLower() != "sato")
                    return BadRequest("El nombre del satelite debe ser: kenobi, skywalker o sato");

                //Me conecto a Firestore
                string path = AppDomain.CurrentDomain.BaseDirectory + @"afmeli.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                FirestoreDb db = FirestoreDb.Create("afmeli");

                DocumentReference doc = db.Collection("satellites").Document(satellite);
                doc.SetAsync(objSatelliteSplit);

                return Ok("Información registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //Me conecto a Firestore
                string path = AppDomain.CurrentDomain.BaseDirectory + @"afmeli.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                FirestoreDb db = FirestoreDb.Create("afmeli");

                Query qref = db.Collection("satellites");

                QuerySnapshot snap = await qref.GetSnapshotAsync();

                if (snap.Count != 3) 
                    return NotFound("No posee la información de los tres satelites");

                List<Satellite> listSat = new List<Satellite>();

                foreach (DocumentSnapshot doc in snap){
                    SatelliteSplit esatSplit = doc.ConvertTo<SatelliteSplit>();
                    Satellite esat = new Satellite();
                    esat.name = doc.Id;
                    esat.distance = esatSplit.distance;
                    esat.message = esatSplit.message;

                    listSat.Add(esat);
                }

                if (listSat != null && listSat.Count == 3){
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

                    double dist1=0, dist2=0, dist3=0;

                    foreach (Satellite item in listSat){
                        if (item.name == "kenobi")
                            dist1 = item.distance;
                        else if (item.name == "skywalker")
                            dist2 = item.distance;
                        else
                            dist3 = item.distance;
                    }

                    Position posSpaceship = objTS.GetLocation(dist1, dist2, dist3);
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
