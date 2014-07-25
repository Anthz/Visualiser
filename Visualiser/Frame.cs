using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    class Frame
    {
        DataPoint[] dataPoints;
        string fileName;
        string data;
        private string _format;
        bool correctFormatting;

        public Frame(string fileName, string data, int lines)
        {
            this.fileName = fileName;
            this.data = data;
            dataPoints = new DataPoint[lines];
            correctFormatting = false;
        }

        public Frame(string fileName, string data, int lines, string format)
        {
            this.fileName = fileName;
            this.data = data;
            _format = format;
            dataPoints = new DataPoint[lines];

            //correctFormatting = FormatData();
        }

        public bool FormatData()
        {
            for(int i = 0; i < dataPoints.Length; i++)
            {
                //dataPoints[i] = new DataPoint(x, y, z, values);
            }

            return true;
        }

        public void Render()
        {

        }

        public void Update()
        {

        }
    }
}
