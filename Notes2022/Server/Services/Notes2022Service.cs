using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
//using ProtoBuf.Grpc;

//namespace Notes2022.Server.Services
//{
//    public class Notes2022Service : INotes2022Service
//    {
//        private readonly NotesDbContext _db;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        public Notes2022Service(NotesDbContext db,
//            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
//            )
//        {
//            _db = db;
//            _userManager = userManager;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public ValueTask<AboutModel> GetAbout()
//        {
//            AboutModel model = new AboutModel
//            {
//                PrimeAdminName = Globals.PrimeAdminName,
//                PrimeAdminEmail = Globals.PrimeAdminEmail,
//                StartupDateTime = Globals.StartupDateTime
//            };

//            return ValueTask.FromResult(model);
//        }

//        //public ValueTask<HomePageModel> GetAdminPageData()
//        //{
//        //    HomePageModel model = new HomePageModel();

//        //    string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
//        //    ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
//        //    bool test = _userManager.IsInRoleAsync(user, "Admin").GetAwaiter().GetResult();
//        //    if (!test)
//        //        return ValueTask.FromResult(model);

//        //    NoteFile hpmf = _db.NoteFile.Where(p => p.NoteFileName == "homepagemessages").FirstOrDefault();
//        //    if (hpmf is not null)
//        //    {
//        //        NoteHeader hpmh = _db.NoteHeader.Where(p => p.NoteFileId == hpmf.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault();
//        //        if (hpmh is not null)
//        //        {
//        //            model.Message = _db.NoteContent.Where(p => p.NoteHeaderId == hpmh.Id).FirstOrDefault().NoteBody;
//        //        }
//        //    }

//        //    model.NoteFiles = _db.NoteFile
//        //        .OrderBy(p => p.NoteFileName).ToList();

//        //    model.NoteAccesses = new List<NoteAccess>();

//        //    List<ApplicationUser> udl = _db.Users.ToList();

//        //    model.UserListData = new List<UserData>();
//        //    foreach (ApplicationUser userx in udl)
//        //    {
//        //        UserData ud = NoteDataManager.GetUserData(userx);
//        //        model.UserListData.Add(ud);
//        //    }

//        //    try
//        //    {
//        //        if (!string.IsNullOrEmpty(userId))
//        //        {
//        //            model.UserData = NoteDataManager.GetUserData(user);

//        //            foreach (NoteFile nf in model.NoteFiles)
//        //            {
//        //                NoteAccess na = AccessManager.GetAccess(_db, user.Id, nf.Id, 0).GetAwaiter().GetResult();
//        //                model.NoteAccesses.Add(na);
//        //            }
//        //        }
//        //        else
//        //        {
//        //            model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
//        //        }
//        //    }
//        //    catch
//        //    {
//        //        model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
//        //    }

//        //    return ValueTask.FromResult(model);

//        //}


//        //public ValueTask<HomePageModel> GetHomePageData()
//        //{
//        //    HomePageModel model = new HomePageModel();

//        //    model.Message = string.Empty;
//        //    NoteFile hpmf = _db.NoteFile.Where(p => p.NoteFileName == "homepagemessages").FirstOrDefault();
//        //    if (hpmf is not null)
//        //    {
//        //        NoteHeader hpmh = _db.NoteHeader.Where(p => p.NoteFileId == hpmf.Id && p.IsDeleted == false).OrderByDescending(p => p.CreateDate).FirstOrDefault();
//        //        if (hpmh is not null)
//        //        {
//        //            model.Message = _db.NoteContent.Where(p => p.NoteHeaderId == hpmh.Id).FirstOrDefault().NoteBody;
//        //        }
//        //    }

//        //    model.NoteFiles = _db.NoteFile
//        //        .OrderBy(p => p.NoteFileName).ToList();

//        //    model.NoteAccesses = new List<NoteAccess>();

//        //    try
//        //    {
//        //        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

//        //        if (!string.IsNullOrEmpty(userId))
//        //        {
//        //            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
//        //            model.UserData = NoteDataManager.GetUserData(user);

//        //            foreach (NoteFile nf in model.NoteFiles)
//        //            {
//        //                NoteAccess na = AccessManager.GetAccess(_db, user.Id, nf.Id, 0).GetAwaiter().GetResult();
//        //                model.NoteAccesses.Add(na);
//        //            }

//        //            if (model.NoteAccesses.Count > 0)
//        //            {
//        //                NoteFile[] theList = new NoteFile[model.NoteFiles.Count];
//        //                model.NoteFiles.CopyTo(theList);
//        //                foreach (NoteFile nf2 in theList)
//        //                {
//        //                    NoteAccess na = model.NoteAccesses.SingleOrDefault(p => p.NoteFileId == nf2.Id);
//        //                    if (!na.ReadAccess && !na.Write && !na.EditAccess)
//        //                    {
//        //                        model.NoteFiles.Remove(nf2);
//        //                    }
//        //                }
//        //            }
//        //        }
//        //        else
//        //        {
//        //            model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
//        //        }
//        //    }
//        //    catch
//        //    {
//        //        model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
//        //    }

//        //    return ValueTask.FromResult(model);
//        //}

//    }
//}
