#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuJunOlRuntime
{
    internal const string ZhuJunDrawReason = "skill-program.zhu-jun.cuipo.draw";
    internal bool HasZhuJunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveCuipoActualTurnUse);

    internal static int ZhuJunCardNameLength(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhu_jun_ol_ZhuJun.ZhuJunCardNameLength(kind);
    internal static bool IsZhuJunDamageCard(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhu_jun_ol_ZhuJun.IsZhuJunDamageCard(kind);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27030)]
    internal void ObserveZhuJunActualUse(IGameEvent payload)
    {
        if (!HasZhuJunCapability || !_coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn)
            return;
        CardUseFrame? use = null;
        CardActionContext? action = null;
        CurrentTurnCardUseKindsRecordedEvent? ordinal = null;
        CardKind kind;
        int actor;
        if (payload is CardUseDeclaredEvent declared)
        {
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId);
            if (use is null || use.CardId != declared.CardId || use.SourceSeat != declared.SourceSeat || use.CardKind != declared.CardKind)
                throw new InvalidOperationException("Cuipo requires its exact actual native declaration.");
            actor = use.SourceSeat;
            kind = use.CardKind;
            action = use.Action;
            ordinal = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CurrentTurnCardUseKindsRecordedEvent>().SingleOrDefault(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.ActorSeat == actor && e.OriginFrameId == use.Id && e.CardActionId == action?.ActionId);
        }
        else if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
        {
            action = accepted.Action;
            actor = action.ActorSeat;
            kind = action.EffectiveKind;
            ordinal = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CurrentTurnCardUseKindsRecordedEvent>().SingleOrDefault(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.ActorSeat == actor && e.CardActionId == action.ActionId);
        }
        else
            return;
        var length = ZhuJunCardNameLength(kind);
        if (ordinal?.ActualTurnActorUseOrdinal != length || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive)
            return;
        var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[actor])?.GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted) ?? [];
        foreach (var binding in bindings.Where(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveCuipoActualTurnUse)))
        {
            var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, actor, binding.SkillInstanceId);
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().Any(e => e.OriginFrameId == ordinal.OriginFrameId && e.ActionId == action?.ActionId && e.Source == source))
                continue;
            var qualified = new ZhuJunCuipoQualifiedEvent(ordinal.OriginFrameId, action?.ActionId, source, binding.Program.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, kind, length, length, IsZhuJunDamageCard(kind));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(qualified);
            if (qualified.Damage)
            {
                if (use is null)
                    throw new InvalidOperationException("Cuipo damage must be issued to the exact native use.");
                var grant = new ZhuJunCuipoDamageReceipt(use.Id, action?.ActionId, source, qualified.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, kind, length, length, action?.ProviderSeat ?? actor);
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhuJunCuipoDamage = new() { Grants = (u.ZhuJunCuipoDamage?.Grants ?? []).Append(grant).ToArray() } });
            }
        }
    }

    internal ZhuJunCuipoQualifiedEvent? ZhuJunQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } card || card.ActorSeat != candidate.OwnerSeat || context.OwnerSeat != candidate.OwnerSeat)
            return null;
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId);
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().SingleOrDefault(e => !e.Damage && e.Source == source && e.GameplayHash == candidate.GameplayHash && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.EffectiveKind == card.EffectiveKind && (e.ActionId == card.CardActionId || e.ActionId is null && use is not null && e.OriginFrameId == use.Id && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use)));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27030)]
    internal bool CanOfferZhuJun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveCuipoActualTurnUse) || ZhuJunQualification(candidate, context)is not null;
    SkillProgramStepOutcome global::CardGame.Core.IZhuJunHost.Resolve(ProgramSkillFrame f) => BeginZhuJunCuipo(f);
    internal SkillProgramStepOutcome BeginZhuJunCuipo(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ZhuJunCuipoDraw is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context)
            throw new InvalidOperationException("Cuipo requires its exact mandatory committed instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority);
        var q = ZhuJunQualification(candidate, context) ?? throw new InvalidOperationException("Cuipo lost its declaration-qualified actual use.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoDrawIssuedEvent>().Any(e => e.Source == q.Source && e.OriginFrameId == q.OriginFrameId && e.ActionId == q.ActionId))
            throw new InvalidOperationException("Cuipo cannot draw twice for one accepted use.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhuJunCuipoDraw = new(q, context.ParentFrameId, f.InstructionIndex, before, before, 0), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(ZhuJunDrawReason)).Count;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhuJunCuipoDraw = f.ZhuJunCuipoDraw!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuJunCuipoDrawIssuedEvent(f.Id, context.ParentFrameId, q.OriginFrameId, q.ActionId, q.Source, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinueZhuJunCuipo(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueZhuJunCuipo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhuJunCuipoDraw: { } r } f || f.Id != id)
            return;
        AssertZhuJunCuipo(f);
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuJunCuipoDrawCompletedEvent(id, r.Qualification.OriginFrameId, r.Qualification.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { ZhuJunCuipoDraw = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27030)]
    internal bool ResumeZhuJunCuipo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhuJunCuipoDraw: not null } f || f.Id != id)
            return false;
        ContinueZhuJunCuipo(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27030)]
    internal bool ReturnZhuJunMovement(ProgramSkillFrame f)
    {
        if (f.ZhuJunCuipoDraw is null)
            return false;
        ContinueZhuJunCuipo(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27030)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27030)]
    internal bool ContinuePaidZhuJun(ProgramSkillFrame f)
    {
        if (f.ZhuJunCuipoDraw is null)
            return false;
        AssertZhuJunCuipo(f);
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 160)]
    internal int ZhuJunCuipoDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ZhuJunCuipoDamage: { } state } use)
            return 0;
        if (state.Grants.Count == 0 || state.Grants.Select(r => r.Source).Distinct().Count() != state.Grants.Count)
            throw new InvalidOperationException("Cuipo damage requires its frozen distinct active source instances.");
        var actualAction = use.Action;
        foreach (var r in state.Grants)
        {
            if (r.UseId != use.Id || r.EffectiveKind != use.CardKind || attack.EffectiveCardKind != r.EffectiveKind || !IsZhuJunDamageCard(r.EffectiveKind) || r.Ordinal != r.NameLength || r.NameLength != ZhuJunCardNameLength(r.EffectiveKind) || (actualAction is null ? use.SourceSeat != r.Source.OwnerSeat || r.ActionId is not null || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) : !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, r.Source.OwnerSeat, r.ProviderSeat) || r.ActionId is { } actionId && actualAction.ActionId != actionId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().Count(e => e.Damage && e.OriginFrameId == use.Id && e.ActionId == r.ActionId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.EffectiveKind == r.EffectiveKind && e.Ordinal == r.Ordinal && e.NameLength == r.NameLength) != 1)
                throw new InvalidOperationException("Cuipo damage lost its exact issued native card and original accepted actor.");
        }

        return state.Grants.Count;
    }
}
