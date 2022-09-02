using System;
using System.Web.Configuration;
using System.Web.Http;
using NLog;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GatewaySensor.Models;

namespace GatewaySensor.Controllers
{
    /// <summary>
    /// Prueba de api y Beacon
    /// 
    /// </summary>
    public class BeaconController : ApiController
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        
        //base de datos para log
        private static readonly string dbConnection = WebConfigurationManager.AppSettings["DB_CONNECTION"].ToString();


        /// <summary>
        /// Recibe JSON desde beacon y lo guarda en el log
        /// </summary>
        /// <param name="beaconInfoJSON">Representa el JSON generado por el beacon</param>
        /// <returns>Devuelve el mismo objeto recibido</returns>
        [HttpPost]
        [Route("api/PostBeacon/")]
        public object Post(BeaconInfoJSON beaconInfoJSON)
        {
            Console.WriteLine(beaconInfoJSON);
            logger.Info("Recibido desde Beacon" + JsonConvert.SerializeObject(beaconInfoJSON));
            using (var db = new BeaconContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.BeaconInfoJSON.Add(beaconInfoJSON);
                db.SaveChanges();
            }
            return beaconInfoJSON;
        }


        /// <summary>
        /// Recibe JSON desde app para guardar en DB
        /// </summary>
        /// <param name="informacionPallet">Representa el JSON para ingresar la informacion del pallet</param>
        /// <returns>Devuelve el mismo objeto recibido</returns>
        /*[HttpPost]
        [Route("api/PostInformacionPallets/")]
        public object PostInformacionPallets(InformacionPallet informacionPallet)
        {
            Console.WriteLine(informacionPallet);
            logger.Info("Recibido desde Beacon" + JsonConvert.SerializeObject(informacionPallet));
            
            using (IDbConnection cn = new SqlConnection(dbConnection))
            {
                DynamicParameters p = new DynamicParameters();

                p.Add()
            }
            return informacionPallet;
        }
        */


        /*
        [HttpPost]
        [Route("api/PostLote/")]
        public object PostLote(LoteSap LoteSap)
        {
            Console.WriteLine(LoteSap);
            logger.Info("Recibido desde Beacon" + JsonConvert.SerializeObject(LoteSap));
            using (var db = new BeaconContext())
            {
                //Agregamos el json al contexto BeaconContext para guardar en la base
                //usando entity framework
                db.LoteSap.Add(LoteSap);
                db.SaveChanges();
            }
            return LoteSap;
        }*/


        /// <summary>
        /// Recibe mac address del sensor y devuelve todos sus registros
        /// </summary>
        /// <param name="dmac">mac address del sensor</param>
        /// <returns>Devuelve una lista de SensorInfo</returns>
        [HttpGet]
        [Route("api/GetSensorInfoList/")]
        public ICollection<SensorInfo> Get(string dmac)
        {

            Console.WriteLine(dmac);
            logger.Info("Get request dmac = " + dmac);

            //DbContext  linq
            List<SensorInfo> sensorsInfoList;
            using (var db = new BeaconContext())
            {
               sensorsInfoList = db.SensorInfo.Where( s => s.dmac == dmac && s.temp > 0).OrderBy(s => s.time).ToList();
            }

            logger.Info("Get Response: " + JsonConvert.SerializeObject(sensorsInfoList));
            return sensorsInfoList;
        }

        [HttpGet]
        [Route("api/GetSensorMacList/")]
        public List<String> GetSensorsMacs()
        {
            logger.Info("Get GetSensorsMacs");

            //DbContext  
            List<String> sensorsInfoList;
            using (var db = new BeaconContext())
            {
                sensorsInfoList = db.SensorInfo.Where(s => s.temp > 0).OrderBy(s => s.time).Select(s => s.dmac).Distinct().ToList();
            }

            logger.Info("Get Response: " + JsonConvert.SerializeObject(sensorsInfoList));
            return sensorsInfoList;
        }
    }
}

