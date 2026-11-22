# Csag.OrchardCoreModules.PageViewStats

Record and analyze page views of your visitors without having to give your data to a third party.

An [Orchard Core](https://orchardcore.net/) 1.x module for .NET 6. It counts the views of content items, archives them per day and shows the statistics in a dashboard and on the content items in the admin.

## Installation

```bash
dotnet add package Csag.OrchardCoreModules.PageViewStats
```

Reference the package in your Orchard Core application, then enable the **PageViewStats** feature under **Configuration → Features**. In recipes, enable it by its ID:

```json
{
  "name": "feature",
  "enable": ["Neolution.OrchardCoreModules.PageViewStats"]
}
```

The settings are under **Page Views → Settings**, the statistics under **Page Views → Dashboard**.

### Module ID

Orchard Core takes the module ID from the assembly name, and sites store it as the ID of the enabled feature. The assembly is therefore still named `Neolution.OrchardCoreModules.PageViewStats`, and so are the feature ID, the MVC area of the routes and the full names of the types whose names a site stores: the data migration, the background task and the page view archive document. Sites that ran the module under its previous package ID keep it enabled, keep their data and continue at their recorded migration version.

## Preview packages

Each pull request to `main` or to a `release/**` branch publishes the package to a public preview feed on feedz.io. A draft PR, a PR from a fork and a PR from Dependabot publish no preview. The version is one patch above the current release, for example `1.0.1-pr.42.183` (PR 42, workflow run 183). The **"chore: version packages"** PR publishes a release candidate of the upcoming version instead, for example `1.1.0-pr.73.190`, so the changes of the next release can be tested before it ships. In prerelease mode the version in `package.json` is a prerelease such as `2.0.0-rc.0`, and the preview extends it, for example `2.0.0-rc.0.pr.42.183`. That version sorts after the prerelease and before the next one. A comment on the PR gives the version of its latest push. The feed removes previews after 30 days. Releases go to nuget.org only.

To use a preview in an application, add the feed to the `nuget.config` of the application. The feed needs no credentials:

```xml
<packageSources>
  <clear />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  <add key="csag-orchardcoremodules-pageviewstats-previews" value="https://f.feedz.io/collana-solutions/csag-orchardcoremodules-pageviewstats/nuget/index.json" />
</packageSources>
<packageSourceMapping>
  <packageSource key="nuget.org">
    <package pattern="*" />
    <package pattern="Csag.OrchardCoreModules.PageViewStats" />
  </packageSource>
  <packageSource key="csag-orchardcoremodules-pageviewstats-previews">
    <package pattern="Csag.OrchardCoreModules.PageViewStats" />
  </packageSource>
</packageSourceMapping>
```

The `Csag.OrchardCoreModules.PageViewStats` pattern is also necessary under nuget.org. NuGet looks for a package only in the sources of its most specific pattern, so without it the released versions do not restore.

To test a preview:

1. Set `Csag.OrchardCoreModules.PageViewStats` to the preview version.
2. If the application restores in locked mode, update its `packages.lock.json` files.
3. Add the feed only on the branch that tests the preview. The main branch of the application then does not need feedz.io to restore.

## Build and Test

### Prerequisites

- The .NET SDK pinned in [global.json](global.json) (10.0.4xx, a newer 10.0 SDK rolls forward). It builds the `net6.0` target.
- The .NET 6.0 runtime, to run the tests.
- Node.js 24 with Corepack, for the changesets tooling: run `corepack enable` and then `pnpm install` once in the repository root.

### Build

```bash
dotnet restore --locked-mode
dotnet build --configuration Release --no-restore
```

Every project has a `packages.lock.json`, and CI restores in locked mode. After changing a package reference, run `dotnet restore` without `--locked-mode` and commit the updated lock files.

### Run Tests

```bash
dotnet test --configuration Release --no-build
```

### Pack

```bash
dotnet pack --configuration Release --no-build -o ./nupkgs
```

## Release process

This repository follows the [collana-solutions release playbook](https://github.com/collana-solutions/release-playbook) (Changesets, NuGet variant):

1. Every PR that changes the package needs a changeset. Run `pnpm changeset`, choose the bump type and describe the change for consumers of the package. CI's **Changeset Check** fails a PR that changes the project folder, `Directory.Build.props`, `Directory.Packages.props` or `global.json` without a changeset. Add an empty one (`pnpm changeset --empty`) when such a change ships nothing. A PR that only touches CI or repository documentation needs none.
2. On merge to `main`, the Release workflow keeps a **"chore: version packages"** PR up to date. It bumps the version in `Csag.OrchardCoreModules.PageViewStats/package.json`, writes it into the `.csproj` with `scripts/sync-versions.mjs` and updates the `CHANGELOG.md`.
3. Merging that PR creates the git tag and the GitHub Release, which start the **NuGet Publish** workflow. It packs the package and pushes it to nuget.org through trusted publishing.

Dependabot PRs get their changesets automatically. Do not edit the `CHANGELOG.md` or the `<Version>` in the `.csproj` by hand.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
