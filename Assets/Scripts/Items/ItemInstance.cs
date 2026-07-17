namespace AF
{
    public class ItemInstance
    {
        public string id;
        public Item itemData;

        public ItemInstance(string id, Item itemData)
        {
            this.id = id;
            this.itemData = itemData;
        }
    }
}
