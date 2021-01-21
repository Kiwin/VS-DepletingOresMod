using depletingores.src.block;
using depletingores.src.blockEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace depletingores.src
{
    public class DepletingOresSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("depletingore", typeof(DepletingOreBlock));
            api.RegisterBlockEntityClass("depletingore", typeof(DepletingOreEntity));
        }

    }

}
