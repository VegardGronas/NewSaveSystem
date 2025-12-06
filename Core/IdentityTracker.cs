using System.Collections.Generic;

namespace WorldKeeper
{
    public static class IdentityTracker
    {
        private static List<Identity> identities = new();

        public static Identity[] GetIdentitiesAsArray()
        {
            return identities.ToArray();
        }

        public static List<Identity> GetIdentitiesAsList()
        {
            return identities;
        }

        public static bool Contains(Identity identity)
        {
            return identities.Contains(identity);
        }

        public static bool Contains(string uniqueID)
        {
            foreach (var identity in identities)
            {
                if (identity.UniqueID == uniqueID) return true;
            }
            return false;
        }

        public static void Register(Identity identity)
        {
            if (!identities.Contains(identity))
            {
                identities.Add(identity);
            }
        }

        public static void Unregister(Identity identity)
        {
            if (identities.Contains(identity))
            {
                identities.Remove(identity);
            }
        }
    }

}