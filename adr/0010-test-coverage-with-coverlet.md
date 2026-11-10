# 0010. Test coverage via coverlet.msbuild + reportgenerator

Status: Accepted

## Context

AudioCppClient measures test coverage with coverlet.msbuild (cobertura) plus
the `dotnet-reportgenerator-globaltool` for HTML reports, scripted in
`bin/coverage.sh` and a `coverage` GitHub Actions workflow. We want the same
here.

## Decision

Add `coverlet.msbuild` as a test-only `PackageReference`
(`PrivateAssets="all"`, version in `Directory.Packages.props`). HTML reports
are generated locally by `bin/coverage.sh` using the globally installed
`reportgenerator` tool — no new project dependency. CI (`coverage.yml`) runs
`dotnet test` with coverage collection, uploads the raw cobertura XML as an
artifact, and comments the summary on a PR when a `/coverage` comment triggers
the run.

## Consequences

The package never reaches package consumers (`PrivateAssets="all"` keeps it
out of the nuspec). HTML generation needs the global tool, so CI keeps plain
cobertura as the artifact.
