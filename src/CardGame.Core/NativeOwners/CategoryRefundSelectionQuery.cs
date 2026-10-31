#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CategoryRefundSelectionQuery(Func<RuntimeEntries.CoreGameEngineDrawDiscardCategoryRefundEntries> getCoreGameEngineDrawDiscardCategoryRefundRuntime) : ICategoryRefundSelectionQuery
{
    public bool CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => getCoreGameEngineDrawDiscardCategoryRefundRuntime().DrawDiscardCategoryTargetAvailable(ownerSeat, skillId, stateId, targetSeat);
}
