#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DrawDiscardCategoryProgramCapability(Func<RuntimeEntries.CoreGameEngineDrawDiscardCategoryRefundEntries> getCoreGameEngineDrawDiscardCategoryRefundRuntime) : IDrawDiscardCategoryProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardDistinctCategories(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => getCoreGameEngineDrawDiscardCategoryRefundRuntime().BeginDrawDiscardCategoryRefund(f, targetSeat, amount, stateId);
}
