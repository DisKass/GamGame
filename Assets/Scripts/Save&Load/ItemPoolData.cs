using System.Collections.Generic;

namespace Assets.Scripts.Game.Items
{
    [System.Serializable]
    public class ItemPoolData : IPersistentData<ItemPools>
    {
        public List<int> RecievedItemsID = new List<int>();
        public void Initialize(ItemPools persistendObject)
        {
            RecievedItemsID = persistendObject.recievedItemsID;
        }
    }
}
