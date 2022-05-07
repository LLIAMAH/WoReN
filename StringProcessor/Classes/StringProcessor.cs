namespace StringProcessor.Classes
{
    internal enum CheckResult
    {
        Exit,
        Result,
        Clean,
        CheckFormat,
        ValidationFailedNextStepRequest,
        Continue
    }

    internal enum CheckFormat
    {
        Failed,
        Succeeded
    }

    internal class StringProcessor
    {
        private readonly List<string> _result;
        private readonly List<string> _resultBad; // написано было - запихивать в "плохие строки", но что потом сэтим делать - нипанятна. Вообщем я их чищу по Clean тоже и они есть - если надо используй где надо.
        internal const int FormatMin = 1;
        internal const int FormatMax = 7;

        // Строка для вывода результата - она НЕ содержит данных до момента, пока этот результат надо выводить.
        // настоящий источник данных и хранилище всех валидных сегментов - _result, откуда все сегменты и джойнятся в результат
        public string Result => string.Join(',', this._result); 

        internal StringProcessor()
        {
            this._result = new List<string>();
            this._resultBad = new List<string>();
        }

        public CheckResult CheckInput(string? line)
        {
            if (string.IsNullOrEmpty(line))
                return CheckResult.Exit;

            if (line.ToLower().Equals("result", StringComparison.OrdinalIgnoreCase))
                return CheckResult.Result;

            if (line.ToLower().Equals("clean", StringComparison.OrdinalIgnoreCase))
                return CheckResult.Clean;

            return line.Length switch
            {
                >= FormatMax => CheckResult.CheckFormat,
                >= FormatMin and < FormatMax => CheckResult.ValidationFailedNextStepRequest,
                _ => CheckResult.Continue
            };
        }

        public void Clean()
        {
            this._result.Clear();
            this._resultBad.Clear();
        }

        public CheckFormat CheckFormat(string s)
        {
            if (MatchFormat(s))
            {
                if (this._result.Count == 0)
                    this._result.Add(s);
                else
                {
                    var fragmentIndex = -1;
                    for (var i = 0; i < this._result.Count; i++)
                    {
                        if (this._result[i][0] < s[0])
                            fragmentIndex = i;
                        else if (this._result[i][0] == s[0])
                        {
                            var stringFragmentNumbers = GetNumbers(this._result[i]);
                            var fragmentNumbers = GetNumbers(s);

                            if (stringFragmentNumbers > fragmentNumbers)
                                fragmentIndex = i;
                            else if (stringFragmentNumbers == fragmentNumbers)
                            {
                                var stringFragment3Rd = GetLetter(this._result[i], 2);
                                var fragment3Rd = GetLetter(s, 2);
                                if (stringFragment3Rd <= fragment3Rd)
                                    fragmentIndex = i;
                            }
                        }
                    }

                    if (fragmentIndex >= 0)
                        this._result.Insert(fragmentIndex, s);
                }

                return Classes.CheckFormat.Succeeded;
            }

            this._resultBad.Add(s); // add to failed rows
            return Classes.CheckFormat.Failed;
        }

        private static char GetLetter(string input, int index)
        {
            return input[index];
        }

        private static int GetNumbers(string input)
        {
            var stringResult = string.Empty;
            foreach (var item in input)
            {
                if (char.IsNumber(item))
                    stringResult += item;
            }

            return string.IsNullOrEmpty(stringResult) ? 0 : Convert.ToInt32(stringResult);
        }

        private static bool MatchFormat(string s)
        {
            return s.Length == 7 && char.IsLetter(s[0]) && char.IsDigit(s[1]) && char.IsDigit(s[2]) && char.IsDigit(s[3]) && char.IsDigit(s[4]) && char.IsLetter(s[5]) && char.IsLetter(s[6]);
        }
    }
}
