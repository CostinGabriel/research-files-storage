using System.ComponentModel;

namespace ResearchFilesStorage.Application.Enums;

public enum Extension
{
    [Description(".txt")]
    Text,
    [Description(".pdf")]
    Pdf,
    [Description(".doc")]
    Word
}
