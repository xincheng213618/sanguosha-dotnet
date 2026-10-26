using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileDuanfaId = "mobile:duanfa", MobileDuanfaDiscard = "skill-program.mobile-duanfa.discard", MobileDuanfaDraw = "skill-program.mobile-duanfa.draw";
    private static CardConversionSource MobileDuanfaSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private SkillGrantPhaseExpiry? MobileDuanfaPhase(int owner) => _phase == TurnPhase.Play && _currentSeat == owner && _turnNumber > 0 && _cardUseDebitPhaseInstanceId > 0
        ? new(_turnNumber, owner, _cardUseDebitPhaseInstanceId) : null;
    private int MobileDuanfaSpent(int owner, SkillGrantPhaseExpiry phase) => ProgramEventHistory<MobileDuanfaPaidEvent>()
        .Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MobileDuanfaId && e.Phase == phase).Sum(e => e.ActualCount);
    private int MobileDuanfaRemaining(int owner) => MobileDuanfaPhase(owner) is { } phase ? Math.Max(0, _players[owner].MaxHp - MobileDuanfaSpent(owner, phase)) : 0;
    private bool MobileDuanfaHasBlackCost(CharacterState owner) => GetHand(owner).Concat(GetEquipment(owner)).Any(c => GetProgramEffectiveSuit(owner, c) is Suit.Spade or Suit.Club &&
        !IsSelfHandCategoryDiscardForbidden(owner.Seat, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) &&
        !IsForeignEquipmentDiscardPrevented(owner.Seat, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 82300, SkillProgramEffectOp.ResolveMobileDuanfa)]
    private bool CanActivateMobileDuanfa(ProgramActivationLegalityContext g) => g.SkillId == MobileDuanfaId && g.Owner.IsAlive && MobileDuanfaRemaining(g.OwnerSeat) > 0 && MobileDuanfaHasBlackCost(g.Owner);
    [ProgramCapability]
    private sealed class MobileZhouFangHost(GameEngine e) : IMobileZhouFangHost
    {
        public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => e.BeginMobileDuanfa(f);
        public void PublishBait(ProgramSkillFrame f, string bind) => e.PublishMobileYoudiBait(f, bind);
    }
    private IReadOnlyList<PromptChoice> MobileDuanfaCosts(ProgramSkillFrame f) => BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat,
        [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1 &&
            GetProgramEffectiveSuit(_players[f.OwnerSeat], GetAdvancedCard(c.Cards.Single())) is Suit.Spade or Suit.Club).ToArray();
    private SkillProgramStepOutcome BeginMobileDuanfa(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileDuanfaId || f.ActivationId != "phase-black-recycle" || f.WindowContext is not null || f.InstructionIndex != 1 || f.MobileDuanfa is not null ||
            f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || MobileDuanfaPhase(f.OwnerSeat) is not { } phase || MobileDuanfaRemaining(f.OwnerSeat) < 1 || MobileDuanfaCosts(f).Count == 0)
            throw new InvalidOperationException("Mobile Duanfa requires its current real Play phase and at least one legal black HE payment.");
        var r = new MobileDuanfaReceipt(MobileDuanfaSource(f), f.GameplayHash, phase, MobileDuanfaStage.Cards);
        ReplaceRuntimeTop(f with { MobileDuanfa = r }); AdvanceEventRulesAndQueueFact(new MobileDuanfaIssuedEvent(f.Id, r.Source, r.Hash, phase));
        ContinueMobileDuanfa(f.Id); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> MobileDuanfaChoices(ProgramSkillFrame f)
    {
        var r = f.MobileDuanfa!; var choices = new List<PromptChoice>();
        void Add(string key, string label, string branch, IReadOnlyList<int> cards) => choices.Add(new(new($"mobile-duanfa.{f.Id}.{key}"), label, cards, [],
            new Dictionary<string, string> { ["program-action"] = "mobile-duanfa", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        foreach (var id in r.Cards) Add("unpick." + id, "取消已选牌", "unpick", [id]);
        if (r.Cards.Count < MobileDuanfaRemaining(f.OwnerSeat)) foreach (var c in MobileDuanfaCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single())))
            Add("card." + c.Cards.Single(), c.Label, "card", c.Cards);
        if (r.Cards.Count > 0 && r.Cards.Count <= MobileDuanfaRemaining(f.OwnerSeat)) Add("pay", "弃置所选黑色牌并摸等量牌", "pay", []);
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }
    private void FinishMobileDuanfa(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f with { MobileDuanfa = null }); AdvanceRuntimeProgram(f.Id); }
    private void ContinueMobileDuanfa(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileDuanfa: { } r } f || f.Id != id) return;
        if (f.NativeDraw is not null) return;
        if (r.Stage == MobileDuanfaStage.Cards)
        {
            if (!_players[f.OwnerSeat].IsAlive || MobileDuanfaPhase(f.OwnerSeat) != r.Phase) { FinishMobileDuanfa(f); return; }
            var choices = MobileDuanfaChoices(f); if (choices.Count == 0) { FinishMobileDuanfa(f); return; }
            var skill = _contentRegistry.GetSkill(f.SkillId);
            _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"断发：选择黑色手牌或装备牌，本阶段尚可弃置{MobileDuanfaRemaining(f.OwnerSeat)}张。",
                choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
            _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState(); return;
        }
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return;
        f = GetActiveProgramFrame(id); r = f.MobileDuanfa!;
        if (!_players[f.OwnerSeat].IsAlive || _winner != Winner.None || r.ActualCount == 0) { FinishMobileDuanfa(f); return; }
        if (r.Stage != MobileDuanfaStage.CostChildren) throw new InvalidOperationException("Duanfa lost its exact paid native draw return.");
        ReplaceRuntimeTop(f = f with { MobileDuanfa = r with { Stage = MobileDuanfaStage.Drawing } });
        BeginOperationProgramDraw(f, f.OwnerSeat, r.ActualCount, new(MobileDuanfaDraw));
    }
    [ProgramAnswer("mobile-duanfa")]
    private void AnswerMobileDuanfa(PromptChoice selected)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Duanfa answer lost its original frame."); var r = f.MobileDuanfa!;
        if (r.Stage != MobileDuanfaStage.Cards || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) ||
            !AssistedChoicesEqual([selected], MobileDuanfaChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duanfa's actual black HE payment or phase allowance changed.");
        ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "card": ReplaceRuntimeTop(f with { MobileDuanfa = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } }); break;
            case "unpick": ReplaceRuntimeTop(f with { MobileDuanfa = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } }); break;
            case "pay":
                var available = MobileDuanfaCosts(f).SelectMany(c => c.Cards).ToHashSet(); var before = _movementSequence;
                var spent = MobileDuanfaSpent(f.OwnerSeat, r.Phase); var limit = _players[f.OwnerSeat].MaxHp;
                if (MobileDuanfaPhase(f.OwnerSeat) != r.Phase || r.Cards.Count < 1 || r.Cards.Count > limit - spent || r.Cards.Any(c => !available.Contains(c)))
                    throw new InvalidOperationException("Duanfa cannot pay beyond its current actual Play-phase maximum-HP allowance.");
                ReplaceRuntimeTop(f = f with { MobileDuanfa = r with { Stage = MobileDuanfaStage.CostChildren, Limit = limit, SpentBefore = spent, Before = before, After = before } });
                var logical = CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, r.Cards, new(MobileDuanfaDiscard), f.Id);
                MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.DiscardPile, new(MobileDuanfaDiscard), (_, moves) =>
                {
                    var current = GetActiveProgramFrame(f.Id); var paid = current.MobileDuanfa! with { After = _movementSequence,
                        ActualCount = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) };
                    ReplaceRuntimeTop(current with { MobileDuanfa = paid });
                    AdvanceEventRulesAndQueueFact(new MobileDuanfaPaidEvent(f.Id, paid.Source, paid.Hash, paid.Phase, paid.Limit, paid.SpentBefore, paid.ActualCount, before, paid.After));
                });
                CompleteNativeLogicalDiscardInvocation(logical); break;
            default: throw new InvalidOperationException("Unknown mobile Duanfa selection branch.");
        }
        ContinueMobileDuanfa(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 82300)]
    private bool ResumeMobileDuanfa(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileDuanfa: not null } f || f.Id != id) return false; ContinueMobileDuanfa(id); return true; }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileDuanfa)]
    private void ReturnMobileDuanfaDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = GetActiveProgramFrame(supplied.Id); var r = f.MobileDuanfa ?? throw new InvalidOperationException("Duanfa draw lost its original payment.");
        if (r.Stage != MobileDuanfaStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.ActualCount || outcome.ActualDrawIds.Count > r.ActualCount)
            throw new InvalidOperationException("Duanfa draw lost its exact equal-cost native request.");
        AdvanceEventRulesAndQueueFact(new MobileDuanfaDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter)); FinishMobileDuanfa(f);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileDuanfa)]
    private PromptChoice SelectMobileDuanfaAi(PendingDecision d, ProgramSkillFrame f) =>
        d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? d.Choices.First(c => c.Parameters["branch"] == "card");
}
