using UnityEngine;
using System;
using Necro;
using Partiality.Modloader;
using System.Collections.Generic;
using HBS.Text;
using MonoMod.ModInterop;
using ThisIsASwordsUncutEquipment;

/*
 * post-build event command line
 * delI:\SteamLibrary\steamapps\common\Necropolis\Mods\ThisIsASwordsUncutEquipment\ThisIsASwordsUncutEquipment.dll & copy ThisIsASwordsUncutEquipment.dll I:\SteamLibrary\steamapps\common\Necropolis\Mods\ThisIsASwordsUncutEquipment\ThisIsASwordsUncutEquipment.dll
 */

class TriggerDash : PartialityMod
{
    Dictionary<string, Action<TextFieldParser, EffectDef>> methodParsers;

    public override void Init()
    {
        typeof(Utils).ModInterop();
        On.Necro.DataManager.Awake += (orig, instance) =>
        {
            methodParsers = Utils.GetMethodParsers();
            methodParsers["TriggerDash"] = new Action<TextFieldParser, EffectDef>(this.ParseGameEffect_TriggerDash); ;
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

    private void ParseGameEffect_TriggerDash(TextFieldParser parser, EffectDef def)
    {
        def.actorMethod = CreateMethod_TriggerDash(Utils.TryParseFloat(parser, "Radius", null));
    }

    private static GameEffectActorMethod CreateMethod_TriggerDash(float distance)
    {
        return delegate (EffectContext context, Actor actor)
        {
            if (actor.IsJumpAttacking || context.sourceAction.holdAndRelease )
            {
                return;
            }
            Vector3 targetPosition = new Vector3();
            Transform targetT = actor.transform;
            Debug.Log("actor.Position: " + actor.Position.ToString() + "\n actor.Rotation: " + actor.Rotation.ToString() + "\n actor.transform.rotation: " + actor.transform.rotation.ToString());

            if (actor.IsTargeting)
            {
                targetPosition = actor.TargetActor.Position;
                Debug.Log("Target Position: " + targetPosition.ToString());
            }
            else
            {
                // actor.position               world position Vector3
                // actor.transform.position     local position Vector3
                // actor.transform.TransformDirection(Vector3.forward) // reliably returns which way the actor is facing.
                //Vector3 offset = new Vector3(0f, 0f, distance);




                //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view) 
                // m_Rigidbody.velocity = transform.forward * m_Speed;


                //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view) m_Rigidbody.velocity = transform.forward * m_Speed; }
                targetT.position = actor.Forward * distance;
                targetPosition = targetT.position;
                //Debug.Log("Target Position: " + targetPosition.ToString() + "\n Actor Position: " + actor.Position.ToString() + "\n Transform Direction: " + actor.transform.TransformDirection(Vector3.forward).ToString());
            }
            Vector3 velocity = targetPosition - actor.Position;
			velocity.y = 0f;
			float magnitude = velocity.magnitude;
			velocity = velocity.normalized * Mathf.Min(distance, magnitude) + Vector3.up * 2.5f;
			actor.AddVelocity(velocity);
        };
    }
}





