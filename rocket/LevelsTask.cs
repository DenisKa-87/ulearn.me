//using DynamicData.Binding;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
	static readonly Physics standardPhysics = new();

	public static IEnumerable<Level> CreateLevels()
	{
		Vector standartTarget = new Vector(600, 200);
		Vector upTarget = new Vector(700, 500);
		Rocket standartRocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

		Gravity whiteG = (size, v) =>
		{
			var d = (standartTarget - v).Length;
			return (standartTarget - v).Normalize() * (-140 * d) / (d * d + 1);
		};

		Gravity blackG = (size, v) =>
		{
			var anomaly = (standartRocket.Location + standartTarget)/2;
			var d = (v - anomaly).Length;
			return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);

        };


        yield return new Level("Zero",
			standartRocket,
			standartTarget,
			(size, v) => Vector.Zero,
			standardPhysics);
		yield return new Level("Heavy",
			standartRocket,
			standartTarget,
			(size, v) => new Vector(0, 0.9),
			standardPhysics);
		yield return new Level("Up",
			standartRocket,
			upTarget,
			(size, v) => new Vector(0, -GetGravitationForce(size, v)),
			standardPhysics);
		yield return new Level("WhiteHole",
			standartRocket,
			standartTarget,
			whiteG,
			standardPhysics);
		yield return new Level("BlackHole",
			standartRocket,
			standartTarget,
			blackG,
			standardPhysics);
		yield return new Level("BlackAndWhite",
			standartRocket,
			standartTarget,
			(size, v) => (whiteG(standartRocket.Location,v)+blackG(standartRocket.Location,v))/2,
			standardPhysics);


	}

	private static double GetGravitationForce(Vector size, Vector v) => 300 / (size.Y - v.Y + 300);
	
	
}