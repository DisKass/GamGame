using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Items
{
    public enum InitializeReason
    {
        PICKUPPED,
        LOADED
    }
    public interface IItemEffect
    {
        void Initialize(InitializeReason reason);
    }
}
