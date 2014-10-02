using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Scheduler
{
    public static class TaskScheduler
    {
        private static List<WrapTask> taskScheduler;

        public static int Count
        {
            get { return taskScheduler.Count; }
        }

        static TaskScheduler()
        {
            taskScheduler = new List<WrapTask>();
        }

        /// <summary>
        /// 查找任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static WrapTask Find(string name)
        {
            return taskScheduler.Find(task => task.Name == name);
        }

        public static IEnumerator<WrapTask> GetEnumerator()
        {
            return taskScheduler.GetEnumerator();
        }

        /// <summary> 
        /// 终止任务 
        /// </summary> 
        public static void TerminateAllTask()
        {
            lock (taskScheduler)
            {
                taskScheduler.ForEach(task => task.Close());
                taskScheduler.Clear();
                taskScheduler.TrimExcess();
            }
        }

        internal static void Register(WrapTask task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Add(task);
            }
        }
        internal static void Deregister(WrapTask task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Remove(task);
            }
        }
    }
}
