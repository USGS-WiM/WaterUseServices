using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterUseServices.Controllers
{

    public abstract class NSSControllerBase: Controller
    {
        public bool IsAuthorizedToEdit(string OwnerUserName)
        {
            throw new NotImplementedException();
        }

        protected List<string> parse(string items)
        {
            if (items == null) items = string.Empty;
            char[] delimiterChars = { ';', ',' };
            return items.ToLower().Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
