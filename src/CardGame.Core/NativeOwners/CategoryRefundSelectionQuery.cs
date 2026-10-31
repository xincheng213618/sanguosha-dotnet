#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CategoryRefundSelectionQuery(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime>> getCoreGameEngineDrawDiscardCategoryRefundRuntime) : ICategoryRefundSelectionQuery
{
    public bool CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => getCoreGameEngineDrawDiscardCategoryRefundRuntime().Value.DrawDiscardCategoryTargetAvailable(ownerSeat, skillId, stateId, targetSeat);
}
