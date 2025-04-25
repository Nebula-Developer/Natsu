#!/bin/bash

echo "Copying Natsu.Templates/content/ as ./LocalTemplate/"
rm -rf ./LocalTemplate
cp -r ./Natsu.Templates/content/ ./LocalTemplate/

cd ./LocalTemplate/NatsuApp
dotnet add reference ../../Natsu/Natsu.csproj

cd ../NatsuApp.Desktop
dotnet add reference ../../Natsu.Desktop/Natsu.Desktop.csproj

cd ../../
echo "Done!"
