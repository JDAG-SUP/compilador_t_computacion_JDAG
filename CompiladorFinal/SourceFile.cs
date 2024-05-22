using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    public class SourceFile
    {
        public string SourceCode { get; private set; }

        public SourceFile(string filePath)
        {
            try
            {
                SourceCode = File.ReadAllText(filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
            }
        }
    }
}