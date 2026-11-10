#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
dotnet test tests/ReadiumTypes.Tests -v q --nologo /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=coverage/
reportgenerator -reports:tests/ReadiumTypes.Tests/coverage/coverage.cobertura.xml -targetdir:tests/ReadiumTypes.Tests/coverage/report
echo "report: tests/ReadiumTypes.Tests/coverage/report/index.html"
