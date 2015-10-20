using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contests.App.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;

    public class FileController : BaseController
    {
        public FileController(IContestsData data)
            : base(data)
        {
        }

        public ActionResult Index(int id)
        {
            var fileToRetrieve = this.ContestsData.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}
