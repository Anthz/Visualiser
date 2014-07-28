using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    class Frame
    {
        string fileName;
        string[] data;
        public string displayData;
        DataPoint[] dataPoints;

        public Frame(string fileName, string[] data, string displayData)
        {
            this.fileName = fileName;
            this.data = data;
            this.displayData = displayData;
            dataPoints = null;
        }

        public bool FormatData()
        {
            //format - 0 = no value
            //1-> order of variables
            dataPoints = new DataPoint[data.Length];
            for(int i = 0; i < dataPoints.Length; i++)
            {
                float x, y, z, data1, data2, data3;
                string[] splitLine = data[i].Split(' ');
                try
                {
                    x = MainWindow.format.x == 0 ? 0 : float.Parse(splitLine[MainWindow.format.x - 1]);
                    y = MainWindow.format.y == 0 ? 0 : float.Parse(splitLine[MainWindow.format.y - 1]);
                    z = MainWindow.format.z == 0 ? 0 : float.Parse(splitLine[MainWindow.format.z - 1]);
                    data1 = MainWindow.format.data1 == 0 ? 0 : float.Parse(splitLine[MainWindow.format.data1 - 1]);
                    data2 = MainWindow.format.data2 == 0 ? 0 : float.Parse(splitLine[MainWindow.format.data2 - 1]);
                    data3 = MainWindow.format.data3 == 0 ? 0 : float.Parse(splitLine[MainWindow.format.data3 - 1]);
                }
                catch(Exception)
                {
                    throw;
                }
                dataPoints[i] = new DataPoint(x, y, z, data1, data2, data3);
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
