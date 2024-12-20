﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace HebrewAcronymUtil;

public interface IHebrewAcronyms
{
    string? ConvertAcronymToWords(string acronym);

    public string? ConvertAcronymWithPrefixToWords(string acronym);

    public Task Initialize(params HashSet<string> ignoredAcronyms);
}