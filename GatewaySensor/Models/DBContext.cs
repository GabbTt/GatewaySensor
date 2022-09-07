using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace GatewaySensor.Models
{

    public class LocalizarPAlletsActivos
    {
        public int Id { get; set; }
        public DateTime SensorTime { get; set; }
        public string Gmac { get; set; }
        public string Dmac { get; set; }

        public string NumLote { get; set; }
        public string CodPallet { get; set; }
        public string Centro { get; set; }
        public string Bodega { get; set; }
        public string Rssi { get; set; }
    }

    public class SensorAndGatewayData
    {
        public int Id { get; set; }
        public DateTime SensorTime { get; set; }
        public string Gmac { get; set; }
        public string Dmac { get; set; }
        public string Rssi { get; set; }
        public string Vbatt { get; set; }
        public string Temp { get; set; }
        public string X0 { get; set; }
        public string Y0 { get; set; }
        public string Z0 { get; set; }


    }


    public class GatewayData
    {
        public int Id { get; set; }
        public string Msg { get; set; }
        public string Gmac { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? HoraRegistro = DateTime.Now;

        public ICollection<SensorData> obj { get; set; }
    }

    public class SensorData
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public string Dmac { get; set; }
        public string Time { get; set; }
        public int? Rssi { get; set; }
        public int? Ver { get; set; }
        public int? Vbatt { get; set; }
        public double? Temp { get; set; }
        public double? Humidty { get; set; }
        public int? X0 { get; set; }
        public int? Y0 { get; set; }
        public int? Z0 { get; set; }
        public string Data1 { get; set; }
        public string Uuid { get; set; }
        public int? MajorId { get; set; }
        public int? MinorId { get; set; }
        public int? Refpower { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? HoraRegistro = DateTime.Now;
    }

    public class RegistroGateways
    {
        public int Id { get; set; }
        public string Gmac { get; set; }
        public string Centro { get; set; }
        public string Bodega { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Z { get; set; }
        public bool? EsActivo { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? HoraRegistro = DateTime.Now;
    }

    public  class RegistroSensores
    {
        public int Id { get; set; }
        public string Dmac { get; set; }
        public bool? EsActivo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? HoraRegistro = DateTime.Now;

        
    }

    public partial class RegistroPalletSensor
    {
        public int Id { get; set; }
        public string NumLote { get; set; }
        public string NumOrden { get; set; }
        public string CodPallet { get; set; }
        public string ColorPallet { get; set; }
        public string PesoPalletVacio { get; set; }
        public string Producto { get; set; }
        public string TipoProducto { get; set; }
        public string PesoProducto { get; set; }

        public string Dmac { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? HoraRegistro = DateTime.Now;

       

    }

    public class PalletsEnBodegaModel
    {
        public string Centro { get; set; }
        public string Bodega { get; set; }
        public int Pallets { get; set; }


    }

    public class DBContext : DbContext
    {


        public DBContext(): base("name=DBConnectionString")
        {
        }

        public virtual DbSet<GatewayData> GatewayData { get; set; }
        public virtual DbSet<RegistroGateways> RegistroGateways { get; set; }
        public virtual DbSet<RegistroPalletSensor> RegistroPalletSensor { get; set; }
        public virtual DbSet<RegistroSensores> RegistroSensores { get; set; }
        public virtual DbSet<SensorData> SensorData { get; set; }






    }
}
