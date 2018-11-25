using UnityEngine;
using System;
using Necro;
using Partiality.Modloader;
using HBS;
using System.Collections.Generic;
using HBS.Text;
using HBS.Pooling;
using MonoMod.ModInterop;
using ThisIsASwordsUncutEquipment;

/*
 * post-build event command line
 * delI:\SteamLibrary\steamapps\common\Necropolis\Mods\ThisIsASwordsUncutEquipment\ThisIsASwordsUncutEquipment.dll & copy ThisIsASwordsUncutEquipment.dll I:\SteamLibrary\steamapps\common\Necropolis\Mods\ThisIsASwordsUncutEquipment\ThisIsASwordsUncutEquipment.dll
 */

class ShootGems : PartialityMod
{
    Dictionary<string, Action<TextFieldParser, EffectDef>> methodParsers;

    public override void Init()
    {
        typeof(Utils).ModInterop();
        On.Necro.DataManager.Awake += (orig, instance) =>
        {
            methodParsers = Utils.GetMethodParsers();
            methodParsers["ShootGems"] = new Action<TextFieldParser, EffectDef>(this.ParseGameEffect_ShootGems);;
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

    private void ParseGameEffect_ShootGems(TextFieldParser parser, EffectDef def)
    {
        def.worldMethod = CreateMethod_ShootGems(parser["Params"], (int)Utils.TryParseFloat(parser, "Radius", null), (int)Utils.TryParseFloat(parser, "Duration", null));
    }

    private static GameEffectWorldMethod CreateMethod_ShootGems(string itemDefId, int gemCost, int duration)
    {
        return delegate (EffectContext context)
        {
            /* #########################################
             * Check if there is enough money to shoot 
             #########################################*/
            if (!LazySingletonBehavior<NetworkManager>.Instance.IsSimulationServer)
            {
                return;
            }
            Actor wielder = context.sourceActor;
            if (wielder.IsRemoteSimulation)
            {
                return;
            }

            WeaponBody weapon = context.sourceWeapon;
            ActionDef action = context.sourceAction;
            int num = 0;
            if (weapon != null)
            {
                string ammoId = weapon.ItemDef.ammoId;
                num = weapon.GetAmmoUse(action);

                Inventory inventory = Inventory.Get(wielder.gameObject, true);
                int playerGems = inventory.GetCurrency("Gem1");
                // (For Ammo system) Checks if the weapon uses ammo and the rounds used are 0 or greater
                if (num >= 0 && !string.IsNullOrEmpty(ammoId))
                {
                    //Inventory inventory = Inventory.Get(wielder.gameObject);
                    if (inventory == null || !inventory.HasAmmo(ammoId, num))
                    {
                        //Debug.Log("Not playing NO AMMO animation.");
                        wielder.thisAnimator.SetTrigger(wielder.Body.thisAnimator.GetControlId("AttackEmpty"));
                        return;
                    }
                    if (num > 0)
                    {
                        inventory.RemoveAmmo(ammoId, num);
                    }
                    inventory.UpdateRangedSlot();
                }
                // (For consuming gems without an ammo system) if cost of gem projectile <= to player's bank account...
                if (gemCost <= playerGems)
                {
                    // remove cost of gem projectile
                    //Debug.Log("The Ambassador bribes its enemy to death.");
                    inventory.RemoveCurrency("Gem1", gemCost);

                    // Maybe give the amount returned by RemoveCurrency() to context.targetActor?
                }
                else
                {
                    // Uncomment the line below to trigger a NO AMMO animation if there isn't enough money.
                    // wielder.thisAnimator.SetTrigger(wielder.Body.thisAnimator.GetControlId("AttackEmpty"));
                    //Debug.Log("Ain't got no gems.");
                    return;
                }
            }
            /* #########################################
             * Handle the business of shooting
             #########################################*/
            ItemDef itemDef = LazySingletonBehavior<DataManager>.Instance.Items.Get(itemDefId);
            num = ((num <= 0) ? 1 : num);
            int count = 0;
            Vector3 position = weapon.transform.position;
            Quaternion rotation = weapon.transform.rotation;
            for (int i = 0; i < num; i++)
            {
                PrefabManager.InstantiatePrefab(itemDef.prefabName, false, null, position, rotation, delegate (GameObject go)
                {
                    Projectile component = go.GetComponent<Projectile>();
                    if (component == null)
                    {
                        //GameEffectManager.log.Error("Item \"" + itemDefId + "\" doesn't have a valid projectile prefab: " + go.name, go);
                        Debug.LogError("Item \"" + itemDefId + "\" doesn't have a valid projectile prefab: " + go.name, go);
                        go.Despawn();
                        return;
                    }
                    Vector3 zero = Vector3.zero;
                    float num2 = 1f;
                    if (count++ > 0)
                    {
                        zero = new Vector3((UnityEngine.Random.value - 0.5f) * num2, (UnityEngine.Random.value - 0.5f) * num2, (UnityEngine.Random.value - 0.5f) * num2);
                    }
                    go.SetActive(true);
                    component.Initialize(itemDef, wielder, weapon, action, wielder.TargetObject, duration, zero, Projectile.ProjectileType.Shoot, 0u);
                });
            }
        };
    }
}





