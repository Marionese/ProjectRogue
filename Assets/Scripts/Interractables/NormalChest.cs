public class NormalChest : BaseChest
{
    protected override ItemData DecideItem(int playerIndex)
    {
        return GameSession.Instance.PickFromPool(GameSession.Instance.normalPool);
    }
}
