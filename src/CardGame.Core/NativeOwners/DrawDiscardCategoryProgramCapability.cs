#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DrawDiscardCategoryProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawDiscardCategoryRefundEntries> getCoreGameEngineDrawDiscardCategoryRefundRuntime) : IDrawDiscardCategoryProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardDistinctCategories(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => getCoreGameEngineDrawDiscardCategoryRefundRuntime().BeginDrawDiscardCategoryRefund(f, targetSeat, amount, stateId);
}
