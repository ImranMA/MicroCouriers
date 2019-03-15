FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src


COPY ["Booking.API/Booking.API.csproj", "Booking.API/"]
COPY ["Booking.Application/Booking.Application.csproj", "Booking.Application/"]
COPY ["Booking.Common/Booking.Common.csproj", "Booking.Common/"]
COPY ["Booking.Domain/Booking.Domain.csproj", "Booking.Domain/"]
COPY ["Booking.Infrastructure/Booking.Infrastructure.csproj", "Booking.Infrastructure/"]
COPY ["Booking.Persistence/Booking.Persistence.csproj", "Booking.Persistence/"]

RUN dotnet restore "Booking.API/Booking.API.csproj"


COPY . .


WORKDIR "/src/Booking.API"


RUN dotnet build "Booking.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Booking.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .


ENTRYPOINT ["dotnet", "Booking.API.dll"]