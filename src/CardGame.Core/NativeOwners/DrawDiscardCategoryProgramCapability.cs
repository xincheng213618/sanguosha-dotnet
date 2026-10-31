#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DrawDiscardCategoryProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime>> getCoreGameEngineDrawDiscardCategoryRefundRuntime) : IDrawDiscardCategoryProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardDistinctCategories(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => getCoreGameEngineDrawDiscardCategoryRefundRuntime().Value.BeginDrawDiscardCategoryRefund(f, targetSeat, amount, stateId);
}
