// See https://aka.ms/new-console-template for more information

using System;
using StringProcessor.Classes;

Console.WriteLine("Hello, StringProcessor is here!");
Console.WriteLine("Started work.");

const string inputRules = "Rules";
Console.WriteLine(inputRules);

var processor = new StringProcessor.Classes.StringProcessor();

var readLine = InputStringsOffer();

var continueWork = true;

while (continueWork && readLine != null)
{
    var splitStrings = readLine.Split(',');
    var checkResult = CheckResult.Continue;

    foreach (var s in splitStrings)
    {
        checkResult = processor.CheckInput(s);
        switch (checkResult)
        {
            case CheckResult.Exit:
                continueWork = false;
                break;
            case CheckResult.Result:
            {
                Console.WriteLine($"Result string: {processor.Result}");
            }
                break;
            case CheckResult.Clean:
            {
                processor.Clean();
                Console.Write(inputRules);
            }
                break;
            case CheckResult.PrintToFile:
            {
                processor.Print();
            }
                break;
            case CheckResult.CheckFormat:
            {
                switch (processor.CheckFormat(s))
                {
                    case CheckFormat.Succeeded:
                        Console.WriteLine($"Format check - succeeded. String '{s}' added to result.");
                        break;
                    case CheckFormat.Failed:
                    default:
                        Console.WriteLine($"Format check - failed. Cannot add string '{s}' to result.");
                        break;
                }
            }
                break;
            case CheckResult.ValidationFailedNextStepRequest:
                Console.WriteLine($"Validation check - failed: string '{s}' length in range of [{StringProcessor.Classes.StringProcessor.FormatMin}..{StringProcessor.Classes.StringProcessor.FormatMax}]");
                break;
            case CheckResult.Continue:
            default: break;
        }
    }

    if (checkResult == CheckResult.Exit)
        continueWork = false;
    else
        readLine = InputStringsOffer();
}

Console.WriteLine("Finished work.");

static string? InputStringsOffer()
{
    Console.Write("Input string (or strings set with ',' character as delimiter): ");
    var inputLine = Console.ReadLine();
    return inputLine;
}