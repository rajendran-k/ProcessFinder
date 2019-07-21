using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessFinder
{
    public class Analysis
    {
        private IEnumerable<Process> process;

        public string Cpu { get; set; }
        public string Handle { get; set; }
        public string PrivateMemory { get; set; }
        public string VirtualMemory { get; set; }
        public int MemoryLeak { get; set; }

        /// <summary>
        /// Search process is present in current context
        /// </summary>
        /// <param name="processName"> Name of the process to search</param>
        /// <returns>Return true if present</returns>
        public async Task<bool> SearchProcessNameAsync(string processName)
        {
            if (string.IsNullOrEmpty(processName))
            {
                throw new ArgumentException("Process name is empty");
            }

            bool flag = false;
            await Task.Run(() =>
            {
                var processList = System.Diagnostics.Process.GetProcessesByName(processName);
                var answers = processList.Any(t => string.Equals(processName, t.ProcessName, StringComparison.OrdinalIgnoreCase));
                if (answers)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            });

            return flag;
        }
        /// <summary>
        /// Getting process details
        /// </summary>
        /// <param name="duration">How much time data should get logged</param>
        /// <param name="sampleInterval"> Interval duration for checking the process</param>
        /// <param name="processName">Name of the process</param>
        /// <returns></returns>
        public Dictionary<string, int> GetProcessDetials(int duration, int sampleInterval, string processName)
        {
            if ((duration <= 0) || (sampleInterval < 0))
            {
                throw new ArgumentException("values are less than 0");
            }

            return GetEachParameterValues(processName, duration, sampleInterval);
        }

        public void DisplayResult(Dictionary<string, int> result)
        {
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.Key.Equals("Cpu", StringComparison.OrdinalIgnoreCase))
                    {
                        Cpu = item.Value.ToString();
                    }

                    if (item.Key.Equals("Handles", StringComparison.OrdinalIgnoreCase))
                    {
                        Handle = item.Value.ToString();
                    }

                    if (item.Key.Equals("Private Memory", StringComparison.OrdinalIgnoreCase))
                    {
                        PrivateMemory = item.Value.ToString();
                    }

                    if (item.Key.Equals("Virtual Memory", StringComparison.OrdinalIgnoreCase))
                    {
                        VirtualMemory = item.Value.ToString();
                    }

                    if (item.Key.Equals("Memory leak", StringComparison.OrdinalIgnoreCase))
                    {
                        MemoryLeak = item.Value;
                    }                    
                }
            }
        }

        /// <summary>
        /// Save the data coming from each process
        /// </summary>
        /// <param name="duration">How much time data should get logged</param>
        /// <param name="sampleInterval"> Interval duration for checking the process</param>
        /// <param name="processName">Name of the process</param>
        /// <returns></returns>
        public Dictionary<string, int> GetEachParameterValues(string processName, int duration, int sample)
        {
            if (string.IsNullOrEmpty(processName))
            {
                throw new ArgumentException("Process name is empty");
            }
            Dictionary<string, int> samplesCollected = new Dictionary<string, int>();
            DateTime endTime = DateTime.Now.AddMinutes(duration);
            var cpuvalue = new List<int>();
            var handles = new List<int>();
            var pmemory = new List<int>();
            var vmemory = new List<int>();

            while (endTime > DateTime.Now)
            {

                var process = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process processq in process)
                {
                    if (processq.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
                    {
                        var counter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
                        var HandleCountCounter = new PerformanceCounter("Process", "Handle Count", processq.ProcessName);
                        var privateMemory = new PerformanceCounter("Process", "Private Bytes", processq.ProcessName);
                        var virtualMemory = new PerformanceCounter("Process", "Virtual Bytes", processq.ProcessName);
                        var oo = counter.NextValue();
                        cpuvalue.Add((int)(counter.NextValue()));
                        handles.Add((int)HandleCountCounter.NextValue());
                        pmemory.Add((int)privateMemory.NextValue());
                        vmemory.Add((int)virtualMemory.NextValue());
                    }
                }
                Thread.Sleep(sample);
            }

            var cpuaverage = (int)cpuvalue.Average();
            var handlesaverage = (int)handles.Average();
            var memoryaverage = (int)pmemory.Average();
            var vmemoryaverage = (int)vmemory.Average();
            if((int)pmemory.Average() > 2*pmemory.First())
            {
                samplesCollected.Add("Memory leak", 1);
            }
            samplesCollected.Add("Cpu", (int)cpuaverage);
            samplesCollected.Add("Handles", (int)handlesaverage);
            samplesCollected.Add("Private Memory", (int)memoryaverage);
            samplesCollected.Add("Virtual Memory", (int)vmemoryaverage);
            return samplesCollected;
        }
    }
}
