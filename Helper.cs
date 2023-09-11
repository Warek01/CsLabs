namespace CS_Labs;

public static class Helper {
  public static void ColorPrintIf(
    string       text,
    bool         expr,
    ConsoleColor bg = ConsoleColor.Black,
    ConsoleColor fg = ConsoleColor.White
  ) {
    if (expr) {
      Console.BackgroundColor = bg;
      Console.ForegroundColor = fg;
    }

    Console.Write(text);
    Console.ResetColor();
  }

  public static T SelectMultipleChoice<T>(
    List<KeyValuePair<T, string>> dict,
    int                           defaultSelected = 0,
    bool                          vertical        = false,
    ConsoleColor                  bg              = ConsoleColor.DarkCyan,
    ConsoleColor                  fg              = ConsoleColor.Black,
    string?                       title           = null,
    int                           buttonWidth     = 16
  ) where T : notnull {
    int selected = defaultSelected;

    do {
      Console.Clear();
      if (title is not null) {
        Console.WriteLine(title);
      }

      foreach (KeyValuePair<T, string> pair in dict) {
        string str = pair.Value;

        for (int i = 0; i < (buttonWidth - pair.Value.Length) / 2; i++) {
          str =  $" {str} ";
        }

        ColorPrintIf(str, Equals(dict[selected], pair), bg, fg);
        Console.Write(
          vertical
            ? "\n"
            : Equals(pair, dict[^1])
              ? " "
              : " | "
        );
      }

      ConsoleKeyInfo keyInfo = Console.ReadKey(true);
      switch (vertical, keyInfo.Key) {
        case (_, ConsoleKey.Enter):
          return dict[selected].Key;
        case (true, ConsoleKey.DownArrow):
        case (false, ConsoleKey.RightArrow):
          if (selected == dict.Count - 1)
            selected = 0;
          else
            selected++;
          break;
        case (true, ConsoleKey.UpArrow):
        case (false, ConsoleKey.LeftArrow):
          if (selected == 0)
            selected = dict.Count - 1;
          else
            selected--;
          break;
      }
    } while (true);
  }
}
