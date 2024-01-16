FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

# Copy all .csproj files and restores as distinct layers. USE of the same COPY command
# for every dockerfile in the project to take advantage of docker chaining.
COPY Carsties.sln                                Carsties.sln
COPY src/AuctionService/AuctionService.csproj    src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj      src/SearchService/SearchService.csproj
COPY src/BiddingService/BiddingService.csproj      src/BiddingService/BiddingService.csproj
COPY src/GatewayService/GatewayService.csproj    src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj  src/IdentityService/IdentityService.csproj
COPY src/Carsties.Shared/Carsties.Shared.csproj  src/Carsties.Shared/Carsties.Shared.csproj

# Restore package dependencies
RUN dotnet restore Carsties.sln

# Copy the folders over
COPY src/AuctionService src/AuctionService
COPY src/Carsties.Shared src/Carsties.Shared
WORKDIR /app/src/AuctionService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "AuctionService.dll" ]

