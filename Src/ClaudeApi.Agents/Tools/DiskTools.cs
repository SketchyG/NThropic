using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ClaudeApi.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace ClaudeApi.Agents.Tools
{
    public class DiskTools
    {
        private readonly Lazy<string> _sandboxBasePath;
        private readonly IServiceProvider _serviceProvider;

        public DiskTools(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _sandboxBasePath = new Lazy<string>(() =>
            {
                var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
                var path = configuration["Sanctuary:SandboxBasePath"];
                if (string.IsNullOrEmpty(path))
                    throw new InvalidOperationException("Sanctuary:SandboxBasePath is not set in the configuration.");

                return path;
            });
        }

        public string SandboxBasePath => _sandboxBasePath.Value;

        private string GetSafePath(string relativePath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(SandboxBasePath, relativePath));
            if (!fullPath.StartsWith(SandboxBasePath, StringComparison.OrdinalIgnoreCase))
                throw new SecurityException("Access to path outside of sandbox is not allowed.");

            return fullPath;
        }

        [Tool("read_file")]
        [Description("Reads the content of a file from the sandbox")]
        public async Task<string> ReadFileAsync(string relativePath)
        {
            var safePath = GetSafePath(relativePath);
            return await File.ReadAllTextAsync(safePath, Encoding.UTF8);
        }

        [Tool("write_file")]
        [Description("Writes content to a file in the sandbox, creating directories if needed")]
        public async Task WriteFileAsync(string relativePath, string content)
        {
            var safePath = GetSafePath(relativePath);
            var directoryPath = Path.GetDirectoryName(safePath) ?? throw new InvalidOperationException("The directory path could not be determined.");

            Directory.CreateDirectory(directoryPath);
            await File.WriteAllTextAsync(safePath, content, Encoding.UTF8);
        }

        [Tool("append_to_file")]
        [Description("Appends content to an existing file in the sandbox, creating directories if needed")]
        public async Task AppendToFileAsync(string relativePath, string content)
        {
            var safePath = GetSafePath(relativePath);
            var directoryPath = Path.GetDirectoryName(safePath) ?? throw new InvalidOperationException("The directory path could not be determined.");

            Directory.CreateDirectory(directoryPath);
            await File.AppendAllTextAsync(safePath, content, Encoding.UTF8);
        }

        [Tool("file_exists")]
        [Description("Checks if a file exists in the sandbox")]
        public bool FileExists(string relativePath)
        {
            var safePath = GetSafePath(relativePath);
            return File.Exists(safePath);
        }

        [Tool("create_directory")]
        [Description("Creates a new directory in the sandbox")]
        public async Task CreateDirectoryAsync(string relativePath)
        {
            var safePath = GetSafePath(relativePath);
            await Task.Run(() => Directory.CreateDirectory(safePath));
        }
    }
}
