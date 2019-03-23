FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5002

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src


COPY ["Tracking.API/Tracking.API.csproj", "Tracking.API/"]
COPY ["Tracking.Application/Tracking.Application.csproj", "Tracking.Application/"]
COPY ["Tracking.Common/Tracking.Common.csproj", "Tracking.Common/"]
COPY ["Tracking.Domain/Tracking.Domain.csproj", "Tracking.Domain/"]
COPY ["Tracking.Persistence/Tracking.Persistence.csproj", "Tracking.Persistence/"]

RUN dotnet restore "Tracking.API/Tracking.API.csproj"


COPY . .


WORKDIR "/src/Tracking.API"


RUN dotnet build "Tracking.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Tracking.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .


ENTRYPOINT ["dotnet", "Tracking.API.dll"]