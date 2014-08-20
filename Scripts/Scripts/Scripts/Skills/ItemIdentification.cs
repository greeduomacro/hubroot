using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using System.Text;

namespace Server.Items
{
    public class ItemIdentification
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.ItemID].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse( Mobile from )
        {
            from.SendMessage("Select the object you wish to examine.");
            from.Target = new InternalTarget();

            return TimeSpan.FromSeconds(1.0);
        }

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(8, false, TargetFlags.None)
            {
                AllowNonlocal = true;
            }

            protected override void OnTarget( Mobile from, object o )
            {
                if (o is Item)
                {
                    string runes = null;
                    StringBuilder translation = new StringBuilder();
                
                    Item item = o as Item;
                    if (from.InRange(item.GetWorldLocation(), 1))
                    {
                        if (from.CheckTargetSkill(SkillName.ItemID, o, 50, 100))
                        {
                            if (from.Skills.ItemID.Value > 35)
                            {
                                if (item.Weight >= 1 && item.Amount > 1)
                                {
                                    double pounds = (double)(item.Weight / 2);
                                    from.SendMessage("You'd estimate this objects invdividual weight at {0} pounds.", pounds);
                                }

                                if (item.Weight >= 1 && item.Amount == 1)
                                {
                                    double pounds = (double)(item.Weight / 2);
                                    from.SendMessage("You'd estimate this objects weight at {0} pounds.", pounds);
                                }

                                if (item.Weight < 1)
                                    from.SendMessage("You need a scale to accurately gauge this item's weight.");
                            }

                            if (o is BaseWeapon)
                            {
                                BaseWeapon weapon = o as BaseWeapon;

                                if (from.Skills.ItemID.Value > 65)
                                {
                                    if (weapon.Attributes.AttackChance != 0)
                                    {
                                        runes += "AZE";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Accuracy Bonus: +{0}\n", weapon.Attributes.AttackChance.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.DefendChance != 0)
                                    {
                                        runes += "BYZ";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Evasion Bonus: +{0}\n", weapon.Attributes.DefendChance.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.WeaponDamage != 0)
                                    {
                                        runes += "CXS";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Damage Bonus: +{0}\n", weapon.Attributes.WeaponDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 75)
                                {
                                    if (weapon.Attributes.WeaponSpeed != 0)
                                    {
                                        runes += "DWE";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Speed Bonus: +{0}\n", weapon.Attributes.WeaponSpeed.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.SpellDamage != 0)
                                    {
                                        runes += "EVE";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Spell Damage Bonus: +{0}\n", weapon.Attributes.SpellDamage.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitLowerAttack != 0)
                                    {
                                        runes += "ADF";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Break Attack: {0}\n", weapon.WeaponAttributes.HitLowerAttack.ToString());

                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitLowerDefend != 0)
                                    {
                                        runes += "FVS";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Break Defense: {0}\n", weapon.WeaponAttributes.HitLowerDefend.ToString());

                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 80)
                                {
                                    if (weapon.Attributes.BonusStam != 0)
                                    {
                                        runes += "FUW";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Stam: {0}\n", weapon.Attributes.BonusStam.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.BonusHits != 0)
                                    {
                                        runes += "GTA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Hits: {0}\n", weapon.Attributes.BonusHits.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.BonusMana != 0)
                                    {
                                        runes += "HHS";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Mana: {0}\n", weapon.Attributes.BonusMana.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.BonusDex != 0)
                                    {
                                        runes += "IRA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Dex: {0}\n", weapon.Attributes.BonusDex.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.BonusStr != 0)
                                    {
                                        runes += "JQA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Str: {0}\n", weapon.Attributes.BonusStr.ToString());
                                            
                                        }
                                    }

                                    if (weapon.Attributes.BonusInt != 0)
                                    {
                                        runes += "KPL";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Int: {0}\n", weapon.Attributes.BonusInt.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 85)
                                {
                                    if (weapon.Attributes.LowerManaCost != 0)
                                    {
                                        runes += "LOL";
                                        if (from.Skills.Inscribe.Value > 85)
                                        {
                                            translation.AppendFormat("Lower Mana Cost: {0\n}%", weapon.Attributes.LowerManaCost.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 90)
                                {
                                    if (weapon.WeaponAttributes.HitColdArea != 0)
                                    {
                                        runes += "MNT";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Frost Radius ({0}%)\n", weapon.WeaponAttributes.HitColdArea.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitDispel != 0)
                                    {
                                        runes += "NKA";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Hit Dispel ({0})\n", weapon.WeaponAttributes.HitDispel.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitEnergyArea != 0)
                                    {
                                        runes += "ONN";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Shock Radius ({0}%)\n", weapon.WeaponAttributes.HitEnergyArea.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitPoisonArea != 0)
                                    {
                                        runes += "PQA";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Corrosion Radius ({0}%)\n", weapon.WeaponAttributes.HitPoisonArea.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitFireArea != 0)
                                    {
                                        runes += "QRQ";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Burn Radius ({0}%)\n", weapon.WeaponAttributes.HitFireArea.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitPhysicalArea != 0)
                                    {
                                        runes += "RSS";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Physical Area Hit ({0})\n", weapon.WeaponAttributes.HitPhysicalArea.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitHarm != 0)
                                    {
                                        runes += "STZ";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Hit Harm ({0})\n", weapon.WeaponAttributes.HitHarm.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitFireball != 0)
                                    {
                                        runes += "TBS";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Hit Fireball ({0})\n", weapon.WeaponAttributes.HitFireball.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitMagicArrow != 0)
                                    {
                                        runes += "UDD";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Hit Magic Arrow ({0})\n", weapon.WeaponAttributes.HitMagicArrow.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitLightning != 0)
                                    {
                                        runes += "VSD";
                                        if (from.Skills.Inscribe.Value > 90)
                                        {
                                            translation.AppendFormat("Hit Lightning ({0})\n", weapon.WeaponAttributes.HitLightning.ToString());                                           
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 95)
                                {
                                    if (weapon.WeaponAttributes.HitLeechHits != 0)
                                    {
                                        runes += "WXS";
                                        if (from.Skills.Inscribe.Value > 95)
                                        {
                                            translation.AppendFormat("Hit Life Leech ({0})\n", weapon.WeaponAttributes.HitLeechHits.ToString());                                           
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitLeechStam != 0)
                                    {
                                        runes += "BXY";
                                        if (from.Skills.Inscribe.Value > 95)
                                        {
                                            translation.AppendFormat("Hit Stamina Leech ({0})\n", weapon.WeaponAttributes.HitLeechStam.ToString());
                                            
                                        }
                                    }

                                    if (weapon.WeaponAttributes.HitLeechMana != 0)
                                    {
                                        runes += "YAZ";
                                        if (from.Skills.Inscribe.Value > 95)
                                        {
                                            translation.AppendFormat("Hit Mana Leech ({0})\n", weapon.WeaponAttributes.HitLeechMana.ToString());
                                            
                                        }
                                    }
                                }

                                if ((!weapon.WeaponAttributes.IsEmpty) || (!weapon.Attributes.IsEmpty && runes != null))
                                {
                                    from.SendMessage("Runic Symbols:");
                                    from.Send(new AsciiMessage(Serial.MinusOne, -1, MessageType.Regular, 1152, 8, "Runes", runes));

                                    if (from.Skills.Inscribe.Value > 65)
                                    {
                                        from.SendMessage("\nYour skill in inscription allows you to decode some of the runes:");
                                        from.SendMessage(translation.ToString());
                                    }
                                }
                            }

                            if (o is BaseArmor)
                            {
                                BaseArmor armor = o as BaseArmor;

                                if (from.Skills.ItemID.Value > 65)
                                {
                                    if (armor.Attributes.AttackChance != 0)
                                    {
                                        runes += "AZS";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Accuracy Bonus: +{0}\n", armor.Attributes.AttackChance.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.DefendChance != 0)
                                    {
                                        runes += "BYZ";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Evasion Bonus: +{0}\n", armor.Attributes.DefendChance.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.WeaponDamage != 0)
                                    {
                                        runes += "CXS";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Weapon Damage: +{0}\n", armor.Attributes.WeaponDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 75)
                                {
                                    if (armor.Attributes.WeaponSpeed != 0)
                                    {
                                        runes += "DMW";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Weapon Speed: +{0}\n", armor.Attributes.WeaponSpeed.ToString());                                           
                                        }
                                    }

                                    if (armor.Attributes.SpellDamage != 0)
                                    {
                                        runes += "EUL";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Spell Damage: +{0}\n", armor.Attributes.SpellDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 80)
                                {
                                    if (armor.Attributes.BonusStam != 0)
                                    {
                                        runes += "FSV";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Stam: {0}\n", armor.Attributes.BonusStam.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.BonusHits != 0)
                                    {
                                        runes += "GKB";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Bonus Hits: {0}\n", armor.Attributes.BonusHits.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.BonusMana != 0)
                                    {
                                        runes += "HJK";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Mana: {0}\n", armor.Attributes.BonusMana.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.BonusDex != 0)
                                    {
                                        runes += "IQS";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Dex: {0}\n", armor.Attributes.BonusDex.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.BonusStr != 0)
                                    {
                                        runes += "JKH";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Str: {0}\n", armor.Attributes.BonusStr.ToString());
                                            
                                        }
                                    }

                                    if (armor.Attributes.BonusInt != 0)
                                    {
                                        runes += "KLD";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Int: {0}\n", armor.Attributes.BonusInt.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 85)
                                {
                                    if (armor.Attributes.LowerManaCost != 0)
                                    {
                                        runes += "LMN";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Lower Mana Cost: {0}%\n", armor.Attributes.LowerManaCost.ToString());                                           
                                        }
                                    }
                                }

                                if (!(armor.Attributes.IsEmpty) && runes != null)
                                {
                                    from.SendMessage("Runic Symbols:");
                                    from.Send(new AsciiMessage(Serial.MinusOne, -1, MessageType.Regular, 1152, 8, "Runes", runes));

                                    if (from.Skills.Inscribe.Value > 65)
                                    {
                                        from.SendMessage("Your skill in inscription allows you to decode some of the runes:");
                                        from.SendMessage(translation.ToString());
                                    }
                                }
                            }

                            if (o is BaseJewel)
                            {
                                BaseJewel jewel = o as BaseJewel;

                                if (from.Skills.ItemID.Value > 65)
                                {
                                    if (jewel.Attributes.AttackChance != 0)
                                    {
                                        runes += "AZN";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Accuracy Bonus: +{0}\n", jewel.Attributes.AttackChance.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.DefendChance != 0)
                                    {
                                        runes += "BXS";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Evasion Bonus: +{0}\n", jewel.Attributes.DefendChance.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.WeaponDamage != 0)
                                    {
                                        runes += "CYE";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Weapon Damage: +{0}\n", jewel.Attributes.WeaponDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 75)
                                {
                                    if (jewel.Attributes.WeaponSpeed != 0)
                                    {
                                        runes += "DUL";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Weapon Speed: +{0}\n", jewel.Attributes.WeaponSpeed.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.SpellDamage != 0)
                                    {
                                        runes += "EVA";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Spell Damage: +{0}\n", jewel.Attributes.SpellDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 80)
                                {
                                    if (jewel.Attributes.BonusStam != 0)
                                    {
                                        runes += "FSF";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Stam: {0}\n", jewel.Attributes.BonusStam.ToString());
                                            
                                        }
                                    }
                                    if (jewel.Attributes.BonusHits != 0)
                                    {
                                        runes += "GBA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Hits: {0}\n", jewel.Attributes.BonusHits.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.BonusMana != 0)
                                    {
                                        runes += "HKG";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Mana: {0}\n", jewel.Attributes.BonusMana.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.BonusDex != 0)
                                    {
                                        runes += "IDA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Dex: {0}\n", jewel.Attributes.BonusDex.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.BonusStr != 0)
                                    {
                                        runes += "KSS";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Str: {0}\n", jewel.Attributes.BonusStr.ToString());
                                            
                                        }
                                    }

                                    if (jewel.Attributes.BonusInt != 0)
                                    {
                                        runes += "LQL";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Int: {0}\n", jewel.Attributes.BonusInt.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 85)
                                {
                                    if (jewel.Attributes.LowerManaCost != 0)
                                    {
                                        runes += "MTN";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Lower Mana Cost: {0}%\n", jewel.Attributes.LowerManaCost.ToString());
                                            
                                        }
                                    }
                                }


                                if ( !jewel.Attributes.IsEmpty )
                                {
                                    from.SendMessage("Runic Symbols:");
                                    from.Send(new AsciiMessage(Serial.MinusOne, -1, MessageType.Regular, 1152, 8, "Runes", runes));

                                    if (from.Skills.Inscribe.Value > 65)
                                    {
                                        from.SendMessage("\nYour skill in inscription allows you to decode some of the runes:");
                                        from.SendMessage(translation.ToString());
                                    }
                                }
                            }

                            if (o is BaseClothing)
                            {
                                BaseClothing clothes = o as BaseClothing;

                                if (from.Skills.ItemID.Value > 65)
                                {
                                    if (clothes.Attributes.AttackChance != 0)
                                    {
                                        runes += "AZR";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Accuracy Bonus: +{0}\n", clothes.Attributes.AttackChance.ToString());
                                            
                                        }
                                    }

                                    if (clothes.Attributes.DefendChance != 0)
                                    {
                                         runes += "BYE";
                                         if (from.Skills.Inscribe.Value > 65)
                                         {
                                             translation.AppendFormat("Evasion Bonus: +{0}\n", clothes.Attributes.DefendChance.ToString());
                                             
                                         }
                                    }

                                    if (clothes.Attributes.WeaponDamage != 0)
                                    {
                                        runes += "CXS";
                                        if (from.Skills.Inscribe.Value > 65)
                                        {
                                            translation.AppendFormat("Weapon Damage: +{0}\n", clothes.Attributes.WeaponDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 75)
                                {
                                    if (clothes.Attributes.WeaponSpeed != 0)
                                    {
                                        runes += "DMV";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Weapon Speed: +{0}\n", clothes.Attributes.WeaponSpeed.ToString());
                                            
                                        }
                                    }

                                    if (clothes.Attributes.SpellDamage != 0)
                                    {
                                        runes += "EUR";
                                        if (from.Skills.Inscribe.Value > 75)
                                        {
                                            translation.AppendFormat("Spell Damage: +{0}\n", clothes.Attributes.SpellDamage.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 80)
                                {
                                    if (clothes.Attributes.BonusStam != 0)
                                    {
                                        runes += "FTL";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Stam: {0}\n", clothes.Attributes.BonusStam.ToString());
                                            
                                        }
                                    }
                                    if (clothes.Attributes.BonusHits != 0)
                                    {
                                        runes += "GHB";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Hits: {0}\n", clothes.Attributes.BonusHits.ToString());
                                            
                                        }
                                    }
                                    if (clothes.Attributes.BonusMana != 0)
                                    {
                                        runes += "HTA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Mana: {0}\n", clothes.Attributes.BonusMana.ToString());
                                            
                                        }
                                    }
                                    if (clothes.Attributes.BonusDex != 0)
                                    {
                                        runes += "IQL";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Dex: {0}\n", clothes.Attributes.BonusDex.ToString());
                                            
                                        }
                                    }
                                    if (clothes.Attributes.BonusStr != 0)
                                    {
                                        runes += "JHA";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Str: {0}\n", clothes.Attributes.BonusStr.ToString());
                                            
                                        }
                                    }
                                    if (clothes.Attributes.BonusInt != 0)
                                    {
                                        runes += "KNO";
                                        if (from.Skills.Inscribe.Value > 80)
                                        {
                                            translation.AppendFormat("Bonus Int: {0}\n", clothes.Attributes.BonusInt.ToString());
                                            
                                        }
                                    }
                                }

                                if (from.Skills.ItemID.Value > 85)
                                {
                                    if (clothes.Attributes.LowerManaCost != 0)
                                    {
                                        runes += "LPL";
                                        if (from.Skills.Inscribe.Value > 85)
                                        {
                                            translation.AppendFormat("Lower Mana Cost: {0}%\n", clothes.Attributes.LowerManaCost.ToString());
                                            
                                        }
                                    }
                                }

                                if ( !clothes.Attributes.IsEmpty && runes != null )
                                {
                                    from.SendMessage("Runic Symbols:");
                                    from.Send(new AsciiMessage(Serial.MinusOne, -1, MessageType.Regular, 1152, 8, "Runes", runes));

                                    if (from.Skills.Inscribe.Value > 65)
                                    {
                                        from.SendMessage("\nYour skill in inscription allows you to decode some of the runes:");
                                        from.SendMessage(translation.ToString());
                                    }
                                }
                            }
                        }

                        else
                        {
                            from.SendLocalizedMessage(500353); // You are not certain...
                        }
                    }
                    else
                    {
                        from.SendMessage("You are too far away.");
                    }
                }

                else if( o is Mobile )
                {
                    ((Mobile)o).OnSingleClick(from);
                }

                else
                {
                    from.SendLocalizedMessage(500353); // You are not certain...
                }

                EventSink.InvokeSkillUsed(new SkillUsedEventArgs(from, from.Skills[SkillName.ItemID]));
            }
        }
    }
}
