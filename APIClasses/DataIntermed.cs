using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses
{
    public class DataIntermed
    {
        public int index;
        public int bal;
        public uint acct;
        public uint pin;
        public string fName;
        public string lName;
        public byte[] profileImg;

        public DataIntermed()
        {
            index = -1;
            acct= 0;
            pin = 0;
            bal= 0;
            fName = "";
            lName = "";
            profileImg = null;
        }
    }
}
