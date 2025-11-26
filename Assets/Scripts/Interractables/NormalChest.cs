public class NormalChest : BaseChest
{
    protected override ItemData DecideItem()
    {
        return GameSession.Instance.PickFromPool(GameSession.Instance.normalPool);
    }
}
