# UnityGyroAccelCamera
Some way to make gyroscope and accelerometer work on all devices, in all Unity versions, using AHRS algoritm

Add the GyroCamera.cs and AccelerometerCamera.cs scripts to the camera.

Set the GyroCamera.isActive = true on Android and AccelerometerCamera.isActive = true for iOS.

As it uses AHRS algorithm to calculate the atitude, the solution works for all Unity, Android and iOS versions.

To fine tunning the drift when using AccelerometerCamera, you may want to change the rotationSpeed and AHRS variables values.
