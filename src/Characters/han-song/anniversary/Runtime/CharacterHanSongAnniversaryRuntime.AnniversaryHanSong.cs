#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanSongAnniversaryRuntime
{
    internal const string HanSongDiscardReason = "anniversary.shuaiyan.discard", HanSongDrawReason = "anniversary.shuaiyan.draw";
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryHanSongHost.Resolve(ProgramSkillFrame frame) => BeginAnniversaryHanSong(frame);
    internal static CardConversionSource HanSongSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginAnniversaryHanSong(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var t = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        if (f.AnniversaryHanSong is not null || f.InstructionIndex != 1 || f.WindowContext is not { } c || !ExactHanSongParent(f) || !CanOfferAnniversaryHanSong(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), t, c))
            throw new InvalidOperationException("Shuaiyan lost its native phase subject or equal-hand opportunity.");
        var subject = c.SourceSeat!.Value;
        var count = subject == f.OwnerSeat ? _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count) : 1;
        var r = new AnniversaryHanSongReceipt(HanSongSource(f), f.GameplayHash, c.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, subject, count, subject == f.OwnerSeat ? AnniversaryHanSongStage.Drawing : AnniversaryHanSongStage.BenefitChoice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, subject, count));
        if (subject == f.OwnerSeat)
            IssueHanSongDraw(f);
        else
            PublishHanSong(f);
        return subject == f.OwnerSeat ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<(Card Card, CardLocation From, int Slot, bool Hidden)> HanSongDiscardOptions(ProgramSkillFrame f)
    {
        var subject = f.AnniversaryHanSong!.SubjectSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[subject].IsAlive)
            yield break;
        foreach (var zone in new[]
        {
            CardZoneKind.Hand,
            CardZoneKind.Equipment
        }

        )
            foreach (var item in _cardZonesOperations._cardZones.CardsAt(new(zone, subject)).Select((card, index) => (Card: card, From: new CardLocation(zone, subject), Slot: index, Hidden: zone == CardZoneKind.Hand)))
                if (!_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, item.Card, item.From, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, item.Card, item.From, OwnedCardMoveIntent.Discard))
                    yield return item;
    }

    internal PromptChoice HanSongChoice(ProgramSkillFrame f, string verb, string suffix, string label, IReadOnlyList<int>? cards = null, IReadOnlyDictionary<string, string>? extra = null)
    {
        var p = new Dictionary<string, string>
        {
            {
                "program-action",
                "anniversary-han-song"
            },
            {
                "verb",
                verb
            },
            {
                "frame-id",
                f.Id.ToString(CultureInfo.InvariantCulture)
            }
        };
        if (extra is not null)
            foreach (var pair in extra)
                p[pair.Key] = pair.Value;
        return new(new($"han-song.{(f.Id)}.{(verb)}.{(suffix)}"), label, cards ?? [], [], p);
    }

    internal IReadOnlyList<PromptChoice> HanSongChoices(ProgramSkillFrame f)
    {
        if (f.AnniversaryHanSong is not { } r)
            return[];
        if (r.Stage == AnniversaryHanSongStage.BenefitChoice)
            return HanSongDiscardOptions(f).Any() ? [HanSongChoice(f, "draw", "one", "摸一张牌"), HanSongChoice(f, "discard", "one", $"弃置{(_coreMatchParticipantsPlayersAccess._players[r.SubjectSeat].Name)}的一张牌")] : [HanSongChoice(f, "draw", "one", "摸一张牌")];
        if (r.Stage != AnniversaryHanSongStage.CardChoice)
            return[];
        return Array.AsReadOnly(HanSongDiscardOptions(f).Select(m => HanSongChoice(f, "card", $"{(m.From.Zone)}.{(m.Slot)}", m.Hidden ? $"第{(m.Slot + 1)}张手牌" : global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(m.Card), m.Hidden ? [] : [m.Card.Id], new Dictionary<string, string> { { "source-zone", m.From.Zone.ToString() }, { "slot-index", m.Slot.ToString(CultureInfo.InvariantCulture) }, { "card-owner-seat", r.SubjectSeat.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    }

    internal void PublishHanSong(ProgramSkillFrame f)
    {
        var choices = HanSongChoices(f);
        if (choices.Count == 0)
        {
            FinishHanSong(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryHanSong!.Stage == AnniversaryHanSongStage.BenefitChoice ? "率言：摸一张牌，或弃置该角色一张牌。" : "率言：选择一张可弃置的牌；暗手牌只显示牌位。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHanSongShuaiyan)]
    internal PromptChoice HanSongAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer("anniversary-han-song")]
    internal void AnswerHanSong(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHanSong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ValidHanSong(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HanSongChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Shuaiyan's chooser, original phase subject or current owned card changed.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        if (r.Stage == AnniversaryHanSongStage.BenefitChoice)
        {
            if (choice.Parameters["verb"] == "draw")
            {
                IssueHanSongDraw(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = r with { Stage = AnniversaryHanSongStage.CardChoice } });
            PublishHanSong(f);
            return;
        }

        var selected = HanSongDiscardOptions(f).Single(m => m.From.Zone.ToString() == choice.Parameters["source-zone"] && m.Slot.ToString(CultureInfo.InvariantCulture) == choice.Parameters["slot-index"]);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = r with { Stage = AnniversaryHanSongStage.DiscardChildren, DiscardId = selected.Card.Id, DiscardFrom = selected.From, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongBenefitIssuedEvent(f.Id, false, 1));
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [selected.Card.Id], new(HanSongDiscardReason), (_, records) =>
        {
            var paid = records.Single();
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryHanSong = active.AnniversaryHanSong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DiscardSequence = paid.Sequence } });
            _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(paid, f.OwnerSeat, f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongDiscardPaidEvent(f.Id, r.SubjectSeat, paid.Sequence));
        });
        ContinueHanSong(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void IssueHanSongDraw(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHanSong!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = r with { Stage = AnniversaryHanSongStage.Drawing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongBenefitIssuedEvent(f.Id, true, r.DrawCount));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.DrawCount, new(HanSongDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryHanSongShuaiyan)]
    internal void ReturnHanSongDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryHanSong;
        if (r is null || r.Stage != AnniversaryHanSongStage.Drawing || !ValidHanSong(f) || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.DrawCount || outcome.SequenceBefore != r.Before || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Shuaiyan lost its original once-paid native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = r with { Stage = AnniversaryHanSongStage.DrawChildren, Draw = outcome, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongDrawReturnedEvent(f.Id, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueHanSong(f.Id);
    }

    internal void ContinueHanSong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHanSong: { } r } f || f.Id != id)
            return;
        AssertHanSong(f);
        if (r.Stage is AnniversaryHanSongStage.BenefitChoice or AnniversaryHanSongStage.CardChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishHanSong(f);
            return;
        }

        if (r.Stage is not (AnniversaryHanSongStage.DrawChildren or AnniversaryHanSongStage.DiscardChildren))
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishHanSong(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishHanSong(ProgramSkillFrame f)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHanSong = f.AnniversaryHanSong!with { Stage = AnniversaryHanSongStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHanSongCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 45900)]
    internal bool ResumeHanSong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHanSong: { Stage: not AnniversaryHanSongStage.Complete } } f || f.Id != id)
            return false;
        ContinueHanSong(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45900)]
    internal bool ReturnHanSongMovement(ProgramSkillFrame f) => f.AnniversaryHanSong is not null && ResumeHanSong(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45900)]
    internal bool ContinuePaidHanSong(ProgramSkillFrame f) => f.AnniversaryHanSong is { Stage: AnniversaryHanSongStage.Drawing or AnniversaryHanSongStage.DrawChildren or AnniversaryHanSongStage.DiscardChildren } && ValidHanSong(f);
}
