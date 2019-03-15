FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Web/MicroCourier.Web/MicroCourier.Web.csproj", "src/Web/MicroCourier.Web/"]
RUN dotnet restore "src/Web/MicroCourier.Web/MicroCourier.Web.csproj"
COPY . .
WORKDIR "/src/src/Web/MicroCourier.Web"
RUN dotnet build "MicroCourier.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroCourier.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroCourier.Web.dll"]