using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod.Items
{
    public class LightStrike : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Strike");
            Tooltip.SetDefault("Summons a beam of light from the sky");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.magic = true;
            item.mana = 20;
            item.width = 28;
            item.height = 30;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = Item.sellPrice(gold: 10);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<LightBeam>();
            item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
