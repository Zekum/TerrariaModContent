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
                tasks.Insert(graniteIndex + 1, new PassLegacy("Sacrificial Rooms", GenerateSacrificialRooms));
            }
        }

        private void GenerateSacrificialRooms(GenerationProgress progress)
        {
            // Set the progress message
            progress.Message = "Creating Sacrificial Rooms";

            // Loop 10 times to generate 10 rooms
            for (int i = 0; i < 10; i++)
            {
                // Choose a random position in the underground layer
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200);

                // Define the size of the room
                int width = WorldGen.genRand.Next(20, 31);
                int height = WorldGen.genRand.Next(10, 16);

                // Clear a rectangular area for the room
                WorldGen.ClearRectangle(x - width / 2, y - height / 2, width, height);

                // Place sacrificial bricks around the room
                WorldGen.TileFrame(x - width / 2 - 1, y - height / 2 - 1, width + 2, height + 2, TileID.SacrificialBrick);

                // Place two entrances on the left and right sides of the room
                WorldGen.ClearRectangle(x - width / 2 - 1, y - 1, 2, 3);
                WorldGen.ClearRectangle(x + width / 2, y - 1, 2, 3);

                // Place a sacrificial altar in the center of the room
                WorldGen.PlaceTile(x, y + height / 2 - 1, TileID.SacrificialAltar);

                // Place some blood candles around the room
                WorldGen.PlaceTile(x - width / 4, y + height / 2 - 1, TileID.BloodCandle);
                WorldGen.PlaceTile(x + width / 4, y + height / 2 - 1, TileID.BloodCandle);
                WorldGen.PlaceTile(x - width / 4, y - height / 4 - 1, TileID.BloodCandle);
                WorldGen.PlaceTile(x + width / 4, y - height / 4 - 1, TileID.BloodCandle);

                // Place some bones and skulls on the floor of the room
                for (int j = x - width / 2 + 1; j < x + width / 2; j++)
                {
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        WorldGen.PlaceTile(j, y + height / 2 - 1, TileID.BoneBlock);
                    }
                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        WorldGen.PlaceTile(j, y + height / 2 - 2, TileID.Skull);
                    }
                }
            }
        }
    }
}
