FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Services/Booking/Booking.API/Booking.API.csproj", "src/Services/Booking/Booking.API/"]
RUN dotnet restore "src/Services/Booking/Booking.API/Booking.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Booking/Booking.API"
RUN dotnet build "Booking.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Booking.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Booking.API.dll"]