using System;
using System.Collections.Generic;
using Necro;
using Necro.Data.Stats;
using HBS.Text;
using HBS.Collections;

namespace ThisIsASwordsUncutEquipment
{
    public static class Utils
    {
        public static Func<Dictionary<string, Action<TextFieldParser, EffectDef>>> GetMethodParsers;

        public static Func<TextFieldParser, string, bool, TagWeights> ParseTagWeights;

        public static Func<TextFieldParser, string, string, float> TryParseFloat;

        public static Func<TextFieldParser, string, bool, StatModPair []> ParseStatMods;
    }
}