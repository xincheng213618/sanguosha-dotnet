#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeAnniversaryEntries
{
    internal IEnumerable<AnniversaryKuanshiGrant> ActiveAnniversaryKuanshiGrants() => _resolve().ActiveAnniversaryKuanshiGrants();
    internal AnniversaryKuanshiRecoveryReceipt? AnniversaryKuanshiWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().AnniversaryKuanshiWitness(c, w);
    internal void AppendAnniversaryKanZeDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> result) => _resolve().AppendAnniversaryKanZeDamageCandidates(attack, window, result);
    internal void ArmAnniversaryKuanshi(ProgramSkillFrame supplied, int target) => _resolve().ArmAnniversaryKuanshi(supplied, target);
    internal void BindAnniversaryKuanshiDamage(ProgramSkillFrame supplied) => _resolve().BindAnniversaryKuanshiDamage(supplied);
    internal bool ContinueAnniversaryKuanshiIssuedFrame(ProgramSkillFrame f) => _resolve().ContinueAnniversaryKuanshiIssuedFrame(f);
    internal void ExpireAnniversaryKuanshi(TurnStartedEvent fact) => _resolve().ExpireAnniversaryKuanshi(fact);
    internal bool HasIssuedAnniversaryKuanshiFrame(ProgramSkillFrame f) => _resolve().HasIssuedAnniversaryKuanshiFrame(f);
    internal bool IsIssuedAnniversaryKanZeKuanshiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedAnniversaryKanZeKuanshiCandidate(c, w);
    internal void IssueAnniversaryKuanshiDamage(DamageAppliedEvent fact) => _resolve().IssueAnniversaryKuanshiDamage(fact);
    internal bool HasAnniversaryKuanshi { get => _resolve().HasAnniversaryKuanshi; }
}
