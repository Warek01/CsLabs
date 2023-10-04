using CS_Labs.Helpers;

namespace CS_Labs;

public class CaesarCipher {
  private const int AlphabetSize = 26;

  private readonly Printer _printer = new();

  private static readonly List<char> Alphabet = new() {
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z'
  };

  private readonly List<char> _cryptogram;
  private readonly string     _message;
  private readonly int?       _shiftSize;
  private readonly string?    _keyword;

  public CaesarCipher() {
    do {
      _message    = SelectMessageScreen();
      _cryptogram = new(Alphabet);

      Console.Clear();
      if (
        new SelectForm<bool>()
        .Title("Add second key?")
        .AddOption(true,  "Yes")
        .AddOption(false, "No")
        .Render()
      ) {
        Console.Clear();
        _keyword    = SelectKeyword();
        _cryptogram = UpdateCryptogramWithKeyword();
      }

      Console.Clear();
      CipherMethod method = _keyword is not null
        ? CipherMethod.Key
        : new SelectForm<CipherMethod>()
          .Title("Select cipher creation method")
          .AddOption(CipherMethod.Cryptogram, "Cryptogram")
          .AddOption(CipherMethod.Key,        "Key")
          .Render();

      if (method == CipherMethod.Key) {
        _shiftSize  = SelectShiftSizeScreen();
        _cryptogram = UpdateCryptogramWithK();
      }
      else {
        _cryptogram = SelectCryptogram();
      }


      Console.Clear();
      Operation selectedOp = new SelectForm<Operation>()
                             .Title("Select operation")
                             .AddOption(Operation.Decrypt, "Decrypt")
                             .AddOption(Operation.Encrypt, "Encrypt")
                             .Render();
      Console.Clear();

      string newMessage = selectedOp == Operation.Encrypt
        ? EncryptMessage()
        : DecryptMessage();

      _printer
        .Clear()
        .Text("Message:", ConsoleColor.Blue)
        .NewLine()
        .Text(newMessage)
        .NewLine()
        .NewLine()
        .Text("Cryptogram:", ConsoleColor.Blue)
        .NewLine()
        .Text(_cryptogram.Aggregate(string.Empty, (current, c) => current + c + " "))
        .NewLine()
        .NewLine()
        .Text("Press any key to continue.", ConsoleColor.Yellow);

      Console.ReadKey(true);
    } while (
      new SelectForm<bool>()
      .Title("Restart?")
      .AddOption(false, "No")
      .AddOption(true,  "Yes")
      .Render()
    );

    _printer
      .Clear()
      .Text("BYE", ConsoleColor.Green);
  }

  private List<char> UpdateCryptogramWithKeyword() {
    HashSet<char> chars             = new();
    Queue<char>   keywordCharsQueue = new(_keyword!);
    List<char>    newCryptogram     = new();

    for (int i = 0; i < _keyword!.Length + _cryptogram.Count; i++) {
      char c = keywordCharsQueue.Any()
        ? keywordCharsQueue.Dequeue()
        : Alphabet[i - _keyword.Length];

      if (chars.Contains(c)) {
        continue;
      }

      newCryptogram.Add(c);
      chars.Add(c);
    }

    return newCryptogram;
  }

  private int SelectShiftSizeScreen() {
    bool withError = false;
    do {
      _printer.Clear();

      if (withError) {
        _printer
          .Text("Wrong input (Must be between 1 and 25)", ConsoleColor.Red)
          .NewLine();
      }

      _printer
        .Text("Shift size (1 - 25): ", ConsoleColor.Blue)
        .Prompt(out string? shiftSize);

      if (
        shiftSize is null
        || !int.TryParse(shiftSize, out int k)
        || k is < 1 or > 25
      ) {
        withError = true;
        continue;
      }

      return k;
    } while (true);
  }

  private string SelectMessageScreen() {
    _printer
      .Clear()
      .Text("Message: ", ConsoleColor.Blue)
      .Prompt(out string? rawMessage);

    string message = rawMessage!
                     .Where(c => c is not (' ' or '\t' or '\n' or '\0'))
                     .Aggregate(
                       string.Empty,
                       (current, c) => current + c.ToString().ToUpper()
                     );

    return message;
  }

  private string SelectKeyword() {
    bool    error = false;
    string? keyword;

    do {
      _printer.Clear();

      if (error) {
        _printer
          .Text("Message length must be greater than 7", ConsoleColor.Red)
          .NewLine();

        error = false;
      }

      _printer
        .Text("Keyword: ", ConsoleColor.Blue)
        .Prompt(out keyword);

      if (keyword is null || keyword.Length < 7) {
        error = true;
        continue;
      }

      keyword = keyword
                .Where(c => c is not (' ' or '\t' or '\n' or '\0'))
                .Aggregate(string.Empty, (current, c) => current + c.ToString().ToUpper());
      
    } while (error);

    return keyword!;
  }

  private string EncryptMessage() {
    string encrypted = string.Empty;

    foreach (char c in _message) {
      int code = c - 'A';
      encrypted += _cryptogram[code];
    }

    return encrypted;
  }

  private string DecryptMessage() {
    return _message.Aggregate(
      string.Empty,
      (current, c) => current + Alphabet[_cryptogram.FindIndex(cc => cc == c)]
    );
  }

  private List<char> SelectCryptogram() {
    Console.WriteLine("\nInput cryptogram");

    string     line       = Console.ReadLine()!;
    List<char> cryptogram = new(AlphabetSize);

    foreach (var c in line.Where(c => c is > 'A' and < 'z')) {
      cryptogram.Add(c.ToString().ToUpper()[0]);
    }

    return cryptogram;
  }

  private List<char> UpdateCryptogramWithK() {
    List<char> newCryptogram = new();

    for (int i = 0; i < AlphabetSize; i++) {
      newCryptogram.Add(_cryptogram[(i + _shiftSize!.Value) % AlphabetSize]);
    }

    return newCryptogram;
  }

  private enum Operation {
    Encrypt,
    Decrypt,
  }

  private enum CipherMethod {
    Key,
    Cryptogram
  }
}
