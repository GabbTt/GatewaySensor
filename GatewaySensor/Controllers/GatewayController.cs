using System;
using System.Web.Configuration;
using System.Web.Http;
using NLog;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GatewaySensor.Models;
using Microsoft.EntityFrameworkCore.Update;
using System.Net.Http;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace GatewaySensor.Controllers
{
    public class GatewayController : ApiController
    {

        public static Logger logger = LogManager.GetCurrentClassLogger();

        //base de datos para log
        private static readonly string dbConnection = WebConfigurationManager.AppSettings["DB_CONNECTION"].ToString();


        [HttpPost]
        [Route("api/PostGateway/")]
        public GatewayData PostGateway(GatewayData gatewayData)
        {
            Console.WriteLine(gatewayData);
            logger.Info("Recibido desde Beacon" + JsonConvert.SerializeObject(gatewayData));
            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.GatewayData.Add(gatewayData);
                db.SaveChanges();
            }
            return gatewayData;
        }

        


        [HttpPost]
        [Route("api/RegistroGateways/")]
        public RegistroGateways RegistroGateways(RegistroGateways RegistroGateways)
        {
            Console.WriteLine(RegistroGateways);
            logger.Info("Recibido desde APP" + JsonConvert.SerializeObject(RegistroGateways));
            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.RegistroGateways.Add(RegistroGateways);
                db.SaveChanges();
            }
            return RegistroGateways;
        }

        [HttpGet]
        [Route("api/ListaGateways/")]
        public ICollection<RegistroGateways> ListaGateways()
        {

            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
               return db.RegistroGateways.ToList();
            }
        }


        [HttpGet]
        [Route("api/ListaSensores/")]
        public ICollection<RegistroSensores> ListaSensores()
        {

            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                return db.RegistroSensores.ToList();
            }
        }

        [HttpGet]
        [Route("api/ListaSensoresDisponibles/")]
        public List<RegistroSensores> ListaSensoresDisponibles()
        {
            List<RegistroSensores> listaSensores = new List<RegistroSensores>();
            SqlConnection conn = new SqlConnection(dbConnection);
            conn.Open();

            SqlCommand command = new SqlCommand("Select * from SensoresDisponibles", conn);
            
            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    listaSensores.Add(new RegistroSensores
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        Dmac = Convert.ToString( reader.GetValue(1))
                    });
                }
                    
            }

            conn.Close();
            return listaSensores;

        }

        [HttpGet]
        [Route("api/ListaSensoresVisibles/")]
        public List<SensorAndGatewayData> ListaSensoresVisibles()
        {
            List<SensorAndGatewayData> listaSensores = new List<SensorAndGatewayData>();
            SqlConnection conn = new SqlConnection(dbConnection);
            conn.Open();

            SqlCommand command = new SqlCommand("Select * from SensoresVisibles ORDER BY GMAC, DMAC, RSSI DESC", conn);

            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    listaSensores.Add(new SensorAndGatewayData
                    {
                        Id = Convert.ToInt32(reader.GetValue(1)),
                        SensorTime = Convert.ToDateTime(reader.GetValue(0)),
                        Gmac = Convert.ToString(reader.GetValue(2)),
                        Dmac = Convert.ToString(reader.GetValue(3)),
                        Rssi = Convert.ToString(reader.GetValue(4)),
                        Vbatt = Convert.ToString(reader.GetValue(5)),
                        Temp = Convert.ToString(reader.GetValue(6)),
                        X0 = Convert.ToString(reader.GetValue(7)),
                        Y0= Convert.ToString(reader.GetValue(8)),
                        Z0= Convert.ToString(reader.GetValue(9))
                        
                    });
                }

            }

            conn.Close();
            return listaSensores;

        }

        [HttpGet]
        [Route("api/LocalizarPalletsActivos/")]
        public List<LocalizarPAlletsActivos> LocalizarPalletsActivos()
        {
            List<LocalizarPAlletsActivos> localizarPAlletsActivos = new List<LocalizarPAlletsActivos>();
            SqlConnection conn = new SqlConnection(dbConnection);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM LocalizarPalletsActivos", conn);

            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    localizarPAlletsActivos.Add(new LocalizarPAlletsActivos
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        NumLote = Convert.ToString(reader.GetValue(1)),
                        CodPallet = Convert.ToString(reader.GetValue(2)),
                        Centro = Convert.ToString(reader.GetValue(3)),
                        Bodega = Convert.ToString(reader.GetValue(4)),

                        SensorTime = Convert.ToDateTime(reader.GetValue(5)),
                        Gmac= Convert.ToString(reader.GetValue(6)),
                        Dmac = Convert.ToString(reader.GetValue(7)),
                        Rssi = Convert.ToString(reader.GetValue(8)),
                    });
                }

            }

            conn.Close();
            return localizarPAlletsActivos;

        }

        [HttpGet]
        [Route("api/PalletsEnBodega/")]
        public List<PalletsEnBodegaModel> PalletsEnBodega()
        {
            List<PalletsEnBodegaModel> localizarPAlletsActivos = new List<PalletsEnBodegaModel>();
            SqlConnection conn = new SqlConnection(dbConnection);
            conn.Open();

            SqlCommand command = new SqlCommand("select Centro, Bodega, Count(bodega) Pallets from LocalizarPalletsActivos group by Centro, Bodega", conn);

            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    localizarPAlletsActivos.Add(new PalletsEnBodegaModel
                    {
                        Centro = Convert.ToString(reader.GetValue(0)),
                        Bodega= Convert.ToString(reader.GetValue(1)),
                        Pallets = Convert.ToInt32(reader.GetValue(2))
                    });
                }

            }

            conn.Close();
            return localizarPAlletsActivos;

        }


        [HttpDelete]
        [Route("api/gateways/{gmac}")]
        public IHttpActionResult DeleteGateway(string gmac)
        {
            logger.Info("Recibido desde APP" + gmac);
            var response = new HttpResponseMessage();

            using (var db = new DBContext())
            {
                var itemToRemove = db.RegistroGateways.SingleOrDefault(x => x.Gmac == gmac); //returns a single item.

                if (itemToRemove != null)
                {
                    db.RegistroGateways.Remove(itemToRemove);
                    db.SaveChanges();
                    return Ok();
                }
            }

            return NotFound();
        }



        [HttpPost]
        [Route("api/RegistroSensores/")]
        public RegistroSensores RegistroSensores(RegistroSensores RegistroSensores)
        {
            Console.WriteLine(RegistroSensores);
            logger.Info("Recibido desde APP" + JsonConvert.SerializeObject(RegistroSensores));
            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.RegistroSensores.Add(RegistroSensores);
                db.SaveChanges();
            }
            return RegistroSensores;
        }


        [HttpDelete]
        [Route("api/sensores/{dmac}")]
        public IHttpActionResult DeleteSensor(string dmac)
        {
            logger.Info("Recibido desde APP" + dmac);
            var response = new HttpResponseMessage();

            using (var db = new DBContext())
            {
                var itemToRemove = db.RegistroSensores.SingleOrDefault(x => x.Dmac == dmac); //returns a single item.

                if (itemToRemove != null)
                {
                    db.RegistroSensores.Remove(itemToRemove);
                    db.SaveChanges();
                    return Ok();
                }
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/RegistroPalletSensor/")]
        public RegistroPalletSensor RegistroPalletSensor(RegistroPalletSensor RegistroPalletSensor)
        {
            Console.WriteLine(RegistroPalletSensor);
            logger.Info("Recibido desde APP" + JsonConvert.SerializeObject(RegistroPalletSensor));
            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.RegistroPalletSensor.Add(RegistroPalletSensor);
                db.SaveChanges();
            }
            return RegistroPalletSensor;
        }


        


        [HttpGet]
        [Route("api/ListaSensores/{centro}/{bodega}")]
        public List<RegistroPalletSensor>  ListaSensores(string centro, string bodega)
        {
            Console.WriteLine($"Centro {centro} - Bodega {bodega}");
            
            logger.Info("Recibido desde APP" + $"Centro {centro} - Bodega {bodega}");

            List<RegistroPalletSensor> registroPalletSensors = new List<RegistroPalletSensor>();

            using (var db = new DBContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                registroPalletSensors = db.RegistroPalletSensor.ToList();

            }
            return registroPalletSensors;
        }


    }
}