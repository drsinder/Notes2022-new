using System;
using System.Collections.Generic;
using System.Text;

namespace Notes2022.Shared
{
    public enum Commands
    {
        CloseMe,
        Show,
        Dialog,
        Refresh,
        CloseMeAndRefresh,
        Home,
        UpdateTopMenu
    }

    public enum Locations
    {
        Home,
        Login,
        Logout,
        Register,
        FileUserIndex,
        MainHelp,
        About,
        RecentEdit,
        Subscriptions,
        Preferences,
        License,
        AdminFiles,
        Roles,
        Linked,
        CreateLinked,
        EditLinked,
        FileList,
        Temp
    }

    public class Message
    {
        public Commands Command { get; set; }
        public Locations Location { get; set; }
        public int IntArg { get; set; }
        public string StringArg { get; set; }
    }
}
