using Carubbi.Utils.Persistence;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Carubbi.Utils.Diagnostics
{
    public class MutexRunner
    {
        private readonly string _processFullPath;

        /// <summary>
        /// Execute a regular process with mutex behavior
        /// </summary>
        /// <param name="processFullPath"></param>
        public MutexRunner(string processFullPath)
        {
            _processFullPath = processFullPath;
        }

        /// <summary>
        /// Run the process
        /// </summary>
        /// <typeparam name="TArguments">Type of the arguments object</typeparam>
        /// <param name="arguments">Object to be serialized and passed as parameter</param>
        public void Run<TArguments>(TArguments arguments = null) 
            where TArguments : class, new()
        {
            var preparedArguments = string.Empty;
            if (arguments != null)
            {
                if (!typeof(TArguments).IsSerializable && !(typeof(ISerializable).IsAssignableFrom(typeof(TArguments))))
                    throw new InvalidOperationException("A serializable Type is required");

                var argumentsSerializer = new Serializer<TArguments>();
                var serializedArguments = argumentsSerializer.XmlSerialize(arguments)
                    .Replace(Environment.NewLine, string.Empty);

                preparedArguments = $"\"{HttpUtility.HtmlEncode(serializedArguments)}\"";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = _processFullPath
            };

            if (IsRunning) return;

            startInfo.Arguments = preparedArguments;
            startInfo.CreateNoWindow = true;

            Process.Start(startInfo);
        }

        /// <summary>
        /// Get the process' instance
        /// </summary>
        public Process GetProcess => Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_processFullPath)).FirstOrDefault();

        /// <summary>
        /// Check if the process is running
        /// </summary>
        public bool IsRunning => GetProcess != null;

        /// <summary>
        /// Kill the process
        /// </summary>
        public void Kill()
        {
            if (IsRunning)
            {
                GetProcess.Kill();
            }
        }
    }
}
