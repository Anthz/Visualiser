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
        private float fov;
        private float nearPlane;
        private float farPlane;
        private float aspectRatio;
        private Matrix4 orientation;
        private Matrix4 viewMatrix;
        private Matrix4 projMatrix;
        private float xAngle;
        private float yAngle;

        public Vector3 Pos { get; set; }
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

        public Matrix4 CameraMatrix { get; set; }
        public Matrix4 ProjMatrix
        {
            get { return projMatrix; }
            set { projMatrix = value; }
        }

        public Matrix4 ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
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
                ResetProjMatrix();
            }
        }

        public float NearPlane
        {
            get { return nearPlane; }
            set
            {
                nearPlane = value;
                ResetProjMatrix();
            }
        }

        public float FarPlane
        {
            get { return farPlane; }
            set
            {
                farPlane = value;
                ResetProjMatrix();
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                ResetProjMatrix();
            }
        }

        public Camera(Vector3 pos, float xRotation, float yRotation,
                        float fov, float nearPlane, float farPlane, float aspectRatio)
        {
            Pos = pos;
            XAngle = xRotation;
            YAngle = yRotation;
            FOV = fov;
            NearPlane = nearPlane;
            FarPlane = farPlane;
            AspectRatio = aspectRatio;
            ResetProjMatrix();
            NewOrientation();
        }

        public void Update(long deltaTime)
        {
            CameraMatrix = projMatrix;
            CameraMatrix *= Orientation;
            CameraMatrix *= Matrix4.CreateTranslation(-Pos);
            
            OpenTKControl.shader.SetUniform("CameraMatrix", CameraMatrix);
        }

        public void ResetProjMatrix()
        {
            GL.Viewport(0, 0, OpenTKControl.openTKWindow.Size.Width, OpenTKControl.openTKWindow.Size.Height);
            if(fov != 0 && aspectRatio.Equals(0.0f) && farPlane != 0)
                projMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);
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
