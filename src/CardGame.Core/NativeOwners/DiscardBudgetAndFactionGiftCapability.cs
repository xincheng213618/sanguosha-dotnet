#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardBudgetAndFactionGiftCapability(Func<RuntimeEntries.CoreGameEngineDiscardBudgetAndFactionGiftsEntries> getCoreGameEngineDiscardBudgetAndFactionGiftsRuntime) : IDiscardBudgetAndFactionGiftHost
{
    public SkillProgramStepOutcome ResolveDiscardBudgetParticipants(ProgramSkillFrame f) => getCoreGameEngineDiscardBudgetAndFactionGiftsRuntime().BeginDiscardBudget(f);
    public SkillProgramStepOutcome PreventOwnPlayOutsideTargetRangeDamage(ProgramSkillFrame f) => getCoreGameEngineDiscardBudgetAndFactionGiftsRuntime().PreventOwnPlayOutsideRange(f);
    public SkillProgramStepOutcome DiscardOutsideRangeAfterInsufficientUses(ProgramSkillFrame f) => getCoreGameEngineDiscardBudgetAndFactionGiftsRuntime().BeginOutsideRangeDiscard(f);
    public SkillProgramStepOutcome OfferCompletedFactionCostGift(ProgramSkillFrame f, string faction) => getCoreGameEngineDiscardBudgetAndFactionGiftsRuntime().BeginCompletedFactionGift(f, faction);
}
