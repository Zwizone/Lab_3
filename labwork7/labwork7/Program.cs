try
{
    Console.Write("Введите первое число: ");
    int a = int.Parse(Console.ReadLine());

    Console.Write("Введите второе число: ");
    int b = int.Parse(Console.ReadLine());

    Console.WriteLine($"\nСложение: {a} + {b} = {a + b}");
    Console.WriteLine($"Вычитание: {a} - {b} = {a - b}");
    Console.WriteLine($"Умножение: {a} * {b} = {a * b}");
    Console.WriteLine($"Деление: {a} / {b} = {a / b}");
}
catch (FormatException ex) { LogAndShow(ex); }
catch (OverflowException ex) { LogAndShow(ex); }
catch (DivideByZeroException ex) { LogAndShow(ex); }
catch (Exception ex) { LogAndShow(ex); }

static void LogAndShow(Exception ex)
{
    string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}\n";
    Console.WriteLine($"{ex.Message}");
    File.AppendAllText("log.txt", log);
}