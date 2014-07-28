using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    class DataPoint
    {
        private float x, y, z, data1, data2, data3;

        public DataPoint(float x, float y, float z, float data1, float data2, float data3)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;
        }

        public void Render()
        {

        }

        public void Update()
        {

        }
    }
}
