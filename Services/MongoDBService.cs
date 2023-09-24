using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoExmaple.Models;

namespace MongoExample.Services;

public class MongoDBService {

    private readonly IMongoCollection<Playlist> _playlistCollection;

    public MongoDBService (IOptions<MongoDBSettings> mongoDBsettings) {
        MongoClient client = new MongoClient(mongoDBsettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBsettings.Value.DatabaseName);
        _playlistCollection = database.GetCollection<Playlist>(mongoDBsettings.Value.CollectionName);
    }

    public async Task CreateAsync(Playlist playlist) {
        await _playlistCollection.InsertOneAsync(playlist);
    }

    public async Task<List<Playlist>> GetAsync() {
        return await _playlistCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task AddToPlaylistAsync(string id, string movieId){
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
        UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("items", movieId);
        await _playlistCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
        await _playlistCollection.DeleteOneAsync(filter);
    }


}