#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31600, SkillProgramEffectOp.ResolveMouLuBuWushuangBonus)]
    internal bool CanMouLbBonus(int owner, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.BeforeDamageApplied && c.SourceSeat == owner && c.TargetSeat is { } victim && victim != owner && c.Amount > 0 && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } attack && attack.SourceSeat == owner && attack.TargetSeat == victim && attack.EffectiveCardKind is { } kind && (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind == CardKind.Duel) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuBuWushuangBonusEvent>().Any(e => e.Source.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber) && (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? attack is NativeOwners.CardAttackHandle slash && slash.SuccessfulDodgeResponses == 0 : !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DuelResponseEvent>().Any(e => e.ResolutionId == attack.ResolutionId && e.ResponderSeat == victim && e.UsedSlash));
    internal SkillProgramStepOutcome ResolveMouLbBonus(ProgramSkillFrame f)
    {
        var c = f.WindowContext!;
        if (!CanMouLbBonus(f.OwnerSeat, c))
            return SkillProgramStepOutcome.Continue;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not BeforeDamageProgramWindowFrame window || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack)
            throw new InvalidOperationException("Mobile Wushuang lost the original damage window.");
        attack.IncreaseFinalizedDamageAmount(1);
        _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Amount = checked(window.Amount + 1) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuWushuangBonusEvent(f.Id, MouLbSource(f), attack.ResolutionId, window.Id, _coreActualTurnProgressTurnNumberAccess._turnNumber, attack.TargetSeat, attack.EffectiveCardKind!.Value, window.Amount, window.Amount + 1));
        return SkillProgramStepOutcome.Continue;
    }
}
