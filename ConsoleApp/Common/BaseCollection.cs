using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public abstract class BaseCollection<T>
    {
        public abstract string CollectionName { get; }
        IMongoCollection<T> _collection;
        public IMongoCollection<T> Collection
        {
            get
            {
                if (_collection is null)
                {
                    _collection = Database.Db.GetCollection<T>(CollectionName);
                }
                return _collection;
            }
        }
    }
}
