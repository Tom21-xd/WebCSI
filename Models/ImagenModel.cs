using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebCSI.Models
{
    public class ImagenModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Imagen { get; set; }
        public string Name { get; set; }
    }
}
