using Terraria;
using Terraria.ModLoader;

namespace MyMod.Projectiles
{
    public class LightBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Beam");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 2;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            // Make the projectile face upwards
            projectile.rotation = -MathHelper.PiOver2;

            // Spawn dust along the beam
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GoldFlame);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.scale *= 1.2f;
                dust.position.X -= projectile.velocity.X / 3f * i;
                dust.position.Y -= projectile.velocity.Y / 3f * i;
            }

            // Check if the beam hits a valid target
            if (projectile.localAI[1] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                int num = 3;
                for (int j = 0; j < num; j++)
                {
                    Vector2 vector = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                    float num12 = Main.player[projectile.owner].Center.X - vector.X;
                    float num13 = Main.player[projectile.owner].Center.Y - vector.Y;
                    float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
                    num14 = 12f / num14;
                    vector.X += num12 * num14;
                    vector.Y += num13 * num14;
                    float speedX = Main.player[projectile.owner].Center.X - vector.X + Main.rand.Next(-40, 41) * 0.03f;
                    float speedY = Main.player[projectile.owner].Center.Y - vector.Y + Main.rand.Next(-40, 41) * 0.03f;
                    float num17 = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    num17 = 12f / num17;
                    speedX *= num17;
                    speedY *= num17;

                    // Shoot a ray of light from the sky to the target
                    Projectile.NewProjectile(vector.X, vector.Y, speedX, speedY, ModContent.ProjectileType<LightRay>(), (int)(projectile.damage * 0.5f), projectile.knockBack, projectile.owner, 0f, 0f);
                }
                projectile.localAI[1] = 1f;
            }
        }

        // Change the velocity to be faster
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 6f / magnitude;
            }
        }
    }
}
