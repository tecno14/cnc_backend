using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class UserSessionExtendedDataUpdateNotificationCommand
    {
        public static List<Tdf> UserSessionExtendedDataUpdateNotification(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                BlazeHelper.CreateUserDataStruct(pi),
                TdfInteger.Create("USID", pi.UserId)
            };
            return Result;
        }
    }
}
