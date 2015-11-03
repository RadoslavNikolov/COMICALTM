namespace Contests.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
    using Data.UnitOfWork;
    using Infrastructure;
    using Models.BindingModels;
    using Models.ViewModels;
    using MvcPaging;
    using Ninject.Infrastructure.Language;
    using Toastr;

    public class ContestsController : BaseAdminController
    {
        public ContestsController(IContestsData data)
            : base(data)
        {
        }

        // GET: Admin/Contests
        public ActionResult Index(string contest_title, int? page)
        {
            this.TempData["contest_title"] = contest_title;
            int currentPageIndex = page ?? 1;

            var contestsQuery = this.ContestsData.Contests.All();

            if (!string.IsNullOrWhiteSpace(contest_title))
            {
                contestsQuery = contestsQuery.Where(c => c.Title.Contains(contest_title));
            }

            Mapper.CreateMap<Contest, ContestPreviewViewModel>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.Organizator, opts => opts.MapFrom(src => src.Organizator.FullName))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category.Name));

            var contests = contestsQuery
                    .OrderBy(c => c.Title)
                    .Project()
                    .To<ContestPreviewViewModel>()
                    .ToPagedList(currentPageIndex, AppConfig.AdminPanelPageSize);

            return View(contests);
        }

        // GET: Admin/Contests/Edit/id
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Mapper.CreateMap<Contest, ContestBindingModel>()
                .ForMember(dest => dest.Participants, opts => opts.MapFrom(src => src.Participants.Select(p => p.Id)))
                .ForMember(dest => dest.Voters, opts => opts.MapFrom(src => src.Voters.Select(v => v.Id)));

            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestBindingModel>()
                .FirstOrDefault();

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            return View(contest);
        }
    }
}