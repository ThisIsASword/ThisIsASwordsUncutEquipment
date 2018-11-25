using UnityEngine;
using System;
using Necro;
using Necro.Data.Stats;
using Partiality.Modloader;
using System.Collections.Generic;
using HBS.Text;
using MonoMod.ModInterop;
using ThisIsASwordsUncutEquipment;

class WeaponSizeChange : PartialityMod
{
    Dictionary<string, Action<TextFieldParser, EffectDef>> methodParsers;

    public override void Init()
    {
        typeof(Utils).ModInterop();
        On.Necro.DataManager.Awake += (orig, instance) =>
        {
            methodParsers = Utils.GetMethodParsers();
            methodParsers["WeaponSizeChange"] = new Action<TextFieldParser, EffectDef>(this.ParseGameEffect_WeaponSizeChange); ;
            orig(instance);
        };
        //this.name = "ThisIsASword's Uncut Equipment";
        //this.version = "1.0";
        base.Init();
    }

    public override void OnLoad()
    {
        base.OnLoad();
    }

    private void ParseGameEffect_WeaponSizeChange(TextFieldParser parser, EffectDef def)
    {
        def.actorMethod = CreateMethod_WeaponSizeChange(Utils.TryParseFloat(parser, "Radius", null), Utils.TryParseFloat(parser, "Duration", null), Utils.ParseStatMods(parser, "Params", false));
    }

    private static GameEffectActorMethod CreateMethod_WeaponSizeChange(float maxSize, float increment, StatModPair [] statMods)
    {
        return delegate (EffectContext context, Actor actor)
        {
            Debug.Log("This is the script");
            // if partial hit, cut increment in half
            if (context.hitResult == EffectContext.HitResult.PartialHit)
            {
                increment /= 2;
            }

            // if item scale on local x is smaller than maxSize...
            if (context.sourceWeapon.gameObject.transform.localScale.x < maxSize)
            {
                Debug.Log("Growing Weapon.");
                // if item scale on local x would be greater than maxSize...
                if (context.sourceWeapon.gameObject.transform.localScale.x + increment > maxSize)
                {
                    // set item scale on local x to maxSize
                    context.sourceWeapon.gameObject.transform.localScale = new Vector3(maxSize, 1f, 1f);
                }
                else
                {
                    // increment item scale on local x by increment
                    context.sourceWeapon.gameObject.transform.localScale += new Vector3(increment, 0f, 0f);
                }
            }
            if (context.sourceWeapon.gameObject.transform.localScale.x == maxSize && statMods.Length > 0)
            {
                Debug.Log("This weapon should now have extra powers.");
                context.sourceActor.AddMods(statMods, -1, false);
            }
        };
    }
}





