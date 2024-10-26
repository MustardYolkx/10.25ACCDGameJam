using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextAnalyzer : MonoBehaviour
{
    private HashSet<string> keywords;
    private Dictionary<string, int> keywordCounts;
    private Dictionary<string, List<int>> keywordPositions;

    public TextAnalyzer(IEnumerable<string> initialKeywords = null)
    {
        keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // 不区分大小写
        keywordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        keywordPositions = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

        if (initialKeywords != null)
        {
            foreach (var keyword in initialKeywords)
            {
                AddKeyword(keyword);
            }
        }
    }

 
    public void AddKeyword(string keyword)
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keywords.Add(keyword.Trim());
        }
    }


    public void AddKeywords(IEnumerable<string> newKeywords)
    {
        foreach (var keyword in newKeywords)
        {
            AddKeyword(keyword);
        }
    }


    public void RemoveKeyword(string keyword)
    {
        keywords.Remove(keyword);
        keywordCounts.Remove(keyword);
        keywordPositions.Remove(keyword);
    }


    public AnalysisResult AnalyzeText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return new AnalysisResult();


        keywordCounts.Clear();
        keywordPositions.Clear();

        foreach (var keyword in keywords)
        {
           
            var matches = Regex.Matches(text, Regex.Escape(keyword), RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                keywordCounts[keyword] = matches.Count;
                keywordPositions[keyword] = matches.Cast<Match>()
                    .Select(m => m.Index)
                    .ToList();
            }
        }

        return new AnalysisResult
        {
            TotalKeywordsFound = keywordCounts.Values.Sum(),
            KeywordCounts = new Dictionary<string, int>(keywordCounts),
            KeywordPositions = new Dictionary<string, List<int>>(keywordPositions),
            TextLength = text.Length,
            KeywordDensity = CalculateKeywordDensity(text)
        };
    }


    private Dictionary<string, double> CalculateKeywordDensity(string text)
    {
        var density = new Dictionary<string, double>();
        var totalWords = text.Split(new[] { ' ', '\t', '\n', '\r' },
            StringSplitOptions.RemoveEmptyEntries).Length;

        foreach (var kvp in keywordCounts)
        {
            density[kvp.Key] = (double)kvp.Value / totalWords * 100;
        }

        return density;
    }


    public List<string> GetKeywordContext(string text, string keyword, int contextLength = 50)
    {
        var contexts = new List<string>();

        if (!keywordPositions.ContainsKey(keyword))
            return contexts;

        foreach (var position in keywordPositions[keyword])
        {
            var startPos = Math.Max(0, position - contextLength);
            var endPos = Math.Min(text.Length, position + keyword.Length + contextLength);
            var context = text.Substring(startPos, endPos - startPos);


            if (startPos > 0) context = "..." + context;
            if (endPos < text.Length) context = context + "...";

            contexts.Add(context);
        }

        return contexts;
    }


    public class AnalysisResult
    {
        public int TotalKeywordsFound { get; set; }
        public Dictionary<string, int> KeywordCounts { get; set; }
        public Dictionary<string, List<int>> KeywordPositions { get; set; }
        public int TextLength { get; set; }
        public Dictionary<string, double> KeywordDensity { get; set; }

        public AnalysisResult()
        {
            KeywordCounts = new Dictionary<string, int>();
            KeywordPositions = new Dictionary<string, List<int>>();
            KeywordDensity = new Dictionary<string, double>();
        }
    }
}


public class TextAnalysisExample
{
    public static void RunExample()
    {
        
        var analyzer = new TextAnalyzer(new[] { "Unity", "游戏", "开发" });

        
        string sampleText = "Unity是一个强大的游戏开发引擎。使用Unity开发游戏既快速又简单。";

        
        var result = analyzer.AnalyzeText(sampleText);

        
        Console.WriteLine($"文本长度: {result.TextLength}");
        Console.WriteLine($"找到的关键词总数: {result.TotalKeywordsFound}");

        foreach (var kvp in result.KeywordCounts)
        {
            Console.WriteLine($"关键词 '{kvp.Key}' 出现 {kvp.Value} 次");
            Console.WriteLine($"密度: {result.KeywordDensity[kvp.Key]:F2}%");

            // 获取并显示上下文
            var contexts = analyzer.GetKeywordContext(sampleText, kvp.Key);
            foreach (var context in contexts)
            {
                Console.WriteLine($"上下文: {context}");
            }
        }
    }
}
