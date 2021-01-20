using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace depletingores.src.blockEntity
{
    public class DepletingOreEntity : BlockEntity
    {
        private List<Item> lootTable;
        public int BaseQuantity { get; internal set; }
        public int CurrentQuantity { get; internal set; }
        public double QuantityPercentageRemaining
        {
            get { return (double)CurrentQuantity / (double)BaseQuantity; }
        }

        private IWorldAccessor _world;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            _world = api.World;
            BaseQuantity = 10;
            CurrentQuantity = BaseQuantity;
        }

        public Item GenerateItem(double quantityPercentageRemaining, IWorldAccessor world)
        {
            if (lootTable == null)
            {
                lootTable = new List<Item> {
                    world.GetItem(new AssetLocation("pickaxe-copper")),
                    world.GetItem(new AssetLocation("pickaxe-iron")),
                    world.GetItem(new AssetLocation("pickaxe-gold"))
                };
            }

            int index = (int)Math.Round((lootTable.Count - 1) * quantityPercentageRemaining);
            return lootTable[index];
        }

        public override void OnBlockBroken()
        {
            base.OnBlockBroken();
            //Debug message to chat
            if (_world.Api.Side.IsServer())
            {
                var sapi = _world.Api as ICoreServerAPI;
                sapi.BroadcastMessageToAllGroups(string.Format("a:{0} b:{1} c:{2}", BaseQuantity, CurrentQuantity, QuantityPercentageRemaining), EnumChatType.AllGroups);
            }

            //Generate and drop loot
            var itemToDrop = GenerateItem(QuantityPercentageRemaining, _world);
            var itemStackDrop = new ItemStack(itemToDrop);
            _world.SpawnItemEntity(itemStackDrop, this.Pos.ToVec3d());
            CurrentQuantity--;

            //Replace destroyed block if quantity is above zero. (Alternative: The block should be destroyed.)
            if (CurrentQuantity > 0)
            {
                //TODO|WARNING: This is not preventing the blocks default behavior of getting destroyed and therefore deleting this entity..
                _world.BlockAccessor.ExchangeBlock(Block.Id, this.Pos);
            }
        }
    }

}
