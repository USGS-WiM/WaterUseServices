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

    public abstract class WUControllerBase: WiM.Services.Controllers.ControllerBase
    {
        protected IWaterUseAgent agent;

        public WUControllerBase(IWaterUseAgent sa)
        {
            this.agent = sa;
        }
        protected bool IsAuthorizedToEdit<T> () where T:class
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
        protected Manager LoggedInUser() {
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
        protected new virtual Boolean isValid(object item)
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
        protected new IActionResult HandleException(Exception ex)
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
    }
}
