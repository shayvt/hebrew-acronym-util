﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class HebrewAcronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected readonly Dictionary<string, string> AcronymsDict = [];

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return AcronymsDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string? ConvertAcronymToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        return AcronymsDict.GetValueOrDefault(acronym);
    }

    public string? ConvertAcronymWithPrefixToWords(string acronym)
    {
        if (acronym is null)
        {
            throw new ArgumentNullException(nameof(acronym));
        }

        var converted = ConvertAcronymToWords(acronym);

        if (converted is not null)
        {
            return converted;
        }

        foreach (var (prefix, acronymWithoutPrefix) in HebrewAcronymUtils.ExtractWordPrefix(acronym))
        {
            if (prefix is null)
            {
                return null;
            }

            var convertedWithoutPrefix = ConvertAcronymToWords(acronymWithoutPrefix);

            if (convertedWithoutPrefix is null)
            {
                continue;
            }

            return $"{prefix}{convertedWithoutPrefix}";
        }

        return null;
    }
}