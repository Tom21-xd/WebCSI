using MongoDB.Bson;
using MongoDB.Driver;
using WebCSI.Models;


namespace WebReservas.Data
{
    public class ConexionMongo
    {
        private IMongoDatabase cnm;


        public ConexionMongo()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            cnm = client.GetDatabase("CSI");

        }

        public string UploadImage(ImagenModel img)
        {
            var imageCollection = cnm.GetCollection<ImagenModel>("Imagen");
            
            imageCollection.InsertOne(img);
            return img.Id;
        }

        public ImagenModel GetImage(string id)
        {
            var imageCollection = cnm.GetCollection<ImagenModel>("Imagen");
            return imageCollection.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefault();
        }

        public void DeleteImage(string id)
        {
            var imageCollection = cnm.GetCollection<ImagenModel>("Imagen");
            imageCollection.DeleteOne(new BsonDocument("_id", new ObjectId(id)));
        } 

        public void UpdateImage(ImagenModel img,string id)
        {
            var imageCollection = cnm.GetCollection<ImagenModel>("Imagen");
            var filtro = Builders<ImagenModel>.Filter.Eq("Id", id);
            var update = Builders<ImagenModel>.Update.Set("Imagen", img.Imagen);
            imageCollection.UpdateOne(filtro,update);
        }

    }
}
