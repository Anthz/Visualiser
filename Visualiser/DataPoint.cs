using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    class DataPoint
    {
        private float x, y, z;
        float[] dataValues;

        public DataPoint(float x, float y, float z, float[] dataValues)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.dataValues = dataValues;
        }
        public void Render()
        {

        }

        public void Update()
        {

        }
    }
}
