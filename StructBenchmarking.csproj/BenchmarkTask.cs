using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics;
using System.Text;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            var stopWatch = new Stopwatch();
            task.Run();
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            stopWatch.Start();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            stopWatch.Stop();
            return stopWatch.Elapsed.TotalMilliseconds / repetitionCount;
        }
    }

    public class SbTest : ITask
    {
        public void Run()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 1000; i++)
            {
                sb.Append("a");
            }
            sb.ToString();
        }
    }

    public class StrTest : ITask
    {
        public void Run()
        {
            var str = new string('a', 1000);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var strTest = new StrTest();
            var SbTest = new SbTest();
            var benchmark = new Benchmark();
            var strTime = benchmark.MeasureDurationInMs(strTest, 10000);
            var sbTime = benchmark.MeasureDurationInMs(SbTest, 10000);
            Assert.Less(strTime, sbTime);
        }
    }
}