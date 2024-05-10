using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class Acronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected Dictionary<string, string> AcronymsDict = new();
    
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return AcronymsDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}