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
        public int BaseQuantity { get; internal set; }
        public int CurrentQuantity { get; internal set; }
        public double QuantityPercentageRemaining
        {
            get { return (double)CurrentQuantity / (double)BaseQuantity; }
        }

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            BaseQuantity = GetBaseQuantity();
            CurrentQuantity = BaseQuantity;
        }

        // TODO: Choose base quantity based on more factors. 
        // e.i. Block type, Position in cluster(At egde vs. Towards center).
        private int GetBaseQuantity()
        {
            return 10;
        }
    }

}
