﻿namespace Microsoft.DocAsCode.MarkdownLite
{
    using System.Text.RegularExpressions;

    public class MarkdownBlockquoteBlockRule : IMarkdownRule
    {
        public string Name => "Blockquote";

        public virtual Regex Blockquote => Regexes.Block.Blockquote;

        public virtual IMarkdownToken TryMatch(MarkdownEngine engine, ref string source)
        {
            var match = Blockquote.Match(source);
            if (match.Length == 0)
            {
                return null;
            }
            source = source.Substring(match.Length);
            var capStr = Regexes.Lexers.LeadingBlockquote.Replace(match.Value, string.Empty);
            return new MarkdownBlockquoteBlockToken(this, engine.Tokenize(capStr));
        }
    }
}
