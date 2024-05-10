using System.Collections;
using System.Collections.Generic;

namespace HebrewAcronymUtil;

public abstract class Acronyms : IEnumerable<KeyValuePair<string, string>>
{
    protected Dictionary<string, string> _acronyms = new();
    
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _acronyms.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}