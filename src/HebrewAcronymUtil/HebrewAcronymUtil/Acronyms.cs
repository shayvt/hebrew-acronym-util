using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace HebrewAcronymUtil;

internal class Acronyms : IEnumerable<KeyValuePair<string, string>>
{
    private Dictionary<string, string> _acronyms = new();

    public required AcronymCategory Category { get; init; }

    internal async Task Load()
    {
        var thisAssembly = Assembly.GetExecutingAssembly();

        var resourceName =
            $"{GetType().Namespace}.Resources.{Enum.GetName(typeof(AcronymCategory), Category)?.ToLower()}.json";

        await using var stream = thisAssembly.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            return;
        }

        _acronyms = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream) ??
                    new Dictionary<string, string>();
    }

    public string? ConvertAcronymToWord(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return _acronyms.GetValueOrDefault(acronym);
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _acronyms.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}