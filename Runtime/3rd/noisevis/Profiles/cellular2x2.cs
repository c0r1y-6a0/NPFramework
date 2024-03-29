using Unity.Burst;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Serialization;
using static Unity.Mathematics.math;
using static Unity.Mathematics.noise;

[CreateAssetMenu(menuName = "NP/Noise/cellular2x2")]
public class cellular2x2 : NoiseProfile
{
	public enum Signature
	{
		[InspectorName("(float2 P):float2")]
		_0,
	}

	public enum OutputType : int
	{
		[InspectorName("x")]
		X = 0,
		[InspectorName("y")]
		Y = 1
	}

	public Signature signature;
	public OutputType outputType;
	
	public override JobHandle Render(NativeArray<Color32> colors, int2 resolution, float frequency)
	{
		return new cellular2x2Job {res = resolution, frequency = frequency, channel = (int) outputType, colors = colors}.Schedule(resolution.AsArrayLength(), 64);
	}
	
	[BurstCompile]
	private struct cellular2x2Job : IJobParallelFor
	{
		public int2 res;
		public float frequency;
		public int channel;
		public NativeArray<Color32> colors;

		public void Execute(int index)
		{
			float2 uv = index.ToUV(res) * frequency;
			float n = cellular2x2(uv)[channel];
			float4 color = float4(n, n, n, 1f);
			colors[index] = color.As32();
		}
	}
}