using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Raconteur.Helpers;

namespace Raconteur.Parsers
{
    public interface ScenarioParser
    {
        Scenario ScenarioFrom(List<string> Definition);
    }

    public class ScenarioParserClass : ScenarioParser
    {
        public StepParser StepParser { get; set; }

        public List<string> Definition;

        public Scenario ScenarioFrom(List<string> Definition)
        {
            this.Definition = Definition;

            return new Scenario
            {
                Tags = Tags,
                Name = Name,
                Steps = Steps,
                Examples = Examples
            };
        }

        public List<string> Tags
        {
            get
            {
                return Definition
                    .TakeWhile(IsNotDeclaration)
                    .SelectMany(TagsInLine)
                    .Distinct()
                    .ToList();
            }
        }

        protected IEnumerable<string> TagsInLine(string Line) 
        {
            return 
                from Tag in Regex.Split(Line, "@")
                where !Tag.IsEmpty()
                select Tag.Trim();
        }

        string Name
        {
            get
            {
                return Definition
                    .SkipWhile(IsNotDeclaration)
                    .First()
                    .YamlIdentifier();    
            } 
        }

        public List<Step> Steps
        {
            get
            {
                return Definition
                    .SkipWhile(IsNotDeclaration)
                    .Skip(1)
                    .TakeWhile(IsNotExample)
                    .Select(Step)
                    .WhereIsNotNull()
                    .ToList();
            }
        }

        Step Step(string Line)
        {
            var StepFrom = StepParser.StepFrom(Line);
            return StepFrom;
        }

        bool InsideArg;

        protected bool IsNotDeclaration(string Line)
        {
            return !Line.StartsWith(Settings.Language.Scenario);
        }

        List<string> ParseTableRow(string Row)
        {
            return Row
                .Split('|')
                .Chop(1)
                .Select(x => x.Trim())
                .ToList();
        }

        #region examples
		        
        protected List<Table> Examples
        {
            get
            {
                var examples = new List<Table>();

                foreach (var Line in Definition.SkipWhile(IsNotExample))
                    if (IsExample(Line)) examples.Add
                    (
                        new Table { Name = Line.YamlIdentifier()}
                    );
                    else examples.Last().Add(ParseTableRow(Line));

                return examples;
            }
        }

        protected bool IsExample(string Line)
        {
            return !IsNotExample(Line);
        }

        protected bool IsNotExample(string Line)
        {
            if (Line == "\"") InsideArg = !InsideArg;

            return InsideArg || !Line.StartsWith(Settings.Language.Examples);
        }

	    #endregion    
    }
}