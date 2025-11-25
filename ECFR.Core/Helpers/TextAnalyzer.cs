using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECFR.Core.Helpers
{
    public static class TextAnalyzer
    {
        static readonly Regex WordRegex = new Regex(@"\b[\p{L}\p{N}']+\b", RegexOptions.Compiled);

        public static int WordCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return WordRegex.Matches(text).Count;
        }

        public static HashSet<string> UniqueTerms(string text)
        {
            var matches = WordRegex.Matches(text.ToLowerInvariant());
            var set = new HashSet<string>();
            foreach (Match m in matches) set.Add(m.Value);
            return set;
        }

        public static string Sha256Hex(string text)
        {
            if (text == null) text = "";
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        // Jaccard similarity between two sets of tokens
        public static double Jaccard(HashSet<string> a, HashSet<string> b)
        {
            if (a.Count == 0 && b.Count == 0) return 1.0;
            var inter = a.Intersect(b).Count();
            var uni = a.Union(b).Count();
            return uni == 0 ? 0 : (double)inter / uni;
        }

        // Flesch reading ease (approx)
        public static double FleschReadingEase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            var words = WordRegex.Matches(text).Count;
            var sentences = Regex.Split(text, @"[.!?]+").Count(s => !string.IsNullOrWhiteSpace(s));
            // syllable estimate: vowels groups
            var syllables = 0;
            foreach (Match w in WordRegex.Matches(text))
            {
                syllables += EstimateSyllables(w.Value);
            }
            if (words == 0 || sentences == 0) return 0;
            return 206.835 - 1.015 * ((double)words / sentences) - 84.6 * ((double)syllables / words);
        }

        // crude syllable estimation
        static int EstimateSyllables(string word)
        {
            word = word.ToLowerInvariant();
            if (word.Length <= 3) return 1;
            var vowels = "aeiouy";
            int count = 0;
            bool prevVowel = false;
            foreach (var c in word)
            {
                bool isV = vowels.IndexOf(c) >= 0;
                if (isV && !prevVowel) count++;
                prevVowel = isV;
            }
            // silent e
            if (word.EndsWith("e") && count > 1) count--;
            return Math.Max(1, count);
        }
    }
}
