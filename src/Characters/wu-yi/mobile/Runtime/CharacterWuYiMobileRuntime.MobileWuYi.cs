#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuYiMobileRuntime
{
    [ProgramAiChoice(SkillProgramEffectOp.ApplyMobileWuYiBenxi)]
    internal PromptChoice MobileWuYiBenxiAi(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => decision.Choices.First();
    SkillProgramStepOutcome global::CardGame.Core.IMobileWuYiBenxiHost.Apply(ProgramSkillFrame frame) => ApplyMobileWuYiBenxi(frame);
    internal SkillProgramStepOutcome ApplyMobileWuYiBenxi(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        if (_coreActualTurnProgressCurrentSeatAccess._currentSeat != frame.OwnerSeat || _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != frame.OwnerSeat && _coreNativeRuleQueriesOperations.GetCombatDistance(frame.OwnerSeat, p.Seat) != 1))
            return SkillProgramStepOutcome.Continue;
        if (frame.CardEnhancementDraft is not null)
            throw new InvalidOperationException("Mobile Benxi cannot reissue an unfinished current-card benefit.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { IgnoresArmor = true, Enhancements = use.Enhancements | CurrentCardEnhancement.IgnoreArmor, EnhancementOwnerSeat = frame.OwnerSeat });
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == use.Id)
            attack.SetIgnoresArmor(true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, use.Action!.ActionId, frame.OwnerSeat, CurrentCardEnhancement.IgnoreArmor));
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _coreCurrentCardEnhancementsOperations.CurrentCardExtraTargets(frame).Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CardEnhancementDraft = new(use.Id, Array.AsReadOnly(new[] { CurrentCardEnhancement.IgnoreArmor })) });
        _coreCurrentCardEnhancementsOperations.PublishCardEnhancementPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }
}
