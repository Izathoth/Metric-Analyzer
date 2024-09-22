using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Digite o caminho do diretório para analisar:");
        string directoryPath = Console.ReadLine();

        if (Directory.Exists(directoryPath))
        {
            var report = AnalyzeDirectory(directoryPath);
            GenerateReport(report);
        }
        else
        {
            Console.WriteLine("O diretório informado não existe.");
        }
    }

    static Dictionary<string, FileMetrics> AnalyzeDirectory(string directoryPath)
    {
        var report = new Dictionary<string, FileMetrics>();

        foreach (var filePath in Directory.GetFiles(directoryPath, "*.cs"))
        {
            var metrics = AnalyzeFile(filePath);
            report[Path.GetFileName(filePath)] = metrics;
        }

        return report;
    }

    static FileMetrics AnalyzeFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        int numLines = lines.Length;
        int numFunctions = 0;
        int numComments = 0;

        foreach (var line in lines)
        {
            string trimmedLine = line.Trim();

            // Contar funções
            if (trimmedLine.StartsWith("public") || trimmedLine.StartsWith("private") || 
                trimmedLine.StartsWith("protected") || trimmedLine.StartsWith("internal"))
            {
                if (trimmedLine.Contains("()"))
                {
                    numFunctions++;
                }
            }

            // Contar comentários
            if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("/*") || trimmedLine.StartsWith("*"))
            {
                numComments++;
            }
        }

        return new FileMetrics(numLines, numFunctions, numComments);
    }

    static void GenerateReport(Dictionary<string, FileMetrics> report)
    {
        Console.WriteLine($"{ "Arquivo", -30} { "Linhas", -10} { "Funções", -10} { "Comentários", -10}");
        Console.WriteLine(new string('-', 70));

        foreach (var entry in report)
        {
            Console.WriteLine($"{entry.Key,-30} {entry.Value.NumLines,-10} {entry.Value.NumFunctions,-10} {entry.Value.NumComments,-10}");
        }
    }
}

class FileMetrics
{
    public int NumLines { get; }
    public int NumFunctions { get; }
    public int NumComments { get; }

    public FileMetrics(int numLines, int numFunctions, int numComments)
    {
        NumLines = numLines;
        NumFunctions = numFunctions;
        NumComments = numComments;
    }
}