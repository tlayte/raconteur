using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Raconteur.Parsers
{
    public class ScenarioTokenizerClass : ScenarioTokenizer
    {
        public ScenarioParser ScenarioParser { get; set; }

        public string Content;

        public List<Scenario> ScenariosFrom(string Content)
        {
            this.Content = Content;

            return 
            (
                from Definition in ScenarioDefinitions
                select ScenarioParser.ScenarioFrom(Definition)
            ).ToList();
        }

        public List<List<string>> ScenarioDefinitions
        {
            get
            {
                var Results = new List<List<string>>();

                foreach (var Line in Lines)
                {
                    if (IsScenarioDeclaration(Line))
                        Results.Add(new List<string>());

                    Results.Last().Add(Line);
                }

                return Results;
            }
        }

        const string ScenarioDeclaration = "Scenario";

        IEnumerable<string> Lines
        {
            get
            {
                return Regex.Split(Content, Environment.NewLine)
                    .SkipWhile(IsNotScenarioDeclaration)
                    .Select(Line => Line.Trim())
                    .Where(IsNotEmpty);
            }
        }

        bool InsideArg;

        bool IsNotEmpty(string Line)
        {
            if (Line.StartsWith("\"")) InsideArg = !InsideArg;

            return InsideArg || !string.IsNullOrWhiteSpace(Line);
        }

        bool IsScenarioDeclaration(string Line)
        {
            return !InsideArg && Line.TrimStart().StartsWith(ScenarioDeclaration);
        }

        bool IsNotScenarioDeclaration(string Line)
        {
            return !IsScenarioDeclaration(Line);
        }
    }
}