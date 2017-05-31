//------------------------------------------------------------------------------
//----- Authentication -------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   
//
//discussion:   Authentication is the process of determining who you are, while Authorisation 
//              evolves around what you are allowed to do, i.e. permissions. Obviously before 
//              you can determine what a user is allowed to do, you need to know who they are, 
//              so when authorisation is required, you must also first authenticate the user in some way.  
//          
//              https://andrewlock.net/introduction-to-authentication-with-asp-net-core/

using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaterUseAgent;
using WiM.Security;


//where all the authentication work is actually done
namespace WaterUseServices.Security.Authentication.Basic
{
    internal class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private IWaterUseAgent _agent;
        public BasicAuthenticationHandler(IWaterUseAgent agent) {
            this._agent = agent;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                string authorization = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorization))
                {
                    return AuthenticateResult.Skip();
                }
                if (!authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    AuthenticateResult.Skip();
                }

                var encodedUsernamePassword = authorization.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);                   
                

                var manager = _agent.GetManagerByUsername(username);

                if (manager == null || !Cryptography.VerifyPassword(password, manager.Salt, manager.Password)) {
                    return AuthenticateResult.Skip();
                }

                //set the user
                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, manager.FirstName, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Surname, manager.FirstName, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Email, manager.Email, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Role, manager.Role.Name, ClaimValueTypes.String)
                        };
                var userIdentity = new ClaimsIdentity(claims, Options.AuthenticationScheme);
                var userprincipal = new ClaimsPrincipal(userIdentity);               

                return AuthenticateResult.Success(new AuthenticationTicket(userprincipal, null, Options.AuthenticationScheme));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
