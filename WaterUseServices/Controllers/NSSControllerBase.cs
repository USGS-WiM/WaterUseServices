using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterUseDB.Resources;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WaterUseServices.Controllers
{

    public abstract class NSSControllerBase: Controller
    {
        public bool IsAuthorizedToEdit(string OwnerUserName)
        {

            if (User.IsInRole("Administrator")) return true;

            var username = LoggedInUser().Username;
            if (!string.IsNullOrEmpty(username) && 
                string.Equals(OwnerUserName, username, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        }
        public Manager LoggedInUser() {
            return new Manager()
            {
                ID = Convert.ToInt32( User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid)
                   .Select(c => c.Value).SingleOrDefault()),
                FirstName = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                   .Select(c => c.Value).SingleOrDefault(),
                LastName = User.Claims.Where(c => c.Type == ClaimTypes.Surname)
                   .Select(c => c.Value).SingleOrDefault(),
                Username = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault(),
                 RoleID = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.Anonymous)
                   .Select(c => c.Value).SingleOrDefault())
            };
        }
        protected List<string> parse(string items)
        {
            if (items == null) items = string.Empty;
            char[] delimiterChars = { ';', ',' };
            return items.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries).Select(i=>i.Trim().ToLower()).ToList();
        }
        protected virtual Boolean isValid(object item)
        {
            try
            {
                var isvalid = this.TryValidateModel(item);
                return isvalid;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected async Task<IActionResult> HandleExceptionAsync(Exception ex)
        {
            return await Task.Run(() => { return HandleException(ex); });
        }
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is DbUpdateException)
            {
                string errorText;
                if (ex.InnerException is Npgsql.PostgresException && dbBadRequestErrors.TryGetValue(Convert.ToInt32(ex.InnerException.Data["Code"]), out errorText))
                {
                    return new BadRequestObjectResult(new Error(400, errorText));
                }
                else
                {
                    return StatusCode(500, new Error(500, "A managed database error occured."));
                }
            }

            else
            {
                return StatusCode(500, new Error(-999, "An error occured while processing your request."));
            }
        }

        private Dictionary<int, string> dbBadRequestErrors = new Dictionary<int, string>
        {
            //https://www.postgresql.org/docs/9.4/static/errcodes-appendix.html
            {23502, "One of the properties requires a value."},
            {23505, "One of the properties is marked as Unique index and there is already an entry with that value."},
            {23503, "One of the related parameters are not available in the db." }
        };
        protected struct Error
        {
            public int Code { get; private set; }
            public string Message { get; private set; }
            public string Content { get; private set; }

            public Error(int c, string msg) {
                this.Code = c;
                this.Message = msg;
                this.Content = getContent(c);
            }

            private static string getContent(Int32 code) {
                switch (code)
                {
                    case 400: return "Bad Request Received";
                    case 500: return "Internal Server Error Occured";
                    default: return "Error not specified";
                        
                }

            }


        }
    }
}
