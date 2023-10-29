using System.Collections;

namespace CsLabs.Cipher;

public static class Des {
  // Original permutations table (length 32), index starts at 1
  private static readonly int[] PermutationTable = {
    16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13,
    30, 6, 22, 11, 4, 25
  };

  public static BitArray Permute(BitArray input) {
    var permuted = new BitArray(input.Length);

    for (int i = 0; i < input.Length; i++) {
      permuted[i] = input[PermutationTable[i] - 1];
    }

    return permuted;
  }

  public static BitArray RandomBits(int size) {
    var rand  = new Random();
    var bytes = new byte[size / 8];

    rand.NextBytes(bytes);

    var bits = new BitArray(bytes);

    return bits;
  }
}
