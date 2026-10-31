#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheZhanhunDaoRuntime
{
    internal const string ShanheZhanhunDaoSkill = "shanhe:zhanhun-dao";
    SkillProgramStepOutcome IShanheZhanhunDaoProgramHost.ResolveShanheZhanhunDao(ProgramSkillFrame frame) => ResolveShanheZhanhunDao(frame);
    internal bool HasActiveShanheZhanhunDao(int ownerSeat, string skillId, string skillInstanceId) => skillId == ShanheZhanhunDaoSkill && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(card => card.Kind == CardKind.ZhanhunDao && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, skillInstanceId, card));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5754, SkillProgramEffectOp.ResolveShanheZhanhunDao)]
    internal bool CanOfferShanheZhanhunDao(ProgramTriggerLegalityContext gate) => HasActiveShanheZhanhunDao(gate.OwnerSeat, gate.SkillId, gate.Candidate.SkillInstanceId) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } damage && damage.SourceSeat == gate.OwnerSeat && gate.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } targetSeat } && damage.TargetSeat == targetSeat && _coreMatchParticipantsPlayersAccess._players[targetSeat] is { IsAlive: true, HasHpValue: true } target && _coreTreatedWoundedOperations.IsRuleWounded(target);
    internal SkillProgramStepOutcome ResolveShanheZhanhunDao(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!HasActiveShanheZhanhunDao(f.OwnerSeat, f.SkillId, f.SkillInstanceId) || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented || window.SourceSeat != f.OwnerSeat || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } damage || damage.SourceSeat != f.OwnerSeat || damage.TargetSeat != window.TargetSeat || window.Amount <= 0 || _coreMatchParticipantsPlayersAccess._players[window.TargetSeat] is not { IsAlive: true, HasHpValue: true } target || !_coreTreatedWoundedOperations.IsRuleWounded(target))
            throw new InvalidOperationException("Zhanhun Dao requires its actual equipped source and current wounded damage target.");
        var before = target.MaxHp;
        var after = _coreWushuangTacticRulesOperations.HasWushuangTactic(target.Seat, 2020) ? 7 : target.HasShenPangTongKunyu ? 1 : Math.Max(0, checked(before - 1));
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
        if (!_characterDaQiaoXiaoQiaoHappyGodOperations.TryApplyHappyQiaoMaximumHp(f.Id, target.Seat, after, f.SkillId))
        {
            var hp = target.Hp;
            target.MaxHp = after;
            target.Hp = Math.Min(hp, after);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(target.Seat, after - before, after, f.SkillId) { HpBeforeClamp = hp });
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheZhanhunDaoResolvedEvent(f.Id, window.Id, damage.ResolutionId, f.OwnerSeat, target.Seat, window.Amount, before, after));
        if (after == 0 && target.IsAlive)
        {
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, target, null, null, null, causingProgramSkillFrameId: f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }
}
