#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDingYuanOlRuntime
{
    internal bool TracksRoundFirstDamage => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.QualifyFirstRoundDamage) == true;

    internal RoundFirstSourcedDamageEvent? FirstRoundDamageFor(ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.DamageFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Any(window => window.Id == context.ParentFrameId && window.ParentFrameId == id) ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundFirstSourcedDamageEvent>().SingleOrDefault(fact => fact.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && fact.DamageFrameId == id && fact.SourceSeat == context.SourceSeat && fact.TargetSeat == context.TargetSeat && fact.Amount == context.Amount) : null;
    internal void QualifyFirstRoundDamage(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || frame.WindowContext is not { } context || FirstRoundDamageFor(context)is not { } first || !CanOfferDingYuan(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(frame), context))
            throw new InvalidOperationException("First-round damage qualification requires the exact applied native damage and source instance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstRoundDamageQualifiedEvent(frame.Id, first.DamageFrameId, first.RoundNumber, frame.OwnerSeat, frame.SkillInstanceId, frame.TriggerId!));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26020)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26020)]
    internal bool CanContinueIssuedFirstRoundDamage(ProgramSkillFrame frame) => frame.SkillId == "ol:xianshuai" && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstRoundDamageQualifiedEvent>().Any(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat && fact.SkillInstanceId == frame.SkillInstanceId && fact.BindingId == frame.TriggerId && fact.DamageFrameId == frame.WindowContext?.DamageFrameId);
}
