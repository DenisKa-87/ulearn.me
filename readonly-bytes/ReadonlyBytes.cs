using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private byte[] data;
        private int hash = 1677711;
        private readonly int fnvPrime = 2166131;
        public ReadonlyBytes(params byte[] args)
        {
            if (args == null)
                throw new ArgumentNullException();
            data = new byte[args.Length];
            Array.Copy(args, data, args.Length);
            hash = CalcHashCode();
        }

        public int Length { get => data.Length; }
        public byte this[int ix] { get => data[ix]; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
           GetEnumerator();

        public virtual IEnumerator<byte> GetEnumerator() =>
            ((IEnumerable<byte>)data).GetEnumerator();

        public override int GetHashCode() => hash;


        public int CalcHashCode()
        {
            foreach (var b in data)
            {
                unchecked
                {
                    hash = hash * fnvPrime;
                    hash = hash ^ b;
                }
            }
            return hash;
        }

        bool Equals(ReadonlyBytes data)
        {
            if (data.Length != this.Length)
                return false;
            if (data.GetHashCode() != this.hash)
                return false;
            for (int i = 0; i < this.Length; i++)
                if (data[i] != this[i]) 
                    return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return this.Equals((ReadonlyBytes)obj);
        }

        public override string ToString()
        {
            if (this.data.Length == 0) 
                return "[]";
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString() + " ");
            }
            sb.Replace(" ", ", ");
            sb.Remove(sb.Length - 2, 1);
            return "[" + sb.ToString().Trim() + "]";
        }
    }
}