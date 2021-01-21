using depletingores.src.blockEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace depletingores.src.block
{
    // TODO: Make class extend BlockOre instead of Block.
    public class DepletingOreBlock : Block
    {
        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            DepletingOreEntity entity = (DepletingOreEntity)world.BlockAccessor.GetBlockEntity(pos);

            if (entity.CurrentQuantity != 0)
            {
                //Generate and drop loot
                var itemToDrop = GenerateDropItem(entity.QuantityPercentageRemaining, world);
                var itemStackDrop = new ItemStack(itemToDrop);
                world.SpawnItemEntity(itemStackDrop, pos.ToVec3d());

                entity.CurrentQuantity--;
            }
            else
            {
                //Allow the block to get destroyed.
                base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
            }

        }

        // TODO: Generate item drops based on block type. 
        // e.g. ore-rich-bismuthinite-andesite -> 100% = rich-bismuthinite, 50% = medium-bismuthinite, 25% = poor-bismuthinite.

        private static List<Item> lootTable;
        public Item GenerateDropItem(double quantityPercentageRemaining, IWorldAccessor world)
        {
            if (lootTable == null)
            {
                lootTable = new List<Item> {
                    world.GetItem(new AssetLocation("pickaxe-copper")),
                    world.GetItem(new AssetLocation("pickaxe-iron")),
                    world.GetItem(new AssetLocation("pickaxe-gold"))
                };
            }

            // TODO: Fix weird drop distribution.
            int index = (int)Math.Round((lootTable.Count - 1) * quantityPercentageRemaining);
            return lootTable[index];
        }
    }
}
