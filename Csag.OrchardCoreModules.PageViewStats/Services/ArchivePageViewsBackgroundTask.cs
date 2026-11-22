// Orchard Core stores the settings of a background task, such as its schedule or whether it runs,
// under the full type name of the task. Sites may hold settings under this name, so the namespace
// must not change.
namespace Neolution.OrchardCoreModules.PageViewStats.Services;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Csag.OrchardCoreModules.PageViewStats.Extensions;
using Csag.OrchardCoreModules.PageViewStats.Models;
using Csag.OrchardCoreModules.PageViewStats.Queries;
using Csag.OrchardCoreModules.PageViewStats.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Neolution.OrchardCoreModules.PageViewStats.Models;
using OrchardCore.BackgroundTasks;
using OrchardCore.Documents;
using OrchardCore.Settings;

[BackgroundTask(Schedule = "0 0 * * *", Description = "Generate daily summaries for page view statistics for faster access and to save space.")]
//[BackgroundTask(Schedule = "*/1 * * * *", Description = "DEBUG SCHEDULE")]
public class ArchivePageViewsBackgroundTask : IBackgroundTask
{
    public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Debugger.Break();
        var siteService = serviceProvider.GetRequiredService<ISiteService>();
        var settings = await siteService.GetSiteSettingsAsync();

        var documentManager = serviceProvider.GetRequiredService<IDocumentManager<PageViewArchive>>();
        var document = await documentManager.GetOrCreateMutableAsync();
        if (string.IsNullOrWhiteSpace(document.TimeZoneId))
        {
            document.TimeZoneId = settings.TimeZoneId;
        }
        else if (document.TimeZoneId != settings.TimeZoneId)
        {
            // TODO: Handle changed TimeZone setting
            return;
        }

        var aggregateService = serviceProvider.GetRequiredService<IAggregateService>();

        var sender = serviceProvider.GetRequiredService<ISender>();
        var availablePageViewDates = await sender.Send(new GetAvailablePageViewDatesQuery(), cancellationToken);

        var today = DateOnlyExtensions.Today(settings);
        var threshold = today.AddDays(-3);
        foreach (var availablePageViewDate in availablePageViewDates)
        {
            if (availablePageViewDate == today)
            {
                // Do not archive page views of the current day
                continue;
            }

            var archive = document.DailyArchives.FirstOrDefault(x => x.Id == availablePageViewDate.ToString("yyyy-MM-dd"));
            if (archive == null)
            {
                archive = await aggregateService.CreateAggregateAsync(availablePageViewDate);
                document.DailyArchives.Add(archive);
            }
            else
            {
                if (DateOnly.ParseExact(archive.Id, "yyyy-MM-dd") > threshold)
                {
                    document.DailyArchives.Remove(archive);
                    archive = await aggregateService.CreateAggregateAsync(availablePageViewDate);
                    document.DailyArchives.Add(archive);
                }
            }
        }

        document.DailyArchives = document.DailyArchives.OrderByDescending(x => x.Id).ToList();
        await documentManager.UpdateAsync(document);
    }
}