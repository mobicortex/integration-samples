using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for managing Entities (People, Vehicles).
    /// </summary>
    public interface IEntityService
    {
        /// <summary>
        /// Lists entities of a specific registry.
        /// </summary>
        /// <param name="centralRegistryId">Central registry ID</param>
        Task<ApiResult<EntityListResponse>> ListByRegistryAsync(uint centralRegistryId);

        /// <summary>
        /// Lists all entities (paginated, optionally filtered by name).
        /// </summary>
        Task<ApiResult<EntityListResponse>> ListAllAsync(int offset = 0, int count = 10, string? name = null);

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        Task<ApiResult<Entity>> GetAsync(uint entityId);

        /// <summary>
        /// Creates a new entity.
        ///
        /// Returns:
        /// - Success=true, Data.Ret=0: entity created successfully.
        /// - Success=false, IsConflict=true (HTTP 409): entity already exists.
        ///   Resend with Overwrite=true to overwrite, or use UpdateAsync() for partial update.
        /// - Success=false, others: validation or server error.
        /// </summary>
        Task<ApiResult<CreateEntityResponse>> CreateAsync(CreateEntityRequest request);

        /// <summary>
        /// Updates an existing entity (PUT /entities?id=X).
        ///
        /// IMPORTANT: This endpoint performs a PARTIAL update. Use UpdateEntityRequest
        /// and fill ONLY the fields you want to modify (name/doc for person, doc/brand/model/color/lpr_enabled for vehicle).
        /// Unfilled fields (null) will not be changed on the server.
        ///
        /// Do NOT use the full Entity class here - it contains read-only fields
        /// (entity_id, central_registry_id, type, created_at) that cause a 400 error if sent.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> UpdateAsync(uint entityId, UpdateEntityRequest request);

        /// <summary>
        /// Removes an entity and all its media.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteAsync(uint entityId);

        /// <summary>
        /// Removes a vehicle entity by plate.
        /// DELETE /entities?plate=PLATE
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteByPlateAsync(string plate);

        /// <summary>
        /// Removes central registries with no linked entities.
        /// DELETE /entities/cleanup-orphans
        /// </summary>
        Task<ApiResult<CleanupOrphansResponse>> CleanOrphansAsync();

        /// <summary>
        /// Lists the drivers linked to a vehicle.
        /// </summary>
        Task<ApiResult<VehicleDriverListResponse>> GetVehicleDriversAsync(uint vehicleId);

        /// <summary>
        /// Replaces the list of drivers linked to a vehicle.
        /// </summary>
        Task<ApiResult<VehicleDriverUpdateResponse>> UpdateVehicleDriversAsync(uint vehicleId, IEnumerable<uint> driverIds);
    }
}
