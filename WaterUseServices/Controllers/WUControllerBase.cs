using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterUseDB.Resources;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WaterUseAgent;

namespace WaterUseServices.Controllers
{

    public abstract class WUControllerBase: Controller
    {
        protected IWaterUseAgent agent;

        public WUControllerBase(IWaterUseAgent sa)
        {
            this.agent = sa;
        }
        public bool IsAuthorizedToEdit<T> () where T:class
        {

            if (User.IsInRole("Administrator")) return true;

            var username = LoggedInUser();

            switch (typeof(T).Name)
            {
                case "Source":

                default:
                    break;
            }

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
                //Must clear ModelState before validation to ensure any manual updates are applied during validation.
                ModelState.Clear();
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
                    return new BadRequestObjectResult(new Error(errorEnum.e_badRequest, errorText));
                }
                else
                {
                    return StatusCode(500, new Error(errorEnum.e_internalError, "A managed database error occured."));
                }
            }
            else
            {
                return StatusCode(500, new Error(errorEnum.e_internalError, "An error occured while processing your request."));
            }
        }

        private Dictionary<int, string> dbBadRequestErrors = new Dictionary<int, string>
        {
            //https://www.postgresql.org/docs/9.4/static/errcodes-appendix.html
            {23502, "One of the properties requires a value."},
            {23505, "One of the properties is marked as Unique index and there is already an entry with that value."},
            {23503, "One of the related features prevents you from performing this operation to the database." }
        };
        protected struct Error
        {
            public int Code { get; private set; }
            public string Message { get; private set; }
            public string Content { get; private set; }

            public Error(errorEnum c, string msg) {
                this.Code = (int)c;
                this.Message = msg;
                this.Content = getContent(c);
            }
            public Error(errorEnum c)
            {
                this.Code = (int)c;
                this.Message = getDefaultmsg(c);
                this.Content = getContent(c);
            }

            private static string getContent(errorEnum code) {
                switch (code)
                {
                    case errorEnum.e_badRequest: return "Bad Request Received";
                    case errorEnum.e_notFound: return "Not Found";
                    case errorEnum.e_notAllowed: return "Method Not Allowed.";
                    case errorEnum.e_internalError: return "Internal Server Error Occured";
                    case errorEnum.e_unauthorize: return "Unauthorized";
                    default: return "Error not specified";                        
                }

            }
            private static string getDefaultmsg(errorEnum code)
            {
                switch (code)
                {
                    case errorEnum.e_badRequest: return "Object is invalid, please check you have populated all required properties and try again.";
                    case errorEnum.e_notFound: return "Object was not found.";
                    case errorEnum.e_notAllowed: return "Method not allowed.";
                    case errorEnum.e_internalError: return "Internal server error occured";
                    case errorEnum.e_unauthorize: return "Unauthorized to perform this action.";
                    default: return "Error not specified";

                }

            }


        }
        protected enum errorEnum
        {
            e_badRequest=400,
            e_unauthorize =401,
            e_notFound=404,
            e_notAllowed=405,
            e_internalError=500,
            e_error=0
        }
    }
}
