using System.IO;
using NUnit.Framework;

namespace HtmlParser.Parser.Tests
{
    public static class TestExtensions
    {
        /// <summary>
        /// Возвращает путь к папке тестового проекта.
        /// <remarks>
        /// Начиная с NUnit 3, путь к текущей папке изменился.
        /// </remarks>
        /// </summary>
        /// <param name="testContext">Тестовый контекст</param>
        public static string GetTestProjectPath(this TestContext testContext)
        {
            var di = new DirectoryInfo(testContext.TestDirectory);
            var path = di.Parent.Parent.Parent.FullName;
            return path;
        }
    }
}