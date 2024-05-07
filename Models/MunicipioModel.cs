namespace WebCSI.Models
{
    public class MunicipioModel
    {
        public int ID_MUNICIPIO { get; set; }
        public string NOMBRE_MUNICIPIO { get; set; }
        public int FK_ID_DEPARTAMENTO { get; set; }
    }
}
