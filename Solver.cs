using System.Collections;
using System.Diagnostics;
using System.Numerics;
using Dasync.Collections;

namespace PentonimoSolver;

public static class Solver
{
	public static async Task Solve()
	{

		var pentObjs = Pentonimos.GetPentonimos().Select(a => new PentonimoWithTransforms(a)).ToArray();

		var colVals = new BigInteger[600];

		for (int i = 0; i < 600; i++)
			colVals[i] = BigInteger.Pow(2, i);

		var objectGrids = pentObjs.GetObjectGrids(10, 10);
		BigInteger[,,] compatibilityArray = objectGrids.GenerateCompatibilityArray(ref colVals);

#if DEBUG
		// decimal countAll = 0;
		// foreach (var bitArray in compatibilityArray)
		// {
		// 	countAll += bitArray?.Length??0;
		// }
		// Console.WriteLine($"Finished generating compatibility data, {countAll} comparisons stored from {objectGrids.Sum(p => p.Length)} grids");
#else
		Console.WriteLine($"Finished generating compatibility data");
#endif
		using (var finish = new CancellationTokenSource())
		{
			var indexes = objectGrids[0].Select((_, i) => i).ToArray();

			var arrayTransformEquality = new ArrayTransformEquality(new ArrayEquality());

			indexes = indexes.Where(i =>
			{
				var thisGrid = objectGrids[0][i];
				for (int j = 0; j < i; j++)
				{
					if (arrayTransformEquality.Equals(objectGrids[0][j], thisGrid)) return false;
				}

				return true;
			}).ToArray();

			int denom = indexes.Length;
			int nom = 0;

			var lastPercentage = -1d;

			var sw = new Stopwatch();
			sw.Start();
			await indexes.ParallelForEachAsync(index => Task.Run(() =>
			{
				var levels = new BigInteger[objectGrids.Length - 1];

				for (int i = 0; i < (objectGrids.Length - 1); i++) levels[i] = compatibilityArray[0, index, i + 1];
				var checkedGrids = new List<byte[,]>();
				for (int i = 0; i < levels[0].GetBitLength(); i++)
				{
					if (finish.IsCancellationRequested) return;
					if ((levels[0] & BigInteger.Pow(2,i)) == 0)
					{
						Console.WriteLine($"Skipped {index}-{i}");
						continue;
					}

					var gridToCheck = objectGrids[0][index].CopyOf();
					gridToCheck.AddObject(objectGrids[1][i]);

					if (checkedGrids.Any(g => arrayTransformEquality.Equals(gridToCheck, g)))
					{
						Console.WriteLine($"Already checked transform of {index}-{i}");
						continue;
					}

					checkedGrids.Add(gridToCheck);

					var res = CheckRoute(ref compatibilityArray, ref objectGrids, ref colVals, 1, i, levels);

					if (res == null!)
					{
						Console.WriteLine($"Finished {index}-{i}");
						continue;
					}

					res.AddObject(objectGrids[0][index]);

					res.ToRender().PrintGrid();

					sw.Stop();
					Console.WriteLine($"Found solution in {sw.Elapsed}");
					finish.Cancel();

					return;
				}

				Interlocked.Increment(ref nom);
				Helpers.UpdatePercentageAndPrint(nom, denom, ref lastPercentage, sw.Elapsed);
			}, finish.Token), 64, finish.Token).ConfigureAwait(false);

		}
	}

	// compatibility map indexes:
	// level/pentomino, grid/permutation index, other pentomino, bitarray index value: compatibility with other pentomino's grid/permutation at this index
	public static byte[,] CheckRoute(ref BigInteger[,,] cMap, ref byte[][][,] objGrids, ref BigInteger[] colVals, int currentLevel, int gridIndex, BigInteger[] currentLevels)
	{
		var nextLevelsLength = objGrids.Length-currentLevel-1;
        
		if (nextLevelsLength == 0)
			return objGrids[currentLevel][gridIndex].CopyOf();

		var nextLevels = new BigInteger[nextLevelsLength]; //new BitArray[nextLevelsLength];
        
		for (int level = 0; level < nextLevelsLength; level++)
		{
			// var newArr = new BitArray(currentLevels[level+1]);
			// newArr.And(cMap[currentLevel,gridIndex,currentLevel+level+1]);
			// if (newArr.AllFalse()) return null!;
			nextLevels[level] =
				currentLevels[level + 1] & cMap[currentLevel, gridIndex, currentLevel + level + 1]; //newArr;
		}

		for (int i = 0; colVals[i] < nextLevels[0]; i++)
		{
			if ((nextLevels[0] & colVals[i]) == 0) continue;
            
			var result = CheckRoute(ref cMap, ref objGrids, ref colVals, currentLevel + 1, i, nextLevels);
			if (result == null!) continue;
                
			result.AddObject(objGrids[currentLevel][gridIndex]);
			return result;
		}

		return null!;
	}

}