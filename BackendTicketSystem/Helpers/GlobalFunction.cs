using BackendTicketSystem.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
namespace BackendTicketSystem.Helpers
{
    public class GlobalFunction
    {
        public static DateTime GetCurrentDateTime()
        {
            var now = DateTime.Now;
            var local = TimeZoneInfo.Local;
            var destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(now, local, destinationTimeZone);
        }

        public static object RenderErrorMessageFromState(ModelStateDictionary modelState)
        {
            return new
            {
                error = "Info(s):\n" + string.Join("\n- ", modelState.Values
                                      .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage))
            };
        }

        public static int GetCurrentUserId(BackendTicketSystemContext db = null, string token = "")
        {
            if (db == null)
            {
                db = new BackendTicketSystemContext();
            }
            var userIdList = db.UserAccountTokens.Where(x => x.Token == token).Select(x => x.UserAccountId).ToList();
            var currentUser = db.UserAccounts.Where(x => userIdList.Contains(x.Id)).ToList();
            return currentUser[0].Id;
        }
    }
}

