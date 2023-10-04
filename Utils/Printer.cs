using System.Globalization;

namespace CS_Labs.Helpers;

public class Printer {
  private ConsoleColor _fg;
  private ConsoleColor _bg;

  public Printer(ConsoleColor? fg = null, ConsoleColor? bg = null) {
    _fg = fg ?? Console.ForegroundColor;
    _bg = bg ?? Console.BackgroundColor;
  }

  public Printer Text(string text, ConsoleColor? fg = null, ConsoleColor? bg = null) {
    Print(text, fg, bg);
    return this;
  }

  public Printer Number(double number, ConsoleColor fg = ConsoleColor.Red, ConsoleColor? bg = null) {
    Print(number.ToString(CultureInfo.InvariantCulture), fg, bg);
    return this;
  }
  
  public Printer Currency(double number, ConsoleColor fg = ConsoleColor.Red, ConsoleColor? bg = null) {
    Print(number.ToString("C2"), fg, bg);
    return this;
  }

  public Printer Bool(bool expr, ConsoleColor fg = ConsoleColor.Yellow, ConsoleColor? bg = null) {
    Print(expr ? "True" : "False", fg, bg);
    return this;
  }

  public Printer YesNo(bool expr, ConsoleColor fg = ConsoleColor.Yellow, ConsoleColor? bg = null) {
    Print(expr ? "Yes" : "No", fg, bg);
    return this;
  }

  public Printer NewLine(ConsoleColor? bg = null) {
    Print("\n", null, bg);
    return this;
  }

  public Printer Prompt(out string? text) {
    text = Console.ReadLine();
    return this;
  }

  public Printer PromptInt(out int num) {
    num = Convert.ToInt32(Console.ReadLine()!);
    return this;
  }

  public Printer PromptDouble(out double num) {
    num = Convert.ToDouble(Console.ReadLine()!);
    return this;
  }

  public Printer SetGlobalBg(ConsoleColor? bg = null) {
    _bg = bg ?? Console.BackgroundColor;
    return this;
  }

  public Printer SetGlobalFg(ConsoleColor? fg = null) {
    _fg = fg ?? Console.ForegroundColor;
    return this;
  }

  public Printer SetBg(ConsoleColor? bg = null) {
    return new Printer(_fg, bg);
  }

  public Printer SetFg(ConsoleColor? fg = null) {
    return new Printer(fg);
  }

  public Printer Clear() {
    Console.Clear();
    return this;
  }

  private void Print(string text, ConsoleColor? fg, ConsoleColor? bg) {
    Console.ForegroundColor = fg ?? _fg;
    Console.BackgroundColor = bg ?? _bg;
    Console.Write(text);
    Console.ResetColor();
  }
}
