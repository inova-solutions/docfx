﻿namespace Microsoft.DocAsCode.MarkdownLite
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public class MarkdownTableBlockRule : IMarkdownRule
    {
        public string Name => "Table";

        public virtual Regex Table => Regexes.Block.Tables.Table;

        public virtual IMarkdownToken TryMatch(MarkdownEngine engine, ref string source)
        {
            var match = Table.Match(source);
            if (match.Length == 0)
            {
                return null;
            }
            source = source.Substring(match.Length);
            var header = match.Groups[1].Value.ReplaceRegex(Regexes.Lexers.UselessTableHeader, string.Empty).SplitRegex(Regexes.Lexers.TableSplitter);
            var align = ParseAligns(match.Groups[2].Value.ReplaceRegex(Regexes.Lexers.UselessTableAlign, string.Empty).SplitRegex(Regexes.Lexers.TableSplitter));
            var cells = match.Groups[3].Value.ReplaceRegex(Regexes.Lexers.UselessGfmTableCell, string.Empty).Split('\n').Select(x => new string[] { x }).ToArray();
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = cells[i][0]
                  .ReplaceRegex(Regexes.Lexers.EmptyGfmTableCell, string.Empty)
                  .SplitRegex(Regexes.Lexers.TableSplitter);
            }
            return new MarkdownTableBlockToken(this, header, align, cells);
        }

        protected virtual Align[] ParseAligns(string[] aligns)
        {
            var result = new Align[aligns.Length];
            for (int i = 0; i < aligns.Length; i++)
            {
                if (Regexes.Lexers.TableAlignRight.IsMatch(aligns[i]))
                {
                    result[i] = Align.Right;
                }
                else if (Regexes.Lexers.TableAlignCenter.IsMatch(aligns[i]))
                {
                    result[i] = Align.Center;
                }
                else if (Regexes.Lexers.TableAlignLeft.IsMatch(aligns[i]))
                {
                    result[i] = Align.Left;
                }
                else
                {
                    result[i] = Align.NotSpec;
                }
            }
            return result;
        }
    }
}
