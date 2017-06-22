using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterUseDB.Resources;
using System.Security.Claims;

namespace WaterUseServices.Controllers
{

    public abstract class NSSControllerBase: Controller
    {
        public bool IsAuthorizedToEdit(string OwnerUserName)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

            if (!string.IsNullOrEmpty(username) && 
                string.Equals(OwnerUserName, username, StringComparison.OrdinalIgnoreCase) &&
                (User.IsInRole("Manager")||User.IsInRole("Administrator")))
                return true;

            return false;
        }

        protected List<string> parse(string items)
        {
            if (items == null) items = string.Empty;
            char[] delimiterChars = { ';', ',' };
            return items.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries).Select(i=>i.Trim().ToLower()).ToList();
        }
    }
}
