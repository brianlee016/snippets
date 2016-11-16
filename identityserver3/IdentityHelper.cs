using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace !ChangeThisNamespace!
{
    public static class IdentityHelper
    {
        public class Profile
        {
            public string Email { get; set; }
            public string DisplayName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NtName { get; set; }
        }

        private static ClaimsIdentity User { get; set; }

        private static ClaimsIdentity Get()
        {
            try
            {
                return (Thread.CurrentPrincipal.Identity as ClaimsIdentity);
            }
            catch(Exception e)
            {
                throw new InvalidOperationException("Unable to cast the authenticated user to ClaimsIdentity");
            }
            
        }

        private static void SetUser()
        {
            User = Get();
        }

        public static List<string> GetRoles()
        {
            if (User != null)
            {
                try
                {
                    return User.FindAll("role").ToList().ConvertAll(x => x.Value);
                }
                catch (Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find user roles in claims identity for authenticated user.");
                }
            }
            return null;
        }

        public static Profile GetProfile()
        {
            SetUser();
            try
            {
                return new Profile
                {
                    NtName = GetUserNtName(),
                    LastName = GetLastName(),
                    FirstName = GetFirstName(),
                    Email = GetEmail(),
                    DisplayName = GetUserDisplayName()
                };
            }
            catch(Exception e)
            {
                throw new InvalidOperationException($"Encountered issue while getting user profile from token: {e.Message}");
            }
        }

        private static string GetUserNtName()
        {
            if(User != null)
            {
                try
                {
                    return User.FindFirst("nt_name").Value;
                }
                catch (Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find 'nt_name' in claims identity for authenticated user.");
                }
            }
            return null;
        }

        private static string GetEmail()
        {
            if(User != null)
            {
                try
                {
                    return User.FindFirst("email").Value;
                }
                catch(Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find 'email' in claims identity for authenticated user.");
                }
            }
            return null;
        }

        private static string GetFirstName()
        {
            if(User != null)
            {
                try
                {
                    return User.FindFirst("given_name").Value;
                }
                catch(Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find 'given_name' in claims identity for authenticated user.");
                }
            }
            return null;
        }

        private static string GetLastName()
        {
            if (User != null)
            {
                try
                {
                    return User.FindFirst("family_name").Value;
                }
                catch (Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find 'family_name' in claims identity for authenticated user.");
                }
            }
            return null;
        }

        public static string GetUserDisplayName()
        {
            if (User != null)
            {
                try
                {
                    return User.FindFirst("name").Value;
                }
                catch (Exception e)
                {
                    throw new ArgumentOutOfRangeException("Unable to find 'name' in claims identity for authenticated user.");
                }
            }
            return null;
        }

        public static string GetUserGuid()
        {
            if(User != null)
            {
                try
                {
                    return User.FindFirst("sub").Value;
                }
                catch(Exception e)
                {

                }

            }
            return null;
        }
    }
}