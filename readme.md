# Hebrew Acronym Util
This is a C# utility library for handling Hebrew acronyms.

## Usage
To use this library, create an instance of `CategorizedHebrewAcronyms`, set the `Categories` property with the desired categories, and then call the `Load` method to load the acronyms. You can then use the `ConvertAcronymToWords` method to convert acronyms to words.

```csharp
CategorizedHebrewAcronyms acronyms = new()
        {
            Categories =
            [
                AcronymCategory.Common,
                AcronymCategory.Judaism
            ]
        };
await acronyms.Initialize();
var words = acronyms.ConvertAcronymToWords("""בנ"א""");
Console.WriteLine(words); // בני אדם
```

