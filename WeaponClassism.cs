using System;
using System.Collections.Generic;
using UnityEngine;
using Necro;
using Partiality.Modloader;
using HBS.Text;
using HBS.Collections;
using MonoMod.ModInterop;
using ThisIsASwordsUncutEquipment;

class WeaponClassism : PartialityMod
{
    Dictionary<string, Action<TextFieldParser, EffectDef>> methodParsers;

    public override void Init()
    {
        typeof(Utils).ModInterop();
        On.Necro.DataManager.Awake += (orig, instance) =>
        {
            methodParsers = Utils.GetMethodParsers();
            methodParsers["WeaponClassism"] = new Action<TextFieldParser, EffectDef>(this.ParseGameEffect_WeaponClassism); ;
            orig(instance);
        };
        base.Init();
    }

    public override void OnLoad()
    {
        base.OnLoad();
    }

    private void ParseGameEffect_WeaponClassism(TextFieldParser parser, EffectDef def)
    {
        def.actorMethod = CreateMethod_WeaponClassism(parser["Params"], Utils.TryParseFloat(parser, "Duration", null));
    }

    // If the wielder does not have a group ID that is compatible with this effect, discriminate
    private static GameEffectActorMethod CreateMethod_WeaponClassism(string tag, float damageScaleModifier)
    {
        return delegate (EffectContext context, Actor targetActor)
        {
            Actor wielder = context.sourceActor;
            TagSet wielderTags = wielder.ActorDef.tags;
            WeaponBody sourceWeapon = context.sourceWeapon;
            damageScaleModifier *= 0.1f;

            // If this weapon is being used by an actor of the proper class...
            if (wielderTags.Contains(tag))
            {
                // Beneficiate...
                Debug.Log("You are the one! Applied damage scale modifier of " + damageScaleModifier.ToString());
                targetActor.ApplyHitDamage(context, 1f + damageScaleModifier);
            }
            // If this weapon is being used by an actor of the proper class...
            else
            {
                // Discriminate...
                Debug.Log("You are NOT the one. Applied damage scale modifier of " + damageScaleModifier.ToString());
                targetActor.ApplyHitDamage(context, 1f + (-damageScaleModifier));
            }
        };
    }
}
