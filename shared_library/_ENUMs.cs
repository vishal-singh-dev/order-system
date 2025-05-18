namespace shared_library
{
    public class _ENUMs
    {
        public enum orderStatus
        {
            Placed = 0,
            Updated = 1,
            Prerparing = 2,
            Completed = 3,
            Served = 4,
            Billed = 5,
            Cancelled = 6
        }
        public enum orderType
        {
            newOrder = 0,
            orderUpdated = 1,
            orderCancelled=2,
        }
    }
}
