using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Collections;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        AccountCollection _collection = new AccountCollection();

        [HttpGet]
        public List<Account> GetAllAccount()
        {
            var lstItem = _collection.getItems();
            return lstItem;
        }
    }
}
