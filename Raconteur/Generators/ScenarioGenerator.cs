﻿using System.Linq;

namespace Raconteur.Generators
{
    public class ScenarioGenerator : CodeGenerator
    {
        private const string ScenarioDeclaration = 
@"        
        [TestMethod]
        public void {0}()
        {{ {1}
        }}
";

        readonly Scenario Scenario;

        public ScenarioGenerator(Scenario Scenario) 
        {
            this.Scenario = Scenario;
        }

        public string Code
        {
            get
            {
                return Scenario.IsOutline ?
                    OutlineScenarioCode :
                    ScenarioCode;
            }
        }

        string ScenarioCode
        {
            get
            {
                var StepCode = Scenario.Steps.Aggregate(string.Empty, 
                    (Steps, Step) => Steps + CodeFor(Step));

                return string.Format(ScenarioDeclaration, Scenario.Name, StepCode);
            }
        }

        string CodeFor(Step Step) { return new StepGenerator(Step).Code; }

        string OutlineScenarioCode
        {
            get
            {
                var Result = string.Empty;
                var Outline = ScenarioCode;

                for (var Row = 0; Row < Scenario.Examples.Count; Row++)
                {
                    var CurrentCode = ReplaceNameIn(Outline, Row);

                    for (var Col = 0; Col < Scenario.Examples.Header.Count; Col++) 
                        CurrentCode = ReplaceExampleIn(CurrentCode, Row, Col);

                    Result += CurrentCode;
                }

                return Result;
            }
        }

        string ReplaceNameIn(string Outline, int Row)
        {
            return Outline.Replace(Scenario.Name+"()", Scenario.Name+(Row + 1)+"()");
        }

        string ReplaceExampleIn(string Outline, int Row, int Col)
        {
            return Outline.Replace
            (
                Scenario.Examples.Header[Col].Quote(), 
                ArgFormatter.ValueOf(Scenario.Examples[Row, Col])
            );
        }
    }
}