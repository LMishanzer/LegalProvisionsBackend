﻿namespace LegalProvisionsLib.TextExtracting;

public interface ITextExtractable
{
    public IEnumerable<string> ExtractEntireText();
}