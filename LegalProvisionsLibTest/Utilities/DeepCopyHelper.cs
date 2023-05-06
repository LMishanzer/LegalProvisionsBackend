using Newtonsoft.Json;

namespace LegalProvisionsLibTest.Utilities;

public static class DeepCopyHelper
{
    public static T? DeepCopy<T>(T? obj)
    {
        var serialized = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}