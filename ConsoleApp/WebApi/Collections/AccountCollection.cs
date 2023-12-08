using ConsoleApp.Models;
using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class AccountCollection : BaseCollection<Account>
    {
        public override string CollectionName
        {
            get
            {
                return "Account";
            }
        }

        public List<Account> getItems()
        {
            var filter = Builders<Account>.Filter.Empty;
            var rs = Collection.Find<Account>(filter);
            return rs.ToList();
        }

        public void InsertItem(Account acc)
        {
            Collection.InsertOne(acc);
        }

        public Account getAccount_ByUserName(string un)
        {
            var filter = Builders<Account>.Filter.Empty;
            var rs = Collection.Find<Account>(filter).ToList().FirstOrDefault();
            return rs;
        }
    }
}
