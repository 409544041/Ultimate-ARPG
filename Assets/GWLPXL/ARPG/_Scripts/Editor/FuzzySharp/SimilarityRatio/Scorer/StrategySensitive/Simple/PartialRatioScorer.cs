﻿using System;
using GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Strategy;

namespace GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Scorer.StrategySensitive.Simple
{
    public class PartialRatioScorer : SimpleRatioScorerBase
    {
        protected override Func<string, string, int> Scorer => PartialRatioStrategy.Calculate;
    }
}
