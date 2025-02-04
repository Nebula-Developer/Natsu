#!/bin/bash

# This is a pretty messy way of doing things, but it works for now.
# Templates won't change very often, especially when the foundations of the framework are set (and there aren't constant breaking changes).

# 1. Copy Natsu.Templates/content/ as ./LocalTemplate/
echo "Copying Natsu.Templates/content/ as ./LocalTemplate/"
rm -rf ./LocalTemplate
cp -r ./Natsu.Templates/content/ ./LocalTemplate/

# 2. Inside of ./LocalTemplate/NatsuApp.App/NatsuApp.App.csproj and ./LocalTemplate/NatsuApp.Desktop/NatsuApp.Desktop.csproj, replace <PackageReference>s with <ProjectReference>s
echo "Replacing PackageReference with ProjectReference in ./LocalTemplate/NatsuApp.App/NatsuApp.App.csproj"
sed -i 's/<PackageReference Include="Natsu" Version="_NATSU_VERSION_" \/>/<ProjectReference Include="..\/..\/Natsu\/Natsu.csproj" \/>/g' ./LocalTemplate/NatsuApp.App/NatsuApp.App.csproj
echo "Replacing PackageReference with ProjectReference in ./LocalTemplate/NatsuApp.Desktop/NatsuApp.Desktop.csproj"
sed -i 's/<PackageReference Include="Natsu.Desktop" Version="_NATSU_VERSION_" \/>/<ProjectReference Include="..\/..\/Natsu.Desktop\/Natsu.Desktop.csproj" \/>/g' ./LocalTemplate/NatsuApp.Desktop/NatsuApp.Desktop.csproj

echo "Done!"
