using AuctionService.Data;
using AuctionService.Entities;
using AuctionService.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carsties.Shared.Contracts;
using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Services;

public class AuctionService : IAuctionService
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AuctionDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionService(ILogger logger,
        IMapper mapper,
        AuctionDbContext context,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<IReadOnlyList<AuctionDto>>> GetAllAuctions(string date, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - get all auctions");

        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();
        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        var auctions = (IReadOnlyList<AuctionDto>)await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        if (!auctions.Any())
        {
            _logger.Here().Warning("No auctions found in database");
            return Result<IReadOnlyList<AuctionDto>>.Failure(ErrorCodes.NotFound);
        }

        _logger.Here().Information("Total {@count} auctions found", auctions.Count);
        _logger.Here().MethodExited();
        return Result<IReadOnlyList<AuctionDto>>.Success(auctions);
    }

    public async Task<Result<AuctionDto>> GetAuction(string id, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - get all auctions");

        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

        if (auction is null)
        {
            _logger.Here().WithCorrelationId(correlationId)
                .Warning("No auction was found in database");
            return Result<AuctionDto>.Failure(ErrorCodes.NotFound);
        }

        var result = _mapper.Map<AuctionDto>(auction);

        _logger.Here().WithCorrelationId(correlationId)
            .Information("Actions found {@auction}", result);
        _logger.Here().MethodExited();
        return Result<AuctionDto>.Success(result);
    }

    public async Task<Result<AuctionDto>> CreateAuction(CreateAuctionDto createAuction, RequestInformation requestInformation)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .WithCorrelationId(requestInformation.CorrelationId)
            .Information("Request -  create new auction {@auction}", createAuction);

        var auctionEntity = _mapper.Map<Auction>(createAuction);
        auctionEntity.Seller = requestInformation.CurrentUser.Username;

        _context.Add(auctionEntity);
        var newAuctionDto = _mapper.Map<AuctionDto>(auctionEntity);

        // public auction create event
        await PublishMessage<AuctionDto, AuctionCreated>(newAuctionDto, requestInformation.CorrelationId);

        var createResult = await _context.SaveChangesAsync() > 0;

        if (!createResult)
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Error("{category} - Could not save changes to the db", ErrorCodes.OperationFailed);
            return Result<AuctionDto>.Failure(ErrorCodes.OperationFailed);
        }

        _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Information("New auction created successfully with id {id}", auctionEntity.Id);
        _logger.Here().MethodExited();
        return Result<AuctionDto>.Success(_mapper.Map<AuctionDto>(auctionEntity));
    }

    public async Task<Result<bool>> UpdateAuction(string id, UpdateAuctionDto updateAuction, RequestInformation requestInformation)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
            .Information("Request - update auction of {id} - {@updateAuction}", id, updateAuction);

        var auctionEntity = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

        if (auctionEntity.Seller != requestInformation.CurrentUser.Username)
        {
            _logger.Here()
                .WithCorrelationId(requestInformation.CorrelationId)
                .Error("current user does not have authority to update the auction for {make}", auctionEntity.Item.Make);
            return Result<bool>.Failure(ErrorCodes.Unauthorized);
        }

        if (auctionEntity is null)
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId)
                .Warning("No auction found with id {id}", id);
            return Result<bool>.Failure(ErrorCodes.NotFound);
        }

        _mapper.Map(updateAuction, auctionEntity, typeof(UpdateAuctionDto), typeof(Auction));
        _context.Entry(auctionEntity).State = EntityState.Modified;

        await PublishMessage<Auction, AuctionUpdated>(auctionEntity, requestInformation.CorrelationId);
        var updateResult = await _context.SaveChangesAsync() > 0;

        if (!updateResult)
        {
            _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Error("{category} - Could not save changes to the db", ErrorCodes.OperationFailed);
            return Result<bool>.Failure(ErrorCodes.OperationFailed);
        }

        _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Information("Auction updated successfully");
        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }

    private async Task PublishMessage<T, TEvent>(T newAuction, string correlationId)
    {
        var newEvent = _mapper.Map<TEvent>(newAuction);
        await _publishEndpoint.Publish(newEvent);
        _logger.Here()
            .WithCorrelationId(correlationId)
            .Information("Successfully publihed auction event message");
    }
}
