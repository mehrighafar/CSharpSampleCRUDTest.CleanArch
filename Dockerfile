FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore --no-cache --verbosity minimal
RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release -o /app/publish --no-restore

# If your docker works fine, it's not necessary to copy the dlls seperately.
# Mine is not working, so I had to copy it.
COPY Dlls/. /app/publish/

COPY otel-collector-config.yaml /etc/otel-collector-config.yaml
COPY loki-config.yaml /etc/loki/loki-config.yaml
COPY prometheus.yaml /etc/prometheus/prometheus.yaml

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CSharpSampleCRUDTest.CleanArch.Web.dll"]
