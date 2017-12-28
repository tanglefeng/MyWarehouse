using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kengic.Was.CrossCutting.SocketsServer
{
    public class RequestHandler
    {
        private string _temp = string.Empty;
        public string[] GetActualString(string input) => GetActualString(input, null);

        private string[] GetActualString(string input, List<string> outputList)
        {
            if (outputList == null)
            {
                outputList = new List<string>();
            }
            if (!string.IsNullOrEmpty(_temp))
            {
                input = _temp + input;
            }
            const string pattern = @"(?<=^\[length=)(\d+)(?=\])";
            if (Regex.IsMatch(input, pattern))
            {
                var m = Regex.Match(input, pattern);
                var length = Convert.ToInt32(m.Groups[0].Value);
                var startIndex = input.IndexOf(']') + 1;
                var output = input.Substring(startIndex);
                if (output.Length == length)
                {
                    outputList.Add(output);
                    _temp = string.Empty;
                }
                else if (output.Length < length)
                {
                    _temp = input;
                }
                else if (output.Length > length)
                {
                    output = output.Substring(0, length);
                    outputList.Add(output);
                    _temp = string.Empty;
                    input = input.Substring(startIndex + length);
                    GetActualString(input, outputList);
                }
            }
            else
            {
                _temp = input;
                outputList.Add(_temp);
            }
            return outputList.ToArray();
        }
    }
}