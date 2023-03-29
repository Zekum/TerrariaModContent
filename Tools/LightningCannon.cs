using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdedTerraria
{
    public class LightningCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots lightning from the gun");
        }

        public override void SetDefaults()
        {
            item.damage = 60; //The damage of the weapon
            item.ranged = true; //This makes the weapon a ranged weapon
            item.width = 40; //The width of the weapon sprite
            item.height = 20; //The height of the weapon sprite
            item.useTime = 45; //The time it takes to use the weapon
            item.useAnimation = 45; //The animation time of the weapon
            item.useStyle = ItemUseStyleID.HoldingOut; //The style of using the weapon
            item.noMelee = true; //This makes sure the weapon doesn't do melee damage
            item.knockBack = 8; //The knockback of the weapon
            item.value = Item.sellPrice(0, 20, 0, 0); //The value of the weapon in coins
            item.rare = ItemRarityID.Lime; //The rarity of the weapon
            item.UseSound = SoundID.Item91; //The sound of using the weapon
            item.autoReuse = false; //This makes the weapon not auto-reuse
            item.shoot = ModContent.ProjectileType<LightningProjectile>(); //This sets the type of projectile the weapon shoots
            item.shootSpeed = 20f; //This sets the speed of the projectile the weapon shoots
            item.useAmmo = AmmoID.Bullet; //This sets the type of ammo the weapon uses
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shotgun); //This adds an ingredient to the recipe
            recipe.AddIngredient(ItemID.SoulofMight, 20); //This adds another ingredient to the recipe
            recipe.AddIngredient(ItemID.Wire, 50); //This adds another ingredient to the recipe
            recipe.AddTile(TileID.MythrilAnvil); //This sets the tile you need to craft the item
            recipe.SetResult(this); //This sets the result of the recipe
            recipe.AddRecipe(); //This adds the recipe to the game
        }
    }

    public class LightningProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
        }

        public override void SetDefaults()
        {
            projectile.width = 10; //The width of the projectile sprite
            projectile.height = 10; //The height of the projectile sprite
            projectile.aiStyle = -1; //This sets the ai style of the projectile (-1 means custom AI)
            projectile.friendly = true; //This makes the projectile friendly to players and town NPCs
            projectile.hostile = false; //This makes the projectile not hostile to players and town NPCs
            projectile.ranged = true; //This makes the projectile deal ranged damage
            projectile.penetrate = -1; //This makes the projectile penetrate infinitely
            projectile.timeLeft = 600; //This sets how long the projectile lasts before disappearing
            projectile.light = 0.5f; //This sets how much light the projectile emits
            projectile.ignoreWater = true; //This makes the projectile ignore water physics
            projectile.tileCollide = false; //This makes the projectile not collide with tiles
        }

        public override void AI()
        {
            //This method is called every tick for projectiles with custom AI

            if (projectile.owner == Main.myPlayer) //This checks if this is a local player's projectile (to prevent multiplayer desync)
            {
                Vector2 targetPos = Main.MouseWorld; //This gets the position of the mouse cursor in world coordinates

                float speed = 20f; //This is the speed of the projectile

                Vector2 move = targetPos - projectile.Center; //This gets a vector pointing from the projectile to the target position

                float distanceToTarget = move.Length(); //This gets the distance from the projectile to the target position

                if (distanceToTarget > speed) //This checks if the distance is greater than speed (to prevent overshooting)
                {
                    move *= speed / distanceToTarget; //This scales down the move vector to match
                    projectile.velocity = move; //This sets the velocity of the projectile to the move vector
                }
                else
                {
                    projectile.Kill(); //This kills the projectile if it reaches the target position
                }
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2; //This makes the projectile sprite face the direction of travel

            if (Main.rand.Next(3) == 0) //This makes a dust effect every 3 ticks on average
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Electric); //This creates a new dust at the projectile's position
                dust.noGravity = true; //This makes the dust not affected by gravity
                dust.scale = 1.2f; //This sets the size of the dust
                dust.velocity *= 0.1f; //This sets the speed of the dust
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //This method is called when the projectile hits an NPC

            target.AddBuff(BuffID.Electrified, 300); //This adds the electrified debuff to the target for 5 seconds
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            //This method is called when the projectile hits a player in PvP

            target.AddBuff(BuffID.Electrified, 300); //This adds the electrified debuff to the target for 5 seconds
        }
    }
}
