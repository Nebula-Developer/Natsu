.PHONY: publish

publish:
	@echo "Publishing to ./publish"
	@rm -rf ./publish/*
	@dotnet pack -c Release -o ./publish
