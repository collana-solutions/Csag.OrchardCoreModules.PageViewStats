---
"@collana-solutions/csag-orchardcoremodules-pageviewstats": major
---

The package moves to a new ID, and its namespaces move with it. The module and its behavior are the same as in 0.5.0.

**New package ID:**

| Up to 0.5.0 | From 1.0.0 |
| --- | --- |
| `Neolution.OrchardCoreModules.PageViewStats` | `Csag.OrchardCoreModules.PageViewStats` |

**To migrate:**

1. Replace the `Neolution.OrchardCoreModules.PageViewStats` package reference with `Csag.OrchardCoreModules.PageViewStats` at 1.0.0 or later. Both packages contain the same assembly, so remove the old reference instead of keeping both.
2. Code that uses types of the module: `using Neolution.OrchardCoreModules.PageViewStats.<...>;` becomes `using Csag.OrchardCoreModules.PageViewStats.<...>;`. Three types keep their namespace, because sites store their full names: `Neolution.OrchardCoreModules.PageViewStats.Migrations`, `Neolution.OrchardCoreModules.PageViewStats.Services.ArchivePageViewsBackgroundTask` and `Neolution.OrchardCoreModules.PageViewStats.Models.PageViewArchive`.
3. Translations of the admin menu in `.po` files use the type name as their context, so `Neolution.OrchardCoreModules.PageViewStats.Navigation.AdminMenu` and `.SettingsAdminMenu` become `Csag.OrchardCoreModules.PageViewStats.Navigation.AdminMenu` and `.SettingsAdminMenu`.

Existing sites need no further step. The assembly name stays `Neolution.OrchardCoreModules.PageViewStats`, so the module ID, the feature ID in recipes and in the enabled features, the routes, the recorded migration version, the background task settings and the stored page views, archive and settings are the same as before.

The `Neolution.OrchardCoreModules.PageViewStats` ID gets deprecated on nuget.org, with `Csag.OrchardCoreModules.PageViewStats` as the alternate. Its existing versions stay available, but it gets no new releases.
