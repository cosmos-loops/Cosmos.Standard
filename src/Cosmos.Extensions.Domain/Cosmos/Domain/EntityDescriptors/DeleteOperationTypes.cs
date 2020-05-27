namespace Cosmos.Domain.EntityDescriptors
{
    /// <summary>
    /// Delete operation types
    /// </summary>
    public enum DeleteOperationTypes
    {
        /// <summary>
        /// Logic delete, also named soft-delete.
        /// </summary>
        LogicDelete,

        /// <summary>
        /// Restore, be used to logic deleted entity.
        /// </summary>
        Restore,

        /// <summary>
        /// Physical delete, also named hard-delete.
        /// </summary>
        PhysicalDelete,
    }
}