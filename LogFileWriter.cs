using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KeyEventLogger
{
    /// <summary>Kayıtları yerel bir .txt dosyasına yazar.</summary>
    public static class LogFileWriter
    {
        public static void Save(string path, IEnumerable<KeyRecord> records)
        {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("Key Event Logger - Kayıt Dosyası");
                writer.WriteLine($"Oluşturulma: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('-', 40));
                foreach (var r in records)
                    writer.WriteLine(r.ToString());
            }
        }
    }
}
