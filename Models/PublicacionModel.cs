namespace WebCSI.Models
{
    public class PublicacionModel
    {
        public int ID_PUBLICACION { get; set; }
        public string TITULO_PUBLICACION { get; set; }
        public string IMAGEN_PUBLICACION { get; set; }
        public string DESCRIPCION_PUBLICACION { get; set; }
        public string FECHA_PUBLICACION { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string NOMBRE_USUARIO { get; set; }

        public IFormFile? File { get; set; }

        public ImagenModel Imagen { get; set; }
    }
}
