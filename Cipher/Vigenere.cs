using System.Text;

namespace CsLabs.Cipher;

public class Vigenere {
  public const int MinKeyLength = 3;

  private static readonly List<char> _alphabet = new() {
      'A', 'Ă', 'Â', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'Î', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
      'Q', 'R', 'S', 'Ș', 'T', 'Ț', 'U', 'V', 'W', 'X', 'Y', 'Z'
  };

  public string Key { get; }

  public string OriginalText { get; }

  private readonly string _parsedText;

  public Vigenere(string text, string key) {
    OriginalText = text;
    Key          = key;

    var sb = new StringBuilder();

    foreach (var c in text.ToUpper().Where(c => !IsWhitespace(c))) {
      sb.Append(c);
    }

    _parsedText = sb.ToString();
  }

  public string Encrypt() {
    var encryptedSb = new StringBuilder();

    for (int i = 0; i < _parsedText.Length; i++) {
      char c = _parsedText[i];
      int  cIndex     = _alphabet.IndexOf(c);
      int  keyCIndex  = _alphabet.IndexOf(Key[i % Key.Length]);
      int  newCIndex  = (cIndex + keyCIndex) % _alphabet.Count;
      char encryptedC = Convert.ToChar(_alphabet[newCIndex]);

      encryptedSb.Append(encryptedC);
    }

    return encryptedSb.ToString();
  }

  public string Decrypt() {
    var decryptedSb = new StringBuilder();

    for (int i = 0; i < _parsedText.Length; i++) {
      char c          = _parsedText[i];
      int  cIndex     = _alphabet.IndexOf(c);
      int  keyCIndex  = _alphabet.IndexOf(Key[i % Key.Length]);
      int  newCIndex  = (cIndex - keyCIndex + _alphabet.Count) % _alphabet.Count;
      char decryptedC = Convert.ToChar(_alphabet[newCIndex]);

      decryptedSb.Append(decryptedC);
    }

    return decryptedSb.ToString();
  }

  public static bool IsValidText(string text) {
    foreach (char c in text) {
      char upperC = c.ToString().ToUpper()[0];

      if (upperC != ' ' && !_alphabet.Contains(upperC)) {
        return false;
      }
    }

    return true;
  }

  public static bool IsValidKey(string key) {
    return key.All(c => _alphabet.Contains(c));
  }

  private static bool IsWhitespace(char c) {
    return c is ' ' or '\t' or '\r' or '\n';
  }
}
