 using System;
using System.Collections.Generic;
using Necro;
using Partiality.Modloader;
using HBS.Text;
using HBS.Collections;
using MonoMod.ModInterop;
using ThisIsASwordsUncutEquipment;
using HBS.Logging;

class MaybeSpawnMinions : PartialityMod
{
    Dictionary<string, Action<TextFieldParser, EffectDef>> methodParsers;
    //private static readonly Random random = new Random();

    public override void Init()
    {
        typeof(Utils).ModInterop();
        On.Necro.DataManager.Awake += (orig, instance) =>
        {
            methodParsers = Utils.GetMethodParsers();
            methodParsers["MaybeSpawnMinions"] = new Action<TextFieldParser, EffectDef>(this.ParseGameEffect_MaybeSpawnMinions); ;
            orig(instance);
        };
        base.Init();
    }

    public override void OnLoad()
    {
        base.OnLoad();
    }

    private void ParseGameEffect_MaybeSpawnMinions(TextFieldParser parser, EffectDef def)
    {
        // 20% chance to spawn minions?
        if (UnityEngine.Random.Range(0, 100) < 5)
        {
            TagWeights tagWeights = Utils.ParseTagWeights(parser, "Params", true);
            if (tagWeights == null)
            {
                def.worldMethod = GameEffectManager.CreateMethod_LogMessage(LogLevel.Error, "Broken SpawnMinions GameEffect for: " + def.id);
            }
            else
            {
                string tagAndWeight = tagWeights.GetTagAndWeight(0, out int level);
                def.worldMethod = GameEffectManager.CreateMethod_SpawnMinions(tagAndWeight, level, Utils.TryParseFloat(parser, "Radius", null), Utils.TryParseFloat(parser, "Duration", null));
            }
        }
    }
}
