#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuObserversEntries
{
    internal void AssertSourceHuanfuBets(global::CardGame.Core.CardUseFrame use) => _resolve().AssertSourceHuanfuBets(use);
    internal void AssertSourceHuanfuFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSourceHuanfuFrame(f);
    internal bool CanOfferSourceHuanfu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceHuanfu(gate);
    internal void CollectIssuedSourceHuanfu(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedSourceHuanfu(action, window, result);
    internal global::CardGame.Core.SourceExecutionerHuanfuBet? ExactIssuedSourceHuanfu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ExactIssuedSourceHuanfu(candidate, context);
    internal bool IsIssuedSourceHuanfu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedSourceHuanfu(candidate, context);
    internal bool KeepPaidSourceHuanfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidSourceHuanfu(f);
    internal void RecordSourceHuanfuDamage(global::CardGame.Core.DamageAppliedEvent applied) => _resolve().RecordSourceHuanfuDamage(applied);
    internal global::CardGame.Core.SourceExecutionerHuanfuBet? SourceHuanfuSettlement(int owner, string instance, string hash, global::CardGame.Core.CardUseFrame use) => _resolve().SourceHuanfuSettlement(owner, instance, hash, use);
    internal bool ValidPaidSourceHuanfuFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPaidSourceHuanfuFrame(f);
}
