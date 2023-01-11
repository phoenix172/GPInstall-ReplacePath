using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

string file1 = @"R:\install\GPI\HRM_2008_pw.gpi";
string file2 = @"M:\install_gpi\HRM_2008_pw.gpi";

(string prefix, string suffix)[] appendages =
{
    ("\x0001\x0008FileName\x0006", "\x000c"),
    ("SourceFile\x0006", "\t")
};
string filePathRegexString = @"(?:[a-zA-Z]\:|\\\\[\w\-\.]+\\[\w\-.$]+)\\(?:[\w\-а-яА-Я ]+\\)*[\wа-яА-Я]([\w\-\–.а-яА-Я ])+";
Regex[] fileRegexes = appendages
    .Select(x =>
        new Regex($"(?'prefix'{x.prefix})(?'length'.)(?'path'{filePathRegexString})(?'suffix'{x.suffix})", RegexOptions.IgnoreCase))
    .ToArray();
//$"(?'prefix'\x0001\x0008FileName\x0006)(?'length'.)(?'path'{filePathRegexString})(?'suffix'\x000c)"

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding encoding = Encoding.GetEncoding(1251);

string ReplaceFileName(string content, string replace, string with)
{
    string ReplaceRegex(string replaceContent, Regex regex)
    {
        Debug.WriteLine(regex);
        return regex.Replace(replaceContent, match =>
        {
            string prefix = match.Groups["prefix"].Value;
            int length = (int)match.Groups["length"].Value.First();
            string path = match.Groups["path"].Value;
            string suffix = match.Groups["suffix"].Value;

            string replacedPath = path.Replace(replace, with, StringComparison.OrdinalIgnoreCase);

            string result = $"{prefix}{(char)replacedPath.Length}{replacedPath}{suffix}";

            return result;
        });
    }

    string newContent = fileRegexes.Aggregate(content, ReplaceRegex);
    return newContent;
}

string ReplaceFile(string inputFilePath, string replaceWhat, string replaceWith, Encoding encoding, string? outputFile = null)
{
    string content = File.ReadAllText(inputFilePath, encoding);

    string output = ReplaceFileName(content, replaceWhat, replaceWith);

    string outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + "_replaced" + Path.GetExtension(inputFilePath);
    string outputFilePath = outputFile ?? Path.Combine(Path.GetDirectoryName(inputFilePath), outputFileName);

    File.WriteAllText(outputFilePath, output, encoding);

    return outputFilePath;
}

void RunConsole()
{
    if (args.Length != 3 && args.Length != 4)
    {
        Console.WriteLine("GPInstall-ReplacePath <file> <replace_what> <replace_with> [output_file]");
        return;
    }

    string replaceFilePath = args[0];
    string replaceWhat = args[1];
    string replaceWith = args[2];
    string? outputFile = args.Length > 3 ? args[3] : null;

    if (!File.Exists(replaceFilePath))
    {
        Console.WriteLine($"Replace file path {replaceFilePath} does not exist.");
        return;
    }
    Console.WriteLine($"Replacing \n {replaceWhat} \n with \n {replaceWith}");

    string outputPath = ReplaceFile(replaceFilePath, replaceWhat, replaceWith, encoding, outputFile);

    Console.WriteLine($"Output saved to {outputPath}");
}


//ReplaceFile(file2, encoding);

RunConsole();

