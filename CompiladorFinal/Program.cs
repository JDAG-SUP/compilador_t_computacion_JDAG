using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CompiladorFinal
{
    class Program
    {
        // Ruta del archivo de código fuente
        private const string SourceFilePath = "C:\\Users\\supap\\Downloads\\codigo.txt";

        private static void Main(string[] args)
        {
            // Verificar si se proporcionó la ruta del archivo como argumento
            string sourceFilePath = args.Length > 0 ? args[0] : SourceFilePath;

            // Leer el contenido del archivo de texto
            SourceFile sourceFile = new SourceFile(sourceFilePath);

            // Crear una instancia del analizador léxico
            Lexer lexer = new Lexer(sourceFile.SourceCode);

            // Crear una instancia del analizador sintáctico
            Parser parser = new Parser(lexer);

            try
            {
                // Analizar el código fuente
                parser.Parse();
                Console.WriteLine("Análisis completado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}