using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod.Projectiles
{
    public class SoulFragment : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Display name of the projectile
            DisplayName.SetDefault("Soul Fragment");
        }

        public override void SetDefaults()
        {
            // Basic projectile properties
            projectile.width = 8; // The width of projectile hitbox
            projectile.height = 8; // The height of projectile hitbox
            projectile.aiStyle = 1; // The ai style of the projectile, this is normally 0 for custom AI, which we will use in this case
            projectile.friendly = true; // Can the projectile deal damage to enemies?
            projectile.hostile = false; // Can the projectile deal damage to the player?
            projectile.magic = true; // Is the projectile shoot by a magic weapon?
            projectile.penetrate = -1; // How many monsters the projectile can penetrate. -1 is infinity
            projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            projectile.alpha = 255; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 causes transparency to be reduced by 5 every update cycle)
            projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            projectile.tileCollide = false; // Can the projectile collide with tiles?
            aiType = ProjectileID.Bullet; // Act exactly like default Bullet
        }

        public override void AI()
        {
            // Custom AI here

            // Make the projectile face the right way
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            // Find the nearest enemy within 400 pixels
            float distanceFromTarget = 400f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    if ((closest && inRange) || !foundTarget)
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            // If we found a target, home in on it
            if (foundTarget)
            {
                // Factors for calculations
                float speedFactor = 6f;
                float inertiaFactor = 20f;

                Vector2 direction = targetCenter - projectile.Center;
                direction.Normalize();
                direction *= speedFactor;
                projectile.velocity = (projectile.velocity * (inertiaFactor - 1) + direction) / inertiaFactor;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // Apply the Soul Burn debuff to the target
            target.AddBuff(ModContent.BuffType<SoulBurn>(), 300); // 300 ticks = 5 seconds
        }
    }
}