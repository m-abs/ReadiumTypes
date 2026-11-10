#!/usr/bin/env bash
# Release: bump <Version> in the csproj, commit, tag vX.Y.Z, push.
# The tag triggers publish.yml (nupkg to GitHub Packages + release asset).
set -euo pipefail
cd "$(dirname "$0")/.."
[[ $# -eq 1 ]] || { echo "usage: bin/release.sh major|minor|patch|X.Y.Z" >&2; exit 1; }
CSPROJ=src/ReadiumTypes/ReadiumTypes.csproj
bump=$1
current=$(sed -n 's|.*<Version>\(.*\)</Version>.*|\1|p' "$CSPROJ")

if [[ $bump == major || $bump == minor || $bump == patch ]]; then
  IFS=. read -r major minor patch <<<"$current"
  case $bump in
    major) major=$((major + 1)); minor=0; patch=0 ;;
    minor) minor=$((minor + 1)); patch=0 ;;
    patch) patch=$((patch + 1)) ;;
  esac
  next="$major.$minor.$patch"
else
  next=$bump
fi

die() { echo "release: $*" >&2; exit 1; }
[[ $next =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]] || die "bad version '$next' (from '$bump', current $current)"
[[ $next != "$current" ]] || die "already at $next"
[[ -z $(git tag -l "v$next") ]] || die "tag v$next already exists"
[[ -z $(git status --porcelain) ]] || die "working tree dirty; commit first"

dotnet test tests/ReadiumTypes.Tests -v q --nologo

sed -i "s|<Version>$current</Version>|<Version>$next</Version>|" "$CSPROJ"
git add "$CSPROJ"
git commit -m "bump version to $next"
git tag "v$next"
git push origin HEAD "v$next"
echo "released v$next — watch: gh run list --workflow publish --limit 1"
