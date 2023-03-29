using Terraria;
using Terraria.ModLoader;

namespace MyMod.Buffs
{
    public class SoulBurn : ModBuff
    {
        public override void SetDefaults()
        {
            // Display name of the buff
            DisplayName.SetDefault("Soul Burn");
            // Description of the buff
            Description.SetDefault("Your soul is burning, reducing your defense and life regeneration");
            // Whether this is a debuff or not
            Main.debuff[Type] = true;
            // Whether this buff can be removed by the nurse
            Main.buffNoSave[Type] = true;
            // Whether this buff can be removed by potions
            canBeCleared = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // Apply the debuff effects to the NPC
            npc.defense -= 10; // Reduce defense by 10
            npc.lifeRegen -= 20; // Reduce life regeneration by 20 per second
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Apply the debuff effects to the player
            player.statDefense -= 10; // Reduce defense by 10
            player.lifeRegen -= 20; // Reduce life regeneration by 20 per second
        }
    }
}