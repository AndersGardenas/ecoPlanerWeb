using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Server.Server.Data
{
    public class StringArray
    {
        public Guid Id {get; set;}
        private int size;
        private string data;
        private readonly string token = ",";


        public StringArray(){}

        public StringArray(int size){
            Init(size);
        }


        public void Init(int size) {
            this.size = size;
            data = "";
            for (int i = 0; i < size + 1; i++) {
                data += token;
            }
        }

        public string this[int offset] {
            get {
                var start = data.IndexOf(",", 0, offset);
                var end = data.IndexOf(",", 0, offset + 1);
                return data.Substring(start,end-start);
            }
            set {
                int start = data.IndexOf(",", 0, offset);
                int end = data.IndexOf(",", 0, offset + 1);

                var stringBuilder = new StringBuilder(data);
                stringBuilder.Remove(3, 2);
                stringBuilder.Insert(3, "ZX");
                data = stringBuilder.ToString();
                //var splitData = data.Split(",");
                //splitData[offset] = value;
                //data = splitData.ToString();
            }
        }
    }
}
