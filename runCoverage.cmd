dotnet test --collect:"XPlat Code Coverage" -r "_codeCoverage"
reportgenerator -reports:"_codecoverage/7e2f9c84-f8db-402d-a30a-c193a12b35f2/coverage.cobertura.xml" -targetdir:"_codecoverage/reports"