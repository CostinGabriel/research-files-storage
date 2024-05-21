using System.ComponentModel;

namespace ResearchFilesStorage.Domain.Constants;

public enum Extension
{
    [Description(".txt")]
    Text,
    [Description(".pdf")]
    Pdf,
    [Description(".doc")]
    Word
}
