#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGuanduEventProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IGuanduEventHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteGuanduEvent(f, op);
    internal GuanduBattleEvent? CurrentGuanduEvent => _coreActivityBattleRulesOperations.GuanduRules?.CurrentEvent;

    internal void RecordGuanduCardAction(CardActionContext action, SkillProgramTriggerWindow window) => _coreActivityBattleRulesOperations.GuanduRules?.RecordCardAction(new(action.ActionId, action.Type, action.ActorSeat, action.EffectiveKind), window);
    internal bool CanUseGuanduRoundConversion(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => _coreActivityBattleRulesOperations.GuanduRules?.CanUseRoundConversion(instance.SkillId, rule.Id, owner.Seat) ?? (instance.SkillId != "guandu:battle-event" || rule.Id != "hold-line");
    internal void ConsumeGuanduRoundConversion(CardConversionSource source)
    {
        if (_coreActivityBattleRulesOperations.GuanduRules is { } rules)
            rules.ConsumeRoundConversion(source);
        else if (source.SkillId == "guandu:battle-event" && source.BindingId == "hold-line")
            throw new InvalidOperationException("The activity conversion feature is not installed.");
    }

    internal CardUseFrame? GuanduDamageRewardUse(int owner, ProgramSkillWindowContext context) => context.SourceSeat == owner && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } damage && damage.SourceSeat == owner && _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ResolutionId)is { Action.Type: CardActionType.Use } use && _coreActivityBattleRulesOperations.GuanduRules?.DamageRewardAvailable(use.Id) == true ? use : null;
    internal CardUseFrame? GuanduTenthUse(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } c || _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is not { Action: { } action, ExtraCardEffect: null } use || action.ActionId != c.CardActionId || action.ActorSeat != owner || action.Type != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(use.CardKind) || use.CardKind is CardKind.Dodge or CardKind.Nullification)
            return null;
        return _coreActivityBattleRulesOperations.GuanduRules?.IsTenthAction(action.ActionId) == true ? use : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46000)]
    internal bool AllowsGuanduEvent(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "guandu:battle-event")
            return true;
        var context = gate.WindowContext;
        return _coreActivityBattleRulesOperations.GuanduRules?.AllowsEvent(new(gate.SkillId, gate.Candidate.BindingId, gate.OwnerSeat, context is not null, context?.Amount ?? 0, context is not null && GuanduTenthUse(gate.OwnerSeat, context)is not null, context is not null && GuanduDamageRewardUse(gate.OwnerSeat, context)is not null)) ?? false;
    }

    internal SkillProgramStepOutcome ExecuteGuanduEvent(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        return (_coreActivityBattleRulesOperations.GuanduRules ?? throw new InvalidOperationException("The activity event feature is not installed.")).ExecuteEvent(frame.Id, frame.OwnerSeat, op);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46000)]
    internal bool ResumeGuanduEventFlow(long id) => _coreActivityBattleRulesOperations.GuanduRules?.ResumeEvent(id) ?? false;
    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardGuanduDamage)]
    internal void ReturnGuanduEventDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(f.Id, f.OwnerSeat, null, SkillProgramCardSetVisibility.Private, new("guandu.grain-shortage.draw"), 0, outcome.ActualDrawIds);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46000)]
    internal bool RetainGuanduEventFlow(ProgramSkillFrame f) => f.GuanduEventFlow is not null;
    [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 30)]
    internal bool GuanduRoundConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseGuanduRoundConversion(query.Instance, query.Rule, query.Owner);
}
