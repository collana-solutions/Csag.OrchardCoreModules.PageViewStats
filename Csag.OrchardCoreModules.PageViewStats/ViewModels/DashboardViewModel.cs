namespace Csag.OrchardCoreModules.PageViewStats.ViewModels;

using System.Collections.Generic;
using Csag.OrchardCoreModules.PageViewStats.Models;

public class DashboardViewModel
{
    public int History { get; set; }
    public IList<DailyArchive> PageViewsByDay { get; set; }
    public List<PageViewsPerContentItem> PageViewsByContentItem { get; set; }
}