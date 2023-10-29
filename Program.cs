using System.Collections;
using System.Globalization;
using CS_Labs.Helpers;
using CsLabs.Cipher;

var p = new Printer();
bool isRandom = new SelectForm<bool>()
                .Title("Input form")
                .AddOption(true,  "Random")
                .AddOption(false, "Manual")
                .Render();

BitArray bits;     // S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8)
BitArray permuted; // P(S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8))
BitArray prevL;    // L(i - 1)

var rBytes = new byte[4]; // byte array of R to ease working with bits

if (isRandom) {
  bits  = Des.RandomBits(32);
  prevL = Des.RandomBits(32);
} else {
  PromptValues(out bits, out prevL);
}

permuted = Des.Permute(bits);
prevL.CopyTo(rBytes, 0);

var r = new BitArray(rBytes); // R
r.Xor(permuted).CopyTo(rBytes, 0);

p.Clear();
PrintBytes(bits,     "S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8):");
PrintBytes(permuted, "P(S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8)):");
PrintBytes(prevL,    "L(i - 1):");
PrintBytes(r,        "R(i):");

return;


void PrintBytes(BitArray bitsArray, string title) {
  var bytes = new byte[bitsArray.Length / 8];

  bitsArray.CopyTo(bytes, 0);

  p.Text(title, ConsoleColor.Green).NewLine();

  foreach (byte b in bytes) {
    var value = (uint)b;
    p.Text($"{value:x2} ", ConsoleColor.Blue);
  }

  p.NewLine();
}

void PromptValues(out BitArray bits, out BitArray prevL) {
  var      bytes = new byte[4];
  string   text;
  string[] splitText;

  p
    .Clear()
    .Text("S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8) (hexadecimal separated by spaces):", ConsoleColor.DarkCyan)
    .NewLine()
    .Prompt(out text);

  splitText = text.Split(' ');

  for (int i = 0; i < 4; i++) {
    bytes[i] = (byte) int.Parse(splitText[i], NumberStyles.HexNumber);
  }

  bits = new BitArray(bytes);

  p
    .Clear()
    .Text("L(i - 1) (hexadecimal separated by spaces):", ConsoleColor.DarkCyan)
    .NewLine()
    .Prompt(out text);

  splitText = text.Split(' ');

  for (int i = 0; i < 4; i++) {
    bytes[i] = (byte) int.Parse(splitText[i], NumberStyles.HexNumber);
  }

  prevL = new BitArray(bytes);
}
