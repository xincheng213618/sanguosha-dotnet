#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuObserversEntries
{
    internal void AssertSourceHuanfuBets(CardUseFrame use) => _resolve().AssertSourceHuanfuBets(use);
    internal bool CanOfferSourceHuanfu(ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceHuanfu(gate);
    internal void CollectIssuedSourceHuanfu(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedSourceHuanfu(action, window, result);
    internal SourceExecutionerHuanfuBet? ExactIssuedSourceHuanfu(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ExactIssuedSourceHuanfu(candidate, context);
    internal bool IsIssuedSourceHuanfu(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedSourceHuanfu(candidate, context);
    internal bool KeepPaidSourceHuanfu(ProgramSkillFrame f) => _resolve().KeepPaidSourceHuanfu(f);
    internal void RecordSourceHuanfuDamage(DamageAppliedEvent applied) => _resolve().RecordSourceHuanfuDamage(applied);
    internal SourceExecutionerHuanfuBet? SourceHuanfuSettlement(int owner, string instance, string hash, CardUseFrame use) => _resolve().SourceHuanfuSettlement(owner, instance, hash, use);
    internal bool ValidPaidSourceHuanfuFrame(ProgramSkillFrame f) => _resolve().ValidPaidSourceHuanfuFrame(f);
}
