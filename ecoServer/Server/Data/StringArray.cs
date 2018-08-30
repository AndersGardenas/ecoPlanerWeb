using System;
using System.Text;

namespace Server.Server.Data
{
    public class StringArray
    {
        public Guid Id { get; set; }
        public int Size { get; set; }
        public string Data { get; set; }
        private readonly char token = ',';


        public StringArray() { }

        public StringArray(int size) {
            Init(size);
        }


        public void Init(int size) {
            this.Size = size;
            Data = "";
            for (int i = 0; i < size + 1; i++) {
                Data += token;
            }
        }

        public string this[int offset] {
            get {
                var start = GetNthIndex(Data, token, 1 + offset);
                var end   = GetNthIndex(Data, token, 2 + offset);
                return Data.Substring(start + 1, end - start - 1);
            }
            set {
                var start = GetNthIndex(Data, token, 1 + offset);
                var end   = GetNthIndex(Data, token, 2 + offset);

                var stringBuilder = new StringBuilder(Data);
                stringBuilder.Remove(start + 1, end - start - 1);
                stringBuilder.Insert(start + 1, value);
                Data = stringBuilder.ToString();
            }
        }

        public int GetNthIndex(string s, char t, int n) {
            int count = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == t) {
                    count++;
                    if (count == n) {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
