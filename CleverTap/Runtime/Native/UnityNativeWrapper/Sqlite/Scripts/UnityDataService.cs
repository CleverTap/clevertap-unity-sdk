#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

//TODO: ADD NOT UNITY_ANDROID/IOS platform checks everywhere.
namespace CleverTapSDK.Native
{

    public class UnityDataService
    {   
        private SQLiteConnection _connection;

        public UnityDataService(string databaseName)
        {
            string fileExt = ".db";
            string dbPath = Path.Combine(Application.persistentDataPath, databaseName + fileExt);
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }

        public void Disconnect()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public void CreateTable<T>(bool deleteIfExists = false)
        {
            if (deleteIfExists)
            {
                _connection.DropTable<T>();
            }

            _connection.CreateTable<T>();
        }

        public int Insert<T>(T entry)
        {
            _connection.Insert(entry);
            return GetLastInsertedEntry();
        }

        public void Delete<T>(T entry)
        {
            _connection.Delete(entry);
        }

        public void Delete<T>(int id)
        {
            _connection.Delete<T>(id);
        }

        public void DeleteTable<T>()
        {
            _connection.DropTable<T>();
        }

        public void DeleteDatabase(string databaseName)
        {
            string dbPath = Path.Combine(Application.persistentDataPath, databaseName);
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        public int GetLastInsertedEntry()
        {
            return _connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }

        public T GetEntryById<T>(int id) where T : new()
        {
            return _connection.Find<T>(id);
        }

        public List<T> GetAllEntries<T>() where T : class, new()
        {
            return _connection.Table<T>().ToList();
        }
    }
}
#endif