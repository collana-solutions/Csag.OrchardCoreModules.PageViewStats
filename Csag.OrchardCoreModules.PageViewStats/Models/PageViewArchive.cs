// YesSql stores this document with its full type name and assembly name and finds it again by
// that name. Sites hold their page view archive under this name, so the namespace must not change.
namespace Neolution.OrchardCoreModules.PageViewStats.Models;

using System.Collections.Generic;
using Csag.OrchardCoreModules.PageViewStats.Models;
using OrchardCore.Data.Documents;

public class PageViewArchive : Document
{
    public string TimeZoneId { get; set; }

    public IList<DailyArchive> DailyArchives { get; set; } = new List<DailyArchive>();
}