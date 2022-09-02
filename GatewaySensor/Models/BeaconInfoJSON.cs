using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;


namespace GatewaySensor.Models
{
    [Table("BeaconInfoJSON")]
    public  class BeaconInfoJSON
    {
        public int id { get; set; }
        public string msg { get; set; }
        public string gmac { get; set; }
        public ICollection<SensorInfo> obj { get; set; }
    }

    // Root myDesrializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Table("SensorInfo")]
    public  class SensorInfo
    {
        public int id { get; set; }
        public int type { get; set; }
        public string dmac { get; set; }
        public string time { get; set; }
        public int rssi { get; set; }
        public int ver { get; set; }
        public int vbatt { get; set; }
        public double temp { get; set; }
        public double humidty { get; set; }
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int z0 { get; set; }
        public string data1 { get; set; }
        public string uuid { get; set; }
        public int? majorID { get; set; }
        public int? minorID { get; set; }
        public int? refpower { get; set; }

        //public string numLote { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    //[Table("InformacionPallets")]
    public class InformacionPallet
    {
        public int id { get; set; }
        public string Planta { get; set; }
        public string Bodega { get; set; }
        public string IdPallet { get; set; }

        public string Color { get; set; }
        public string Peso { get; set; }
        public string Producto { get; set; }
        public string Tipo { get; set; }
        public string MacSensor { get; set; }


    }

    public class BeaconContext : DbContext
    {
        //name es el nombre 
        public BeaconContext() : base("name=DBConnectionString")
        {
        }
        //Entities
        public DbSet<SensorInfo> SensorInfo { get; set; }
        public DbSet<BeaconInfoJSON> BeaconInfoJSON { get; set; }

        //public DbSet<InformacionPallet> InformacionPallet { get; set; }





    }


}