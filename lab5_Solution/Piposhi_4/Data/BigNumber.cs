using System.Text;
using System.Text.Json.Serialization;

namespace lab_4_ShN.Data
{
    public class BigNumber
    {
        private const int Base = 1000;
        private const int BlockDigits = 3;
        private readonly int[] number;

        private BigNumber(int[] blocks)
        {
            int start = 0;
            while (start < blocks.Length - 1 && blocks[start] == 0)
                start++;

            if (start == 0)
            {
                number = blocks;
            }
            else
            {
                int[] trimmed = new int[blocks.Length - start];
                Array.Copy(blocks, start, trimmed, 0, trimmed.Length);
                number = trimmed;
            }
        }

        public BigNumber Clone()
        {
            return new BigNumber((int[])this.number.Clone());
        }

        public BigNumber(int value)
        {
            if (value == 0)
            {
                number = new int[] { 0 };
                return;
            }

            var blocks = new List<int>();
            int v = value;
            while (v > 0)
            {
                blocks.Add(v % Base);
                v /= Base;
            }
            blocks.Reverse();
            number = blocks.ToArray();
        }

        [JsonConstructor]
        public BigNumber(string value)
        {
            value = value.Trim();
            if (value.Length == 0) value = "0";

            var blocks = new List<int>();

            int firstLen = value.Length % BlockDigits;
            if (firstLen == 0) firstLen = BlockDigits;

            blocks.Add(int.Parse(value.Substring(0, firstLen)));

            int pos = firstLen;
            while (pos < value.Length)
            {
                blocks.Add(int.Parse(value.Substring(pos, BlockDigits)));
                pos += BlockDigits;
            }

            int start = 0;
            while (start < blocks.Count - 1 && blocks[start] == 0)
                start++;

            if (start > 0)
                blocks.RemoveRange(0, start);

            number = blocks.ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < number.Length; i++)
            {
                if (i == 0)
                    sb.Append(number[i].ToString());
                else
                    sb.Append(number[i].ToString("D3"));
            }

            return sb.ToString();
        }

        public static int Compare(BigNumber a, BigNumber b)
        {
            if (a.number.Length > b.number.Length) return 1;
            if (a.number.Length < b.number.Length) return -1;

            for (int i = 0; i < a.number.Length; i++)
            {
                if (a.number[i] > b.number[i]) return 1;
                if (a.number[i] < b.number[i]) return -1;
            }
            return 0;
        }

        public static bool operator ==(BigNumber a, BigNumber b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return Compare(a, b) == 0;
        }
        public static bool operator !=(BigNumber a, BigNumber b) => !(a == b);

        public static bool operator <(BigNumber a, BigNumber b)
        {
            if (a is null || b is null) return false;
            return Compare(a, b) < 0;
        }
        public static bool operator >(BigNumber a, BigNumber b)
        {
            if (a is null || b is null) return false;
            return Compare(a, b) > 0;
        }
        public static bool operator <=(BigNumber a, BigNumber b)
        {
            if (a is null || b is null) return false;
            return Compare(a, b) <= 0;
        }
        public static bool operator >=(BigNumber a, BigNumber b)
        {
            if (a is null || b is null) return false;
            return Compare(a, b) >= 0;
        }

        public override bool Equals(object obj) => obj is BigNumber bn && this == bn;
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (int block in number)
                    hash = hash * 31 + block;
                return hash;
            }
        }
        public static readonly BigNumber Zero = new BigNumber(0);
        public static readonly BigNumber One = new BigNumber(1);

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            int maxLen = Math.Max(a.number.Length, b.number.Length);
            int[] result = new int[maxLen + 1];

            int carry = 0;
            int aIndex = a.number.Length - 1;
            int bIndex = b.number.Length - 1;
            int resIndex = result.Length - 1;

            while (aIndex >= 0 || bIndex >= 0 || carry > 0)
            {
                int sum = carry;

                if (aIndex >= 0) sum += a.number[aIndex--];
                if (bIndex >= 0) sum += b.number[bIndex--];

                carry = sum / Base;
                result[resIndex--] = sum % Base;
            }

            return new BigNumber(result);
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            if (a < b) return new BigNumber(0);

            int[] result = new int[a.number.Length];
            int borrow = 0;

            int aIndex = a.number.Length - 1;
            int bIndex = b.number.Length - 1;
            int resIndex = result.Length - 1;

            while (aIndex >= 0)
            {
                int diff = a.number[aIndex] - borrow;

                if (bIndex >= 0)
                    diff -= b.number[bIndex];

                if (diff < 0)
                {
                    diff += Base;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result[resIndex] = diff;
                aIndex--;
                bIndex--;
                resIndex--;
            }

            return new BigNumber(result);
        }

        public static BigNumber operator *(BigNumber a, int multiplier)
        {
            if (multiplier == 0) return new BigNumber(0);
            if (multiplier == 1) return a;

            int[] result = new int[a.number.Length + 1];
            long carry = 0;
            int resIndex = result.Length - 1;

            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                long product = (long)a.number[i] * multiplier + carry;

                carry = product / Base;
                result[resIndex--] = (int)(product % Base);
            }

            if (carry > 0)
                result[resIndex] = (int)carry;

            return new BigNumber(result);
        }

        private static BigNumber Multiply(BigNumber a, BigNumber b)
        {
            int[] result = new int[a.number.Length + b.number.Length];
            for (int i = a.number.Length - 1; i >= 0; i--)
            {
                int carry = 0;
                for (int j = b.number.Length - 1; j >= 0; j--)
                {
                    int current = result[i + j + 1] + a.number[i] * b.number[j] + carry;
                    result[i + j + 1] = current % Base;
                    carry = current / Base;
                }
                result[i] += carry;
            }
            return new BigNumber(result);
        }

        public static BigNumber operator *(BigNumber a, BigNumber b) => Multiply(a, b);

        private static BigNumber Divide(BigNumber a, BigNumber b)
        {
            if (b == new BigNumber(0))
                throw new DivideByZeroException();

            if (a < b) return new BigNumber(0);

            BigNumber quotient = new BigNumber(0);
            BigNumber current = new BigNumber(0);

            for (int i = 0; i < a.number.Length; i++)
            {
                current = current * new BigNumber(Base) + new BigNumber(a.number[i]);

                int count = 0;
                while (current >= b)
                {
                    current = current - b;
                    count++;
                }
                quotient = quotient * new BigNumber(Base) + new BigNumber(count);
            }
            return quotient;
        }

        public static BigNumber operator /(BigNumber a, BigNumber b) => Divide(a, b);

        public static BigNumber MultiplyPercent(BigNumber value, int percent)
        {
            if (percent == 100) return value;

            BigNumber multiplied = value * new BigNumber(percent);
            return multiplied / new BigNumber(100);
        }

        public static implicit operator BigNumber(int value) => new BigNumber(value);

    }
}