using Terraria;
using Terraria.ModLoader;

namespace ModdedTerraria
{
    public class Nightmare : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Nightmare");
            Description.SetDefault("You see hallucinations and take damage over time");
            Main.debuff[Type] = true; //This makes the buff a debuff
            Main.buffNoTimeDisplay[Type] = false; //This makes the buff have a timer
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //This method is called every tick for NPCs affected by the buff
            npc.GetGlobalNPC<ModdedTerrariaGlobalNPC>().nightmare = true; //This sets a custom flag for the NPC
            int damage = 5; //This is the base damage of the buff
            damage = (int)(damage * (1 + 0.01f * npc.lifeMax / npc.life)); //This increases the damage based on the NPC's health
            if (Main.rand.Next(4) == 0) //This makes the damage happen every 4 ticks on average
            {
                npc.StrikeNPC(damage, 0f, 0, false, false, false); //This deals damage to the NPC
                if (Main.netMode != NetmodeID.Server) //This checks if the game is not running on a server
                {
                    CombatText.NewText(npc.Hitbox, CombatText.DamagedHostile, damage); //This displays the damage text
                }
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //This method is called every tick for players affected by the buff
            player.GetModPlayer<ModdedTerrariaPlayer>().nightmare = true; //This sets a custom flag for the player
            int damage = 5; //This is the base damage of the buff
            damage = (int)(damage * (1 + 0.01f * player.statLifeMax2 / player.statLife)); //This increases the damage based on the player's health
            if (Main.rand.Next(4) == 0) //This makes the damage happen every 4 ticks on average
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was consumed by nightmares"), damage, 0, false, false, false, -1); //This deals damage to the player
                if (Main.netMode != NetmodeID.Server) //This checks if the game is not running on a server
                {
                    CombatText.NewText(player.Hitbox, CombatText.DamagedFriendly, damage); //This displays the damage text
                }
            }
        }
    }
}
