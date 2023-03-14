using Newtonsoft.Json;

namespace LegalProvisionsLib.Settings;

public class SettingsReader
{
    public ServerSettings ReadSettings(string path)
    {
        var text = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<ServerSettings>(text) ?? new ServerSettings();
    }
}