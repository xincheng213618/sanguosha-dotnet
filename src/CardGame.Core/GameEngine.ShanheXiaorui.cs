namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheXiaoruiCapability(GameEngine engine) : IShanheXiaoruiProgramHost
    {
        public SkillProgramStepOutcome GrantActualTurnSlashQuota(ProgramSkillFrame frame) => engine.GrantShanheXiaoruiQuota(frame);
    }

    private bool IsShanheXiaoruiDamage(int ownerSeat, ProgramSkillWindowContext context) =>
        context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source,
            Facts.DamageCardIsSlash: true } && source == _currentSeat && IsValidPlayerSeat(source) &&
        _players[ownerSeat].IsAlive && _players[source].IsAlive &&
        (ownerSeat == source || OtherRevealedPublicTeamAlly(_players[ownerSeat], _players[source]));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27980)]
    private bool CanOfferShanheXiaorui(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantShanheXiaoruiSlashQuota) ||
        IsShanheXiaoruiDamage(candidate.OwnerSeat, context);

    private SkillProgramStepOutcome GrantShanheXiaoruiQuota(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        ValidateProgramTurnEffectGrant(frame);
        if (frame.WindowContext is not { } context || !IsShanheXiaoruiDamage(frame.OwnerSeat, context))
            return SkillProgramStepOutcome.Continue;
        var grant = _turnCardUseEffects.GrantRuleModifier(_turnNumber, _currentSeat, frame.Id, frame.InstructionIndex - 1,
            CreateProgramTurnEffectSource(frame), SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, 1,
            affectedSeat: context.SourceSeat!.Value);
        AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }
}
