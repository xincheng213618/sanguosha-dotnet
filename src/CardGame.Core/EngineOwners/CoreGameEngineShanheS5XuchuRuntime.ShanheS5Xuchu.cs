#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5XuchuRuntime
{
    internal const string ShanheS5LuoyiNumberPrefix = "shanhe-s5-luoyi-x:";
    void global::CardGame.Core.IShanheS5LuoyiProgramHost.ResolveShanheS5Luoyi(ProgramSkillFrame frame, SkillProgramEffect effect) => ResolveShanheS5Luoyi(frame, effect);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5710, SkillProgramEffectOp.ResolveShanheS5Luoyi)]
    internal bool CanOfferShanheS5Luoyi(ProgramTriggerLegalityContext c) => c.Owner.HasHpValue && c.Owner.Hp > 0 && c.Owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && c.Context.Window == SkillProgramTriggerWindow.DrawPhaseStarting;
    internal ProgramLifecycleTriggerWindowFrame ShanheS5LuoyiDrawPlan(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 1 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != f.OwnerSeat || parent.Window != context.Window || parent.Continuation != ProgramLifecycleContinuation.CompleteDrawPhase || parent.NormalDrawReplaced || parent.ActualPhaseSubstitution is not null)
            throw new InvalidOperationException("S5 Luoyi lost its actual owner's pending draw phase.");
        return parent;
    }

    internal void ResolveShanheS5Luoyi(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var parent = ShanheS5LuoyiDrawPlan(f);
        if (effect.StateId == "reveal")
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            if (!owner.HasHpValue || owner.Hp <= 0)
                throw new InvalidOperationException("S5 Luoyi requires defined positive current HP.");
            var key = ShanheS5LuoyiNumberPrefix + effect.ResultBind;
            if (f.NumberBindings.Any(b => b.Name == key))
                throw new InvalidOperationException("S5 Luoyi cannot freeze its reveal twice.");
            var x = owner.Hp;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NumberBindings = f.NumberBindings.Append(new ProgramSkillNumberBinding(key, x)).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5LuoyiFrozenEvent(f.Id, f.InstructionIndex, f.SkillId, f.SkillInstanceId, f.GameplayHash, effect.ResultBind!, parent.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, x));
            _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, x, null, effect.ResultBind!, SkillProgramCardSetVisibility.Public);
            return;
        }

        var frozen = ShanheS5LuoyiFrozen(f, effect.SourceBind!);
        if (frozen.DrawPlanFrameId != parent.Id || frozen.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || frozen.InstructionIndex >= f.InstructionIndex || _coreProgramEventHistoryOperations.ProgramEventHistory<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.ParentFrameId == f.Id))
            throw new InvalidOperationException("S5 Luoyi lost its unique frozen reveal before draw substitution.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, SkillProgramTurnPhase.Draw) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, SkillProgramTriggerWindow.DrawPhaseStarting, SkillProgramTurnPhase.Draw));
        _coreCardUseModulesOperations.GrantProgramTurnCardDamageModifier(f, [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.Duel], frozen.X, SkillProgramDamageModifierExpiration.NextOwnerTurnStart, SkillProgramDamageModifierSourceScope.DamageSource);
    }

    internal ShanheS5LuoyiFrozenEvent ShanheS5LuoyiFrozen(ProgramSkillFrame f, string bind)
    {
        var values = f.NumberBindings.Where(b => b.Name == ShanheS5LuoyiNumberPrefix + bind).ToArray();
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5LuoyiFrozenEvent>().Where(e => e.FrameId == f.Id && e.Bind == bind).ToArray();
        var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Where(e => e.FrameId == f.Id && e.Bind == bind).ToArray();
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (values is not [var value] || value.Value <= 0 || facts is not [var fact] || fact.X != value.Value || fact.SkillId != f.SkillId || fact.SkillInstanceId != f.SkillInstanceId || fact.GameplayHash != f.GameplayHash || fact.OwnerSeat != f.OwnerSeat || fact.InstructionIndex < 1 || revealed is not [var shown] || shown.SkillId != f.SkillId || shown.OwnerSeat != f.OwnerSeat || shown.BindingId != global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) || shown.Cards.Count > fact.X || !shown.Cards.Select(c => c.Id).SequenceEqual(set.CardIds) || set.Visibility != SkillProgramCardSetVisibility.Public)
            throw new InvalidOperationException("S5 Luoyi's current-HP value lost its owning public reveal.");
        return fact;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 5710)]
    internal void AssertShanheS5LuoyiFrozen(ProgramSkillFrame f)
    {
        foreach (var value in f.NumberBindings.Where(b => b.Name.StartsWith(ShanheS5LuoyiNumberPrefix, StringComparison.Ordinal)))
            _ = ShanheS5LuoyiFrozen(f, value.Name[ShanheS5LuoyiNumberPrefix.Length..]);
    }
}
