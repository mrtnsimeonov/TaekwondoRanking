using System.Collections.Concurrent;

namespace TaekwondoRanking.Helpers
{
    public static class TemporaryDeletionManager
    {
        // ✅ CHANGED: Use a string key to match the Athlete ID data type.
        public static readonly ConcurrentDictionary<string, byte> TemporarilyDeletedAthleteIds = new ConcurrentDictionary<string, byte>();
    }
}