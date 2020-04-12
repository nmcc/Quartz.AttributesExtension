# Contributing to this project

TBD

## Releasing a package

1. Update the version number on `src/Quartz.AttributesExtension/Quartz.AttributesExtension.csproj`
1. run `dotnet pack src/Quartz.AttributesExtension/Quartz.AttributesExtension.csproj -c Release
1. run `dotnet nuget push src/Quartz.AnnotationsExtension/bin/Release/Quartz.AttributesExtension.1.0.0.nupkg -s https://nuget.pkg.github.com/nmcc/index.json -k $GITHUB_
TOKEN`
