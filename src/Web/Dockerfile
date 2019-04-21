
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY *.sln .
COPY . .
WORKDIR /app/src/Web
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/src/Web/out ./



ENTRYPOINT ["dotnet", "Web.dll"]
