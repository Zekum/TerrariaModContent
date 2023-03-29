using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod.Projectiles
{
    public class FallingStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Display name of the projectile
            DisplayName.SetDefault("Falling Star");
        }

        public override void SetDefaults()
        {
            // Basic projectile properties
            projectile.width = 16; // The width of projectile hitbox
            projectile.height = 16; // The height of projectile hitbox
            projectile.aiStyle = 1; // The ai style of the projectile, this is normally 0 for custom AI, which we will use in this case
            projectile.friendly = true; // Can the projectile deal damage to enemies?
            projectile.hostile = false; // Can the projectile deal damage to the player?
            projectile.magic = true; // Is the projectile shoot by a magic weapon?
            projectile.penetrate = 1; // How many monsters the projectile can penetrate. -1 is infinity
            projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            projectile.alpha = 255; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 causes transparency to be reduced by 5 every update cycle)
            projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            projectile.tileCollide = true; // Can the projectile collide with tiles?
            aiType = ProjectileID.FallingStar; // Act exactly like default Falling Star
        }

        public override void AI()
        {
            // Custom AI here

            // Make the projectile face the right way
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            // Add dust trail
            Dust dust;
            Vector2 position = projectile.Center;
            dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.Fire, 0f, 0f, 0, new Color(255,255,255), 1f)];
            dust.noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Explode on tile collision and create smaller star projectiles
            Main.PlaySound(SoundID.Item14, (int)projectile.position.X, (int)projectile.position.Y); // Play explosion sound
            for (int i = 0; i < 10; i++) // Create 10 smaller star projectiles
            {
                Vector2 perturbedSpeed = new Vector2(oldVelocity.X, oldVelocity.Y).RotatedByRandom(MathHelper.ToRadians(360)); // Randomize the velocity
                perturbedSpeed *= Main.rand.NextFloat(0.5f, 1f); // Randomize the speed
                Projectile.NewProjectile(projectile.position.X + perturbedSpeed.X, projectile.position.Y + perturbedSpeed.Y, perturbedSpeed.X * 2f, perturbedSpeed.Y * 2f, ModContent.ProjectileType<SmallStar>(), (int)(projectile.damage * .5f), .5f); // Spawn the smaller star projectiles
            }
            
            return true;
        }
    }
}
