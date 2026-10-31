#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunYuMouRuntime
{
    internal IReadOnlyList<MouXyPlan> MouXyPlans(ProgramSkillFrame f)
    {
        if (f.MouXunYuAnniversary is not { Actor: >= 0 } r || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var plans = new List<MouXyPlan>();
        foreach (var o in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, physicalSuit: Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []))
        {
            if (!CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(o.EffectiveCardKind))
                continue;
            var normal = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(o.EffectiveCardKind, r.Actor, o.TargetSeats);
            plans.Add(new(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, false) { Targets = normal });
            if (r.ActualDraw <= 0)
                continue;
            if (o.EffectiveCardKind == CardKind.BorrowedSword)
            {
                var holders = normal.Where((_, i) => i % 2 == 0).ToArray();
                foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !holders.Contains(p.Seat) && p.Seat != actor.Seat && _coreNativeBorrowedSwordOperations.GetWeapon(p)is not null && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, o.EffectiveCardKind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, o.EffectiveCardKind, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, Suit.None)))
                    foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(holder, v)))
                        plans.Add(new(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, true) { Targets = normal.Concat([holder.Seat, victim.Seat]).ToArray() });
                continue;
            }

            var action = new LegalAction(o.ActionKind, 0, normal.FirstOrDefault(-1), o.Description, o.EffectiveCardKind, o.TargetCardId, normal);
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => !normal.Contains(p.Seat) && _coreNextActualUseTargetAdjustmentsOperations.CanBeExtraTargetAdjustmentTarget(actor, p, action, o.EffectiveCardKind, Suit.None)))
                plans.Add(new(o.EffectiveCardKind, o.ActionKind, o.TargetCardId, o.RequiredCardKind, true) { Targets = normal.Append(target.Seat).ToArray() });
        }

        return Array.AsReadOnly(plans.DistinctBy(p => (p.Kind, p.ActionKind, p.TargetCardId, p.Required, p.Extra, Seats: string.Join(',', p.Targets))).ToArray());
    }

    internal void IssueMouXyTrick(ProgramSkillFrame f)
    {
        if (!MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.IssuingUse, Plan: { } plan } r || !MouXyPlans(f).Any(p => MouXySamePlan(p, plan)) || r.Repeat && r.ActualDiscard <= 0 || plan.Extra && r.ActualDraw <= 0)
            throw new InvalidOperationException("Shimou requires its own adjusted actor, exact legal zero-material plan and actual Draw/Discard benefit.");
        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, 0, f.OwnerSeat, r.Actor, plan.Targets.FirstOrDefault(r.Actor), f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, plan.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, plan.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, r.Actor, plan.Targets, plan.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        MouXyAnniversary.Bound(f, id, use.Action!.ActionId);
        if (r.Repeat)
        {
            var effect = new ExtraCardEffectState
            {
                Source = r.Source,
                ProgramFrameId = f.Id,
                ActionId = use.Action.ActionId,
                PaidCardId = 0,
                AutomaticGameplayHash = f.GameplayHash,
                Trick = new(0, plan.ActionKind, plan.TargetCardId, plan.Required)
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { ExtraCardEffect = effect });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AutomaticExtraCardEffectIssuedEvent(f.Id, r.Source, id, use.Action.ActionId, f.GameplayHash));
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, r.Actor, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, plan.ActionKind, plan.TargetCardId, plan.Required, plan.Kind);
    }

    internal static bool MouXySamePlan(MouXyPlan a, MouXyPlan b) => (a.Kind, a.ActionKind, a.TargetCardId, a.Required, a.Extra) == (b.Kind, b.ActionKind, b.TargetCardId, b.Required, b.Extra) && a.Targets.SequenceEqual(b.Targets);
    internal bool IsMouXyVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { } o || o.SkillId != MouXunYuAnniversaryRuntime.Shimou || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.ParentProgramFrameId)is not { MouXunYuAnniversary: { Plan: { } plan } r } f || !MouXyAnniversary.Valid(f) || r.Stage is not (MouXyStage.IssuingUse or MouXyStage.NativeUse) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0 } a || use.SourceSeat != r.Actor || a.ActorSeat != r.Actor || a.ProviderSeat != r.Actor || a.ActionId != o.CardActionId || use.CardKind != plan.Kind || a.EffectiveKind != plan.Kind || o.InitialActorSeat != r.Actor || o.OwnerSeat != f.OwnerSeat || o.BindingId != f.ActivationId || o.InstructionIndex != f.InstructionIndex || o.SkillInstanceId != f.SkillInstanceId || o.GameplayHash != f.GameplayHash || o.TurnNumber != r.Turn || o.LifecycleFrameId != 0 || r.UseId != 0 && (r.UseId != use.Id || r.ActionId != a.ActionId) || !a.TargetSeats.SequenceEqual(use.TargetSeats) || !use.TargetSeats.SequenceEqual(plan.Targets) || a.EffectiveSuit is not (null or Suit.None) || a.EffectiveRank is not (null or 0) || a.EffectiveIsRed == true)
            return false;
        return (!r.Repeat || r.ActualDiscard > 0) && (!plan.Extra || r.ActualDraw > 0);
    }

    internal bool ReturnMouXyVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: MouXunYuAnniversaryRuntime.Shimou } o)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ParentProgramFrameId);
        var r = f.MouXunYuAnniversary!;
        if (!MouXyAnniversary.Valid(f) || r.Stage != MouXyStage.NativeUse || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == r.Kind) != 1)
            throw new InvalidOperationException("Shimou must return the same full native zero-material Use once.");
        MouXyAnniversary.Returned(f.Id, use.Id, r.ActionId, use.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 37200)]
    internal bool MouXyAttackReturned()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXunYuAnniversary: { Stage: MouXyStage.ReturnedUse } r } f)
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || !MouXyAnniversary.Valid(f))
            throw new InvalidOperationException("Shimou cannot resume before its real attack tail.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
