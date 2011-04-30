﻿using System;
using System.Linq;
using System.Collections.Generic;
using Raconteur.Compilers;
using Raconteur.Helpers;

namespace Raconteur.Generators.Steps
{
    public class StepGeneratorForImplementedStep : StepGenerator
    {
        public StepGeneratorForImplementedStep(Step Step) : base(Step) {}

        public override IEnumerable<string> FormatArgsOnly
        {
            get
            {
                var Args = Step.ArgDefinitions.Take(Step.Args.Count).ToArray();

                return Step.Args.Select((Arg, i) => 
                    ArgFormatter.Format(Arg, Args[i].ParameterType));
            }
        }

        public override IEnumerable<string> FormatArgsForTable
        {
            get
            {
                return Step.Type == StepType.Table ? 
                    FormatArgsForSimpleTable :
                    FormatArgsForTableWithHeader ;
            }
        }

        IEnumerable<string> FormatArgsForSimpleTable
        {
            get
            {
                var ArgsType = Step.TableItemType();

                return Row.Select(Arg => ArgFormatter.Format(Arg, ArgsType));
            }
        }

        IEnumerable<string> FormatArgsForTableWithHeader
        {
            get
            {
                return Step.Type == StepType.ObjectTable? 
                    FormatArgsForObjectTable : 
                    FormatArgsForSimpleTableWithHeader;
            }
        }

        private const string ObjectArgTemplate = 
@"        
                new {0}
                {{{1}
                }}";

        private const string FieldInitializerTemplate = 
@"        
                    {0} = {1}";


        IEnumerable<string> FormatArgsForSimpleTableWithHeader
        {
            get
            {
                var Args = Step.ArgDefinitions.Skip(Step.Args.Count).ToArray();

                return Row.Select((Value, i) => 
                    ArgFormatter.Format(Value, Args[i].ParameterType));
            }
        }

        IEnumerable<string> FormatArgsForObjectTable
        {
            get
            {
                var FieldsInitialized = Row.Select((Value, i) => 
                    FormatArgForObjectInitializer
                    (
                        Value, Step.ObjectArg, Step.Table.Header[i]
                    ));

                return new List<string>
                {
                    string.Format
                    (
                        ObjectArgTemplate, 
                        Step.ObjectArg.FullName, 
                        string.Join(",", FieldsInitialized)
                    )
                };
            }
        }

        string FormatArgForObjectInitializer(string Value, Type ObjectArg, string FieldName)
        {
            return string.Format
            (
                FieldInitializerTemplate,
                FieldName,
                ArgFormatter.Format(Value, ObjectArg.FieldType(FieldName))
            );
        }
    }
}