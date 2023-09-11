namespace CS_Labs;

public static class CaesarCipher {
  private const int AlphabetSize = 26;

  private static readonly char[] Alphabet = new char[AlphabetSize] {
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z'
  };

  public static void Run() {
    do {
      char[]  cryptogram = new char[AlphabetSize];
      int?    shiftSize  = null;
      string? keyword    = null;
      string  message    = SelectMessageScreen();

      for (int i = 0; i < AlphabetSize; i++) {
        cryptogram[i] = Alphabet[i];
      }

      Console.Clear();
      if (AddKeywordPrompt()) {
        Console.Clear();
        keyword    = SelectKeyword();
        cryptogram = UpdateCryptogramWithKeyword(cryptogram, keyword);
      }

      Console.Clear();
      CipherMethod method = keyword is not null ? CipherMethod.Key : SelectEncryptionMethodScreen();
      Console.Clear();

      if (method == CipherMethod.Key) {
        shiftSize  = SelectKScreen();
        cryptogram = UpdateCryptogramWithK(shiftSize.Value, cryptogram);
      }
      else {
        cryptogram = SelectCryptogram();
      }


      Console.Clear();
      Operation selectedOp = SelectOperation();
      Console.Clear();

      string newMessage = selectedOp == Operation.Encrypt
        ? EncryptMessage(message, cryptogram)
        : shiftSize is not null
          ? DecryptMessage(message, cryptogram, shiftSize.Value)
          : DecryptMessage(message, cryptogram);

      Console.WriteLine(newMessage);
      Console.ReadKey(true);
    } while (Helper.SelectMultipleChoice<bool>(
               new() {
                 new(false, "No"),
                 new(true, "Yes"),
               },
               title: "Restart?"
             ));

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("BYE");
    Console.ResetColor();
  }

  private static char[] UpdateCryptogramWithKeyword(char[] cryptogram, string keyword) {
    HashSet<char> chars             = new();
    Queue<char>   keywordCharsQueue = new(keyword);
    char[]        newCryptogram     = new char[AlphabetSize];

    for (int i = 0, j = 0; i < keyword.Length + cryptogram.Length; i++) {
      char c = keywordCharsQueue.Any()
        ? keywordCharsQueue.Dequeue()
        : Alphabet[i - keyword.Length];

      if (chars.Contains(c)) {
        continue;
      }

      newCryptogram[j++] = c;
      chars.Add(c);
    }

    return newCryptogram;
  }

  private static Operation SelectOperation() {
    return Helper.SelectMultipleChoice<Operation>(
      new() {
        new(Operation.Encrypt, "Encrypt"),
        new(Operation.Decrypt, "Decrypt")
      },
      title: "Select operation"
    );
  }

  private static CipherMethod SelectEncryptionMethodScreen() {
    return Helper.SelectMultipleChoice<CipherMethod>(
      new() {
        new(CipherMethod.Key, "Key"),
        new(CipherMethod.Cryptogram, "Cryptogram")
      },
      title: "Select cipher creation method"
    );
  }

  private static int SelectKScreen() {
    bool withError = false;
    do {
      Console.Clear();
      if (withError) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Wrong input");
        Console.ResetColor();
      }

      Console.Write("Shift size (0 - 25): ");
      string? _k = Console.ReadLine();

      if (
        _k is null
        || !int.TryParse(_k, out int k)
        || k is < 0 or > AlphabetSize - 1
      ) {
        withError = true;
        continue;
      }

      return k;
    } while (true);
  }

  private static string SelectMessageScreen() {
    Console.Clear();
    Console.Write("Message: ");
    return Console.ReadLine()!.ToUpper();
  }

  private static string SelectKeyword() {
    Console.Write("Keyword: ");
    return Console.ReadLine()!.ToUpper();
  }

  private static string EncryptMessage(string m, char[] cryptogram) {
    string encrypted = string.Empty;

    foreach (char c in m) {
      int code = c - 'A';
      encrypted += cryptogram[code];
    }

    return encrypted;
  }

  private static string DecryptMessage(string m, char[] cryptogram) {
    string decrypted = string.Empty;
    int    diff      = Alphabet[0] - cryptogram[0];

    foreach (char c in m) {
      int code = c - 'A';
      decrypted += Alphabet[code + diff];
    }

    return decrypted;
  }

  private static string DecryptMessage(string message, char[] cryptogram, int shiftSize) {
    string     decrypted    = string.Empty;
    List<char> cryptogramLs = new(cryptogram);

    for (int i = 0; i < message.Length; i++) {
      int index = cryptogramLs.FindIndex(_c => _c == message[i]);
      decrypted += Alphabet[(i - shiftSize) % AlphabetSize];
    }

    return decrypted;
  }

  private static char[] SelectCryptogram() {
    Console.WriteLine("Input cryptogram");

    string line       = Console.ReadLine()!;
    char[] cryptogram = new char[AlphabetSize];
    int    j          = 0;
    foreach (var c in line.Where(c => c is > 'A' and < 'z')) {
      cryptogram[j++] = c.ToString().ToUpper()[0];
    }

    return cryptogram;
  }

  private static char[] UpdateCryptogramWithK(int k, char[] cryptogram) {
    char[] newCryptogram = new char[AlphabetSize];

    for (int i = 0; i < AlphabetSize; i++) {
      newCryptogram[i] = cryptogram[(i + k) % AlphabetSize];
    }

    return newCryptogram;
  }

  private static bool AddKeywordPrompt() {
    return Helper.SelectMultipleChoice<bool>(
      new() {
        new(true, "Yes"),
        new(false, "No")
      },
      title: "Add second key?"
    );
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
