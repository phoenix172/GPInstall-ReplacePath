using System.Text;
using System.Text.RegularExpressions;

string file1 = @"M:\install_gpi\HRM_2008_upgr_no_instr.gpi";
string file2 = @"M:\install_gpi\HRM_2008_pw.gpi";

string filePathRegexString = @"(?:[a-zA-Z]\:|\\\\[\w\-\.]+\\[\w\-.$]+)\\(?:[\w\-а-яА-Я ]+\\)*[\wа-яА-Я]([\w\-.а-яА-Я ])+";
Regex filePathRegex = new Regex(filePathRegexString);
//Regex fileRegex = new Regex($"\\x00\\x01\\x08FileName\\x08.({filePathRegexString})\\x12");
Regex fileRegex = new Regex($"(?'prefix'\x0001\x0008FileName\x0006)(?'length'.)(?'path'{filePathRegexString})(?'suffix'\x000c)");


string ReplaceFileNamePrefix(string content, string replace, string with)
{
    string newContent = fileRegex.Replace(content, match =>
    {
        string prefix = match.Groups["prefix"].Value;
        int length = (int)match.Groups["length"].Value.First();
        string path = match.Groups["path"].Value;
        string suffix = match.Groups["suffix"].Value;

        string replacedPath = path.Replace(replace, with, StringComparison.OrdinalIgnoreCase);

        string result = $"{prefix}{(char) replacedPath.Length}{replacedPath}{suffix}";

        return result;
    });
    return newContent;
}

void ReplaceFile(string inputFilePath, Encoding encoding)
{
    string content = File.ReadAllText(inputFilePath, encoding);

    string output = ReplaceFileNamePrefix(content, @"\\users\Install_Source\", @"F:\install_sorce\");

    string outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + "_replaced" + Path.GetExtension(inputFilePath);
    string outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), outputFileName);
    
    File.WriteAllText(outputFilePath, output, encoding);
}


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding encoding = Encoding.GetEncoding(1251);

ReplaceFile(file2, encoding);
