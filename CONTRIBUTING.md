# Contributing to this project

## Releasing a package

1. Update the package version number on `<Version>` element on `src/Quartz.AttributesExtension/Quartz.AttributesExtension.csproj`
1. run `dotnet pack src/Quartz.AttributesExtension/Quartz.AttributesExtension.csproj -c Release`
1. run `dotnet nuget push src/Quartz.AttributesExtension/bin/Release/Quartz.AttributesExtension.<VERSION>.nupkg -s https://nuget.pkg.github.com/nmcc/index.json -k $GITHUB_TOKEN`
