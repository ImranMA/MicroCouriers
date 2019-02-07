FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Services/Tracking/Tracking.API/Tracking.API.csproj", "src/Services/Tracking/Tracking.API/"]
RUN dotnet restore "src/Services/Tracking/Tracking.API/Tracking.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Tracking/Tracking.API"
RUN dotnet build "Tracking.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Tracking.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Tracking.API.dll"]