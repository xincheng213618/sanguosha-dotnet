#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeAnniversaryEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryKuanshiGrant> ActiveAnniversaryKuanshiGrants() => _resolve().ActiveAnniversaryKuanshiGrants();
    internal global::CardGame.Core.AnniversaryKuanshiRecoveryReceipt? AnniversaryKuanshiWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AnniversaryKuanshiWitness(c, w);
    internal void AppendAnniversaryKanZeDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => _resolve().AppendAnniversaryKanZeDamageCandidates(attack, window, result);
    internal void ArmAnniversaryKuanshi(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().ArmAnniversaryKuanshi(supplied, target);
    internal void BindAnniversaryKuanshiDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BindAnniversaryKuanshiDamage(supplied);
    internal bool ContinueAnniversaryKuanshiIssuedFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAnniversaryKuanshiIssuedFrame(f);
    internal void ExpireAnniversaryKuanshi(global::CardGame.Core.TurnStartedEvent fact) => _resolve().ExpireAnniversaryKuanshi(fact);
    internal bool HasIssuedAnniversaryKuanshiFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasIssuedAnniversaryKuanshiFrame(f);
    internal bool IsIssuedAnniversaryKanZeKuanshiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedAnniversaryKanZeKuanshiCandidate(c, w);
    internal void IssueAnniversaryKuanshiDamage(global::CardGame.Core.DamageAppliedEvent fact) => _resolve().IssueAnniversaryKuanshiDamage(fact);
    internal void RequireAnniversaryKuanshiRecoveryFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireAnniversaryKuanshiRecoveryFrame(f);
    internal bool HasAnniversaryKuanshi { get => _resolve().HasAnniversaryKuanshi; }
}
