﻿using System;
using System.Collections.Generic;
using Raconteur.Helpers;

namespace Raconteur
{
    public class Feature 
    {
        public List<Scenario> Scenarios { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string FileName { get; set; }

        public Type DefaultStepDefinitions { get; set; }
        public bool HasStepDefinitions
        {
            get { return StepDefinitions.HasItems(); }
        }
        public List<string> DeclaredStepDefinitions { get; set; }
        public List<Type> StepDefinitions { get; set; }

        public Feature()
        {
            Scenarios = new List<Scenario>();
            StepDefinitions = new List<Type>();
            DeclaredStepDefinitions = new List<string>();
        }
    }

    public class InvalidFeature : Feature
    {
        public string Reason { get; set; }
    }
}