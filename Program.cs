using CS_Labs.Helpers;
using CsLabs.Cipher;

var printer = new Printer();

printer
    .Text("Text (no symbols/digits, any case, space allowed) = ", ConsoleColor.Blue)
    .Prompt(out var text);

while (text is null || text.Length == 0 || !Vigenere.IsValidText(text)) {
  printer
      .Clear()
      .Text("Invalid text, input again", ConsoleColor.Red)
      .NewLine()
      .Prompt(out text);
}

printer
    .Text("Key (uppercase, no whitespace) = ", ConsoleColor.Blue)
    .Prompt(out var key);

while (key is null  || key.Length < Vigenere.MinKeyLength || !Vigenere.IsValidKey(key)) {
  printer
      .Clear()
      .Text("Invalid key, input again", ConsoleColor.Red)
      .NewLine()
      .Prompt(out key);
}

var cipher = new Vigenere(text, key);

bool isDecrypt = new SelectForm<bool>()
                 .Title("Select operation")
                 .AddOption(false, "Encrypt")
                 .AddOption(true,  "Decrypt")
                 .Render();

printer
    .Clear()
    .Text("Original text: ", ConsoleColor.Blue)
    .Text(cipher.OriginalText)
    .NewLine()
    .Text("Key: ", ConsoleColor.Blue)
    .Text(cipher.Key)
    .NewLine();

if (isDecrypt) {
  printer
      .Text("Decrypted text: ", ConsoleColor.Blue)
      .Text(cipher.Decrypt());
} else {
  printer
      .Text("Encrypted text: ", ConsoleColor.Blue)
      .Text(cipher.Encrypt());
}
