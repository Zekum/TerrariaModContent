using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace MyMod
{
    public class MyWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            // Find the index of the Granite Cave pass
            int graniteIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite Caves"));

            // Insert a new pass after the Granite Cave pass
            if (graniteIndex != -1)
            {
                tasks.Insert(graniteIndex + 1, new PassLegacy("Deadmans Caves", GenerateDeadmansCaves));
            }
        }

        private void GenerateDeadmansCaves(GenerationProgress progress)
        {
            // Set the progress message
            progress.Message = "Creating Deadmans Caves";

            // Loop 10 times to generate 10 caves
            for (int i = 0; i < 10; i++)
            {
                // Choose a random position in the underground layer
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200);

                // Dig a cave using a worm algorithm
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(100, 200), WorldGen.genRand.Next(50, 100), TileID.Stone);

                // Place a deadmans chest at the end of the cave
                int chestX = x;
                int chestY = y;

                // Find the lowest solid tile in the cave
                while (!WorldGen.SolidTile(chestX, chestY) && chestY < Main.maxTilesY - 50)
                {
                    chestY++;
                }

                // Place the chest on top of the solid tile
                WorldGen.PlaceTile(chestX, chestY - 1, TileID.Containers);
            
                // Get the chest index
                int chestIndex = Chest.FindChest(chestX, chestY - 1);

                // If the chest is valid, set its type to deadmans chest and add some loot
                if (chestIndex >= 0)
                {
                    Main.chest[chestIndex].chestStyle = ChestStyleID.DeadMansChest;

                    // Add some random loot to the chest
                    Item[] loot = new Item[]
                    {
                        new Item { type = ItemID.GoldCoin, stack = WorldGen.genRand.Next(5, 11) },
                        new Item { type = ItemID.Grenade, stack = WorldGen.genRand.Next(10, 21) },
                        new Item { type = ItemID.Dynamite, stack = WorldGen.genRand.Next(3, 7) },
                        new Item { type = ItemID.SilverBullet, stack = WorldGen.genRand.Next(50, 101) },
                        new Item { type = ItemID.GoldenKey },
                        new Item { type = ItemID.BoneWelder }
                    };

                    // Shuffle the loot array
                    for (int j = 0; j < loot.Length; j++)
                    {
                        int k = WorldGen.genRand.Next(j, loot.Length);
                        Item temp = loot[j];
                        loot[j] = loot[k];
                        loot[k] = temp;
                    }

                    // Fill the chest with the loot
                    for (int j = 0; j < loot.Length; j++)
                    {
                        Main.chest[chestIndex].item[j] = loot[j];
                    }
                }
            }
        }
    }
}
