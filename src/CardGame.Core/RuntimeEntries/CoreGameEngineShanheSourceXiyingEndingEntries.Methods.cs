#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceXiyingEndingEntries
{
    internal void AppendSourceXiyingEnding(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendSourceXiyingEnding(items);
    internal void AssertSourceXiyingFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSourceXiyingFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSourceXiyingReward(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginSourceXiyingReward(f);
    internal bool CanOfferSourceXiying(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceXiying(gate);
    internal bool CollectSourceXiying(global::CardGame.Core.SkillProgramTrigger t) => _resolve().CollectSourceXiying(t);
    internal global::CardGame.Core.ShanheSourceXiyingPaidEvent? ExactSourceXiyingEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ExactSourceXiyingEnding(candidate, c);
    internal bool IsIssuedSourceXiyingEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedSourceXiyingEnding(candidate, c);
    internal bool KeepPaidSourceXiying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidSourceXiying(f);
    internal bool SourceXiyingEarned(global::CardGame.Core.ShanheSourceXiyingPaidEvent paid) => _resolve().SourceXiyingEarned(paid);
    internal bool SourceXiyingTurnHasOwnPlayDamage(global::CardGame.Core.ShanheSourceXiyingPaidEvent paid) => _resolve().SourceXiyingTurnHasOwnPlayDamage(paid);
    internal bool ValidSourceXiyingPaid(global::CardGame.Core.ShanheSourceXiyingPaidEvent paid) => _resolve().ValidSourceXiyingPaid(paid);
}
