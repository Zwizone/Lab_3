using System.Diagnostics;

var ts = new TraceSource("Calculator")
{
    Switch = new SourceSwitch("CalculatorSwitch") { Level = SourceLevels.Information }
};
ts.Listeners.Clear();
ts.Listeners.Add(new TextWriterTraceListener("trace.log", "fileListener"));
ts.Listeners.Add(new ConsoleTraceListener());
Trace.AutoFlush = true;

int a = ReadInt(ts, "первое");
if (a == int.MinValue) return;

int b = ReadInt(ts, "второе");
if (b == int.MinValue) return;

ts.TraceInformation($"Введены числа: a = {a}, b = {b}");
ts.TraceEvent(TraceEventType.Verbose, 3, $"Входные параметры: a = {a}, b = {b}");

ExecuteAndLog(ts, "Сложение", a, b, (x, y) => checked(x + y));
ExecuteAndLog(ts, "Вычитание", a, b, (x, y) => checked(x - y));
ExecuteAndLog(ts, "Умножение", a, b, (x, y) => checked(x * y));

if (b == 0)
{
    LogError(ts, "Деление", "деление на ноль");
}
else
{
    int res = a / b;
    Console.WriteLine($"Деление: {a} / {b} = {res}");
    ts.TraceInformation($"Выполнено деление: {a} / {b} = {res}");
}

ts.Flush();
ts.Close();

Console.WriteLine("\nЛогирование завершено. Проверьте файл trace.log.");


static int ReadInt(TraceSource ts, string label)
{
    Console.Write($"Введите {label} число: ");
    string? input = Console.ReadLine();
    if (int.TryParse(input, out int value))
        return value;

    ts.TraceEvent(TraceEventType.Error, 1, $"Ошибка ввода: {label} число не является целым.");
    return int.MinValue;
}

static void ExecuteAndLog(TraceSource ts, string op, int a, int b, Func<int, int, int> operation)
{
    try
    {
        int result = operation(a, b);
        string expr = op switch
        {
            "Сложение" => $"{a} + {b}",
            "Вычитание" => $"{a} - {b}",
            "Умножение" => $"{a} * {b}",
            _ => $"{a} ? {b}"
        };

        Console.WriteLine($"{op}: {expr} = {result}");
        ts.TraceInformation($"Выполнено {op.ToLower()}: {expr} = {result}");
    }
    catch (OverflowException)
    {
        LogError(ts, op, "переполнение");
    }
}

static void LogError(TraceSource ts, string op, string reason)
{
    string msg = $"Ошибка при {op.ToLower()}: {reason}";
    Console.WriteLine($"Ошибка: {msg}");
    ts.TraceEvent(TraceEventType.Error, 4, msg);
}