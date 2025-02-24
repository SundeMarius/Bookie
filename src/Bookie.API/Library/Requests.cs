namespace Bookie.API.Library;

public record UpdateInventoryRequest(Guid BookId, int NewCount);
