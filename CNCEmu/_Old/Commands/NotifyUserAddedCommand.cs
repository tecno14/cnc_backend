using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyUserAddedCommand
    {
        public static List<Tdf> NotifyUserAdded(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                BlazeHelper.CreateUserDataStruct(pi),
                BlazeHelper.CreateUserStruct(pi)
            };
            return Result;
        }
    }
}
