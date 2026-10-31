#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal void PayMouLuXunHold(ProgramSkillFrame f)
    {
        var r = f.MouLuXun!;
        if (!ValidMouLuXun(f) || r.Stage != MouLuXunStage.HoldChoice || r.Selected.Count is < 1 || r.Selected.Count > r.Limit || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: var owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || owner != f.OwnerSeat))
            throw new InvalidOperationException("Qianxun can only hold its accepted bounded actual HE subset.");
        var location = new CardLocation(CardZoneKind.PrivateTurnHold, f.OwnerSeat, privateTurnHold: new(f.Id, f.SkillId, f.SkillInstanceId, r.GrantSourceId, r.ActualTurn));
        _cardZonesOperations._cardZones.EnsurePrivateTurnHold(location);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.HoldChildren, HoldLocation = location, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, location, new("skill-program.mou-mobile:qianxun.hold"), (batch, movements) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouLuXun = current.MouLuXun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            for (var i = 0; i < movements.Count; i++)
            {
                var move = movements[i];
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunEntityMovedEvent(f.Id, i, move.CardId, move.From, move.To, move.Sequence));
            }
        }, disclosure: CardMovementDisclosure.FaceDown);
        ContinueMouLuXun(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 32800)]
    internal bool ResumeMouLuXun(long id) => ContinueMouLuXun(id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 32800)]
    internal bool ResumeMouLuXunMovement(ProgramSkillFrame frame) => ContinueMouLuXun(frame.Id);
    internal bool ContinueMouLuXun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuXun: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (r.Stage is MouLuXunStage.HoldChoice or MouLuXunStage.NameChoice or MouLuXunStage.UseChoice or MouLuXunStage.IssuingUse or MouLuXunStage.NativeUse or MouLuXunStage.Distribution)
            return false;
        if (DrainMouLuXun(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouLuXun!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage is MouLuXunStage.HoldChildren or MouLuXunStage.NativeReturned or MouLuXunStage.ReturnChildren)
        {
            FinishMouLuXun(f);
            return true;
        }

        if (r.Stage == MouLuXunStage.GiftChildren)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r = r with { Cursor = r.Cursor + 1 } });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            ReturnMouLuXunUnassigned(f);
            return true;
        }

        if (r.Cursor >= r.Viewed.Count)
        {
            FinishMouLuXun(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.Distribution } });
        PublishMouLuXun(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32800)]
    internal bool ContinueIssuedMouLuXun(ProgramSkillFrame f) => f.MouLuXun is { Paid: true } && ValidMouLuXun(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 32800)]
    internal bool IsMouLuXunMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.MouLuXun is { Stage: MouLuXunStage.HoldChildren or MouLuXunStage.ViewChildren or MouLuXunStage.GiftChildren or MouLuXunStage.ReturnChildren } r && effect?.Op == r.Operation && ValidMouLuXun(f) && pending == new ProgramMovementContinuation(f.OwnerSeat, 0, null) && r.Before <= r.After && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32800)]
    internal void AssertMouLuXun(ProgramSkillFrame f)
    {
        if (f.MouLuXun is not { } r)
            return;
        if (!ValidMouLuXun(f) || !Enum.IsDefined(r.Stage) || r.Limit is < 0 or > 5 || r.Cursor < 0 || r.Cursor > r.Viewed.Count || r.Selected.Distinct().Count() != r.Selected.Count || r.Viewed.Distinct().Count() != r.Viewed.Count || r.Viewed.Count > r.Limit || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Lu Xun lost its issued source, bounded original physical entities or actual-turn cursor.");
        if (r.Operation == SkillProgramEffectOp.RecordMouLuXunQianxun && (r.Kind is null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunNameRecordedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Kind == r.Kind && e.WindowId == r.WindowId && e.ActualTurn == r.ActualTurn) != 1))
            throw new InvalidOperationException("Qianxun lost its exact real trick-effect name registration.");
        if (r.Operation == SkillProgramEffectOp.UseMouLuXunRecordedTrick && r.Paid && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunNameRemovedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Kind == r.Kind && e.ActualTurn == r.ActualTurn) != 1)
            throw new InvalidOperationException("Qianxun cannot re-use or refund a paid recorded name.");
        if (r.Viewed.Select((card, index) => (card, index)).Any(pair => _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunTopViewedEvent>().Count(e => e.FrameId == f.Id && e.Index == pair.index && e.CardId == pair.card) != 1))
            throw new InvalidOperationException("Lianying lost an exact privately reserved top entity.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.RecordMouLuXunQianxun, SkillProgramEffectOp.UseMouLuXunRecordedTrick, SkillProgramEffectOp.DistributeMouLuXunLianying)]
    internal PromptChoice ChooseMouLuXunAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        var r = f.MouLuXun!;
        if (r.Stage == MouLuXunStage.HoldChoice)
        {
            var dangerous = r.Kind is CardKind.Snatch or CardKind.Dismantlement or CardKind.FireAttack;
            return dangerous ? prompt.Choices.FirstOrDefault(c => c.Parameters["verb"] == "toggle" && !r.Selected.Contains(c.Cards[0])) ?? prompt.Choices.Single(c => c.Parameters["verb"] == "confirm") : prompt.Choices.Single(c => c.Parameters["verb"] == "confirm");
        }

        if (r.Stage == MouLuXunStage.Distribution)
            return prompt.Choices.Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])).OrderBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets[0]]).Count).FirstOrDefault() ?? prompt.Choices[0];
        if (r.Stage == MouLuXunStage.NameChoice)
            return prompt.Choices[0];
        var options = MouLuXunUseOptions(f);
        return prompt.Choices.FirstOrDefault(c => c.Parameters["verb"] == "use" && (options[int.Parse(c.Parameters["value"])].ActionKind is LegalActionKind.DrawTwo or LegalActionKind.FiveGrains or LegalActionKind.PeachGarden || c.Targets.Any(target => !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])))) ?? prompt.Choices.Last();
    }
}
