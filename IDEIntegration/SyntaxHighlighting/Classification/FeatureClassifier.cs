﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Raconteur.IDEIntegration.SyntaxHighlighting.Token;

namespace Raconteur.IDEIntegration.SyntaxHighlighting
{
    internal class FeatureClassifier : ITagger<ClassificationTag>
    { 
        private readonly ITagAggregator<FeatureTokenTag> aggregator;
        private readonly Dictionary<FeatureTokenTypes, IClassificationType> featureTypes;

        public FeatureClassifier(ITextBuffer buffer, ITagAggregator<FeatureTokenTag> tagAggregator, IClassificationTypeRegistryService registry)
        {
            aggregator = tagAggregator;
            featureTypes = new Dictionary<FeatureTokenTypes, IClassificationType>
            {
                 {FeatureTokenTypes.FeatureDefinition, registry.GetClassificationType("Keyword")},
                 {FeatureTokenTypes.ScenarioDefinition, registry.GetClassificationType("Keyword")},
                 {FeatureTokenTypes.ExampleDefinition, registry.GetClassificationType("Keyword")},
                 {FeatureTokenTypes.Arg, registry.GetClassificationType("String")},
                 {FeatureTokenTypes.TableValue, registry.GetClassificationType("Number")},
                 {FeatureTokenTypes.Comment, registry.GetClassificationType("Comment")},
            };
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return (from tag in aggregator.GetTags(spans) 
                    let tagSpans = tag.Span.GetSpans(spans[0].Snapshot) 
                    select new TagSpan<ClassificationTag>(tagSpans[0], 
                                                          new ClassificationTag(featureTypes[tag.Tag.Type])));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }
}