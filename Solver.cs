using System.Collections;
using System.Diagnostics;
using Dasync.Collections;

namespace PentonimoSolver;

public static class Solver
{
	public static async Task Solve()
	{

		var pentObjs = Pentonimos.GetPentonimos().Select(a => new PentonimoWithTransforms(a)).ToArray();

		var objectGrids = pentObjs.GetObjectGrids(10, 10);
		BitArray[,,] compatibilityArray = objectGrids.GenerateCompatibilityArray();
		
#if DEBUG
		decimal countAll = 0;
		foreach (var bitArray in compatibilityArray)
		{
			countAll += bitArray?.Length??0;
		}
		Console.WriteLine($"Finished generating compatibility data, {countAll} comparisons stored from {objectGrids.Sum(p => p.Length)} grids");
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
				var levels = new BitArray[objectGrids.Length - 1];

				for (int i = 0; i < (objectGrids.Length - 1); i++) levels[i] = compatibilityArray[0, index, i + 1];
				var checkedGrids = new List<byte[,]>();
				for (int i = 0; i < levels[0].Count; i++)
				{
					if (finish.IsCancellationRequested) return;
					if (!levels[0][i])
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

					var res = Helpers.CheckRoute(ref compatibilityArray, ref objectGrids, 1, i, levels);

					if (res == null!)
					{
						Console.WriteLine($"Finished {index}-{i}");
						continue;
					}

					res.AddObject(objectGrids[0][index]);

					res.ToRender().PrintGrid();

					finish.Cancel();

					return;
				}

				Interlocked.Increment(ref nom);
				Helpers.UpdatePercentageAndPrint(nom, denom, ref lastPercentage, sw.Elapsed);
			}, finish.Token), 64, finish.Token).ConfigureAwait(false);

			// Console.WriteLine($"Queued {indexes.Length} indexes");
			//await Task.WhenAll(tasks).ConfigureAwait(false);
			sw.Stop();
		}
	}


}