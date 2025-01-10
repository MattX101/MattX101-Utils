using Utils.Noise.Profiles;

namespace Utils.Noise
{
    public sealed class FastNoise2D
    {
        public void Generate(ref float[] noiseMap, int resX, int resY, NoiseProfile noiseProfile, WarpProfile warpProfile, bool is3D, bool useGPU)
        {
            if (useGPU)
            {
                FastNoise2DGPU.GenerateNoise(ref noiseMap, resX, resY, is3D, noiseProfile, warpProfile);
            }
            else
            {
                FastNoise2DCPU.GenerateNoise2D(ref noiseMap, resX, resY, is3D, noiseProfile, warpProfile);
            }
        }
    }
}
