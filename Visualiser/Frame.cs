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
                float x, y, z;
                List<float> dataValues = new List<float>();
                string[] splitLine = data[i].Split(' ');

                if(MainWindow.format.Count != splitLine.Length)
                    return false;

                try
                {
                    x = MainWindow.format.ContainsKey("x") ? float.Parse(splitLine[MainWindow.format["x"]]) : 0;
                    y = MainWindow.format.ContainsKey("y") ? float.Parse(splitLine[MainWindow.format["y"]]) : 0;
                    z = MainWindow.format.ContainsKey("z") ? float.Parse(splitLine[MainWindow.format["z"]]) : 0;

                    for(int j = 3; j < splitLine.Length; j++)
                    {
                        float temp = MainWindow.format.ContainsKey((j - 2).ToString()) ? float.Parse(splitLine[MainWindow.format[(j - 2).ToString()]]) : 0;
                        dataValues.Add(temp);
                    }
                }
                catch(Exception)
                {
                    return false;
                }

                dataPoints[i] = new DataPoint(x, y, z, dataValues.ToArray(), );
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
