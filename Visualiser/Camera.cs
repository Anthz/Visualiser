using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    public class Camera
    {
        public Vector3 pos;
        private float fov;
        private float nearPlane;
        private float farPlane;
        private float aspectRatio;
        private Matrix4 orientation;
        private Matrix4 viewMatrix;
        public Matrix4 projMatrix;
        private float xAngle;
        private float yAngle;
        public OculusRift rift;
        
        public float XAngle
        {
            get { return xAngle; }
            set
            {
                xAngle = value;
                Orientation = NewOrientation();
            }
        }

        public float YAngle
        {
            get { return yAngle; }
            set
            {
                yAngle = value;
                Orientation = NewOrientation();
            }
        }

        public Matrix4 Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public float FOV
        {
            get { return fov; }
            set
            {
                float i = value;
                fov = (float)MathHelper.Clamp(i, 0.0, 180.0);
                //ResetProjMatrix();
            }
        }

        public float NearPlane
        {
            get { return nearPlane; }
            set
            {
                nearPlane = value;
                //ResetProjMatrix();
            }
        }

        public float FarPlane
        {
            get { return farPlane; }
            set
            {
                farPlane = value;
                //ResetProjMatrix();
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                //ResetProjMatrix();
            }
        }

        public Camera(Vector3 pos, float xRotation, float yRotation,
                        float fov, float nearPlane, float farPlane, float aspectRatio)
        {
            this.pos = pos;
            XAngle = xRotation;
            YAngle = yRotation;
            FOV = fov;
            NearPlane = nearPlane;
            FarPlane = farPlane;
            AspectRatio = aspectRatio;
            projMatrix = Matrix4.Identity;
            rift = new OculusRift();
            NewOrientation();
        }

        public void Update(long deltaTime)
        {
            viewMatrix = Orientation;
            if (OpenTKControl.RiftEnabled)
                viewMatrix *= Matrix4.CreateFromQuaternion(rift.PredictedOrientation);
            viewMatrix *= Matrix4.CreateTranslation(-pos);
            
            OpenTKControl.shader.SetUniform("ViewMatrix", ref viewMatrix);
            if(OpenTKControl.RiftEnabled)
                OpenTKControl.openTKWindow.Invalidate();
        }

        public void ResetProjMatrix()
        {
            GL.Viewport(0, 0, OpenTKControl.openTKWindow.Size.Width, OpenTKControl.openTKWindow.Size.Height);
            if (fov != 0 && !aspectRatio.Equals(0.0f) && farPlane != 0)
            {
                projMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, (float)OpenTKControl.openTKWindow.Size.Width / (float)OpenTKControl.openTKWindow.Size.Height, nearPlane, farPlane);
                OpenTKControl.shader.SetUniform("ProjectionMatrix", ref projMatrix);
            }
        }

        public void ResetCamera()
        {
            pos = new Vector3(0, 100, 100);
            orientation = Matrix4.LookAt(pos, new Vector3(0), new Vector3(0, 1, 0));
        }

        public void ResetRiftOrientation()
        {
            rift.Dispose();
            rift = new OculusRift();
        }

        private Matrix4 NewOrientation()
        {
            Matrix4 newOrientation = Matrix4.Identity;
            newOrientation *= Matrix4.CreateRotationX(YAngle);
            newOrientation *= Matrix4.CreateRotationY(XAngle);
            return newOrientation;
        }
    }
}
