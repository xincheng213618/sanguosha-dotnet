#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceXiyingEndingEntries
{
    internal void AppendSourceXiyingEnding(List<TurnEndingBoundaryItem> items) => _resolve().AppendSourceXiyingEnding(items);
    internal SkillProgramStepOutcome BeginSourceXiyingReward(ProgramSkillFrame f) => _resolve().BeginSourceXiyingReward(f);
    internal bool CanOfferSourceXiying(ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceXiying(gate);
    internal bool CollectSourceXiying(SkillProgramTrigger t) => _resolve().CollectSourceXiying(t);
    internal ShanheSourceXiyingPaidEvent? ExactSourceXiyingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().ExactSourceXiyingEnding(candidate, c);
    internal bool IsIssuedSourceXiyingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedSourceXiyingEnding(candidate, c);
    internal bool KeepPaidSourceXiying(ProgramSkillFrame f) => _resolve().KeepPaidSourceXiying(f);
    internal bool SourceXiyingEarned(ShanheSourceXiyingPaidEvent paid) => _resolve().SourceXiyingEarned(paid);
    internal bool SourceXiyingTurnHasOwnPlayDamage(ShanheSourceXiyingPaidEvent paid) => _resolve().SourceXiyingTurnHasOwnPlayDamage(paid);
    internal bool ValidSourceXiyingPaid(ShanheSourceXiyingPaidEvent paid) => _resolve().ValidSourceXiyingPaid(paid);
}
