#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheXiaoruiRuntime
{
    SkillProgramStepOutcome IShanheXiaoruiProgramHost.GrantActualTurnSlashQuota(ProgramSkillFrame frame) => GrantShanheXiaoruiQuota(frame);
    internal bool IsShanheXiaoruiDamage(int ownerSeat, ProgramSkillWindowContext context) => context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, Facts.DamageCardIsSlash: true } && source == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && (ownerSeat == source || _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[source]));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27980)]
    internal bool CanOfferShanheXiaorui(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantShanheXiaoruiSlashQuota) || IsShanheXiaoruiDamage(candidate.OwnerSeat, context);
    internal SkillProgramStepOutcome GrantShanheXiaoruiQuota(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (frame.WindowContext is not { } context || !IsShanheXiaoruiDamage(frame.OwnerSeat, context))
            return SkillProgramStepOutcome.Continue;
        var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, 1, affectedSeat: context.SourceSeat!.Value);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }
}
