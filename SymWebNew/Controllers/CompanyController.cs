using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        BranchRepo branchRepo = new BranchRepo();
        ShampanIdentityVM vm = new ShampanIdentityVM();

        public ActionResult Index()
        {
            List<BranchVM> company = branchRepo.SelectAll();
            return View(company);
        }
        public ActionResult Select(string Id)
        {
            Session["BranchId"] = Id;
            vm.BranchId = Id;

            return Redirect("/Common/Home");
        }

    }
}
