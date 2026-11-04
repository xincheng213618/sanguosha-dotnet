#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageColorDuelProgramsRuntime
{
    internal bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && ownerSeat != actorSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, CardKind.Duel, CardActionType.Use) && _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actorSeat, CardKind.Duel, [])) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actorSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[ownerSeat], CardKind.Duel, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, ownerSeat, CardKind.Duel);
    internal SkillProgramStepOutcome BeginSelectedActorDuel(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (_coreOrderedSelectedActorDuelsOperations.IsOrderedSelectedActorDuelProgram(f))
            return _coreOrderedSelectedActorDuelsOperations.BeginOrderedSelectedActorDuel(f);
        var forward = plan.Instructions.Count == 1 && OrderedSelectedActorDuelContract.IsOwnerToSelected(plan.Instructions[0]);
        if (f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || plan.Instructions.Count != 1 || plan.Instructions[0].Op != SkillProgramEffectOp.UseSelectedActorDuel || plan.Activation is not { MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, UsesPerTurn: null } activation || (forward ? activation.UsesPerPhaseExpression != SkillProgramPhaseUsageExpression.CurrentRound || activation.UsesPerPhase is not null || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 : activation.UsesPerPhase != 2) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Selected actor Duel lost its exact owning zero-card Play activation.");
        var target = forward ? actor : f.OwnerSeat;
        if (forward)
            actor = f.OwnerSeat;
        if (!CanIssueSelectedActorDuel(target, actor))
            return SkillProgramStepOutcome.Continue;
        var origin = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, actor, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            ParticipantMode = forward ? SelectedActorDuelParticipantMode.OwnerToSelectedTarget : null,
            InitialTargetSeat = forward ? target : null
        };
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: origin);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsSelectedActorDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { SelectedActorDuelOrigin: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    [NativeUseReturn(200)]
    internal void ReturnSelectedActorDuel(CardUseFrame use)
    {
        if (use.SelectedActorDuelOrigin is not { AttackStarted: false } origin || origin.MouDiaoChanRing is not null)
            return;
        if (origin.OrderedPairPayment is not null)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame orderedParent)
                throw new InvalidOperationException("Ordered Duel lost its original paid parent.");
            _coreOrderedSelectedActorDuelsOperations.CompleteOrderedSelectedActorDuel(orderedParent, origin, use.Id);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId || f.InstructionIndex != origin.InstructionIndex || f.OwnerSeat != origin.OwnerSeat || f.SkillId != origin.SkillId || f.ActivationId != origin.ActivationId || f.SkillInstanceId != origin.SkillInstanceId || f.GameplayHash != origin.GameplayHash)
            throw new InvalidOperationException("Selected actor Duel lost its typed Program return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal SkillProgramStepOutcome BeginDamageAppearanceDraw(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.DamageAppearanceReceipt is not null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || window.ParentFrameId != context.DamageFrameId || window.TargetSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Damage appearance Draw requires the exact applied damage target window.");
        var attempt = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
        if (!attempt.DamageWasApplied || attempt.DamageAmount <= 0 || attempt.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Damage appearance Draw requires positive actual applied damage.");
        var action = _coreCardUseLifecycleOperations.LifecycleCardUse(attempt.ResolutionId)?.Action;
        var hasCard = action is not null || attempt.EffectiveCardKind is not null || attempt.Card is not null;
        var red = hasCard && (action is not null ? action.EffectiveIsRed ?? (action.EffectiveSuit is Suit.Heart or Suit.Diamond) : attempt.Card?.Suit is Suit.Heart or Suit.Diamond);
        var source = attempt.IsSourceLess || attempt.IsDelayedJudgmentDamage ? (int? )null : attempt.SourceSeat;
        var recipient = hasCard ? red ? source : f.OwnerSeat : null;
        var draw = recipient is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageAppearanceReceipt = new(f.InstructionIndex, window.Id, window.ParentFrameId, f.OwnerSeat, source, action?.EffectiveKind ?? attempt.EffectiveCardKind ?? attempt.Card?.Kind, hasCard, red, recipient, draw) });
        if (draw)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, recipient!.Value, 1, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
        return SkillProgramStepOutcome.Continue;
    }

    SkillProgramStepOutcome IDamageColorDuelProgramHost.DrawByDamageCardColor(ProgramSkillFrame frame) => BeginDamageAppearanceDraw(frame);
    SkillProgramStepOutcome IDamageColorDuelProgramHost.UseSelectedActorDuel(ProgramSkillFrame frame) => BeginSelectedActorDuel(frame);
}
