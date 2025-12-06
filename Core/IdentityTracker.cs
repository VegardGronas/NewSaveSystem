using System.Collections.Generic;

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
