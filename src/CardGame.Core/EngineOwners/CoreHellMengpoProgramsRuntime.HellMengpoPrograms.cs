#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHellMengpoProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IHellMengpoProgramHost.Forget(ProgramSkillFrame frame) => BeginHellWanghun(frame);
    SkillProgramStepOutcome global::CardGame.Core.IHellMengpoProgramHost.BanCategory(ProgramSkillFrame frame) => IssueHellWangshiBan(frame);
    SkillProgramStepOutcome global::CardGame.Core.IHellMengpoProgramHost.Restore(ProgramSkillFrame frame) => _coreHellSoulCardsOperations.BeginHellSoulRestoration(frame);
    internal bool HellEnemies(int first, int second) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(first) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(second) && _coreMatchParticipantsPlayersAccess._players[first].TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[second].TeamId is { } other && team != other;
    internal IReadOnlyList<SkillGrant> HellWanghunEligibleGrants(CharacterState player) => Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(player).ActiveGrants.Where(grant => grant.IsEnabled && !grant.IsModeRule && grant.SkillId is not ("hell:player-tier" or "hell:boss-tier" or "hell:return-soul-resolution") && !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal) && grant.LordProjection is null && grant.GeneralLibraryProjection is null && !_contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Awakening)).OrderBy(grant => grant.SkillId, StringComparer.Ordinal).ThenBy(grant => grant.GrantId, StringComparer.Ordinal).ToArray());
    internal SkillProgramStepOutcome BeginHellWanghun(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "hell:wanghun" || frame.TriggerId != "forget-two-enemies" || frame.InstructionIndex != 1 || frame.HellWanghun is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault()is not { } death || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramDeathTriggerWindowFrame window || death.VictimSeat != frame.OwnerSeat || context.ParentFrameId != window.Id || window.DeathFrameId != death.Id || window.OwnerSeat != frame.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<HellWanghunIssuedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Wanghun requires its exact real Mengpo death and once-issued instruction.");
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && HellEnemies(frame.OwnerSeat, p.Seat)).Select(p => p.Seat).ToList();
        _randomOperations._random.Shuffle(targets);
        var picked = targets.Take(2).Select(seat =>
        {
            var grants = HellWanghunEligibleGrants(_coreMatchParticipantsPlayersAccess._players[seat]);
            var ids = grants.Select(g => g.SkillId).Distinct(StringComparer.Ordinal).ToArray();
            var skill = ids.Length == 0 ? null : ids[_randomOperations._random.Next(ids.Length)];
            return (Seat: seat, Skill: skill, Grants: grants.Where(g => g.SkillId == skill).ToArray());
        }).ToArray();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        var skills = picked.Count(p => p.Skill is not null);
        var removed = picked.Sum(p => p.Grants.Length);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HellWanghun = new(frame.InstructionIndex, death.Id, window.Id, HellWanghunStage.LossChildren, skills, removed) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellWanghunIssuedEvent(frame.Id, death.Id, frame.OwnerSeat, skills, removed));
        foreach (var choice in picked)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[choice.Seat];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellWanghunTargetChosenEvent(frame.Id, frame.OwnerSeat, target.Seat, choice.Skill));
            foreach (var grant in choice.Grants)
            {
                if (!target.SkillGrants.RemoveGrant(grant.GrantId))
                    throw new InvalidOperationException("Wanghun changed a frozen original grant before loss.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellWanghunGrantLostEvent(frame.Id, frame.OwnerSeat, target.Seat, target.General.Id, target.SecondaryGeneral?.Id, grant));
            }
        }

        ContinueHellWanghun(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40051)]
    internal bool ResumeHellWanghun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HellWanghun: not null } frame || frame.Id != id)
            return false;
        ContinueHellWanghun(frame);
        return true;
    }

    internal void ContinueHellWanghun(ProgramSkillFrame frame)
    {
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = frame.HellWanghun!;
        if (receipt.Stage == HellWanghunStage.LossChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HellWanghun = receipt with { Stage = HellWanghunStage.InjectionChildren } });
            var oldOrder = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Select(c => c.Id).ToList();
            for (var ordinal = 0; ordinal < 2; ordinal++)
            {
                var id = _cardZonesOperations._cardZones.CreateDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1;
                var card = new Card(id, CardKind.ReturnSoul, Suit.None, 0);
                _cardZonesOperations._cardZones.AddGeneratedCard(card);
                _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(id);
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.DrawPile, new("activity.hell.return-soul-injected"));
                oldOrder.Insert(_randomOperations._random.Next(oldOrder.Count + 1), id);
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HellWanghun = frame.HellWanghun!with { InjectedCards = ordinal + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellReturnSoulInjectedEvent(frame.Id, frame.OwnerSeat, id));
            }

            var currentOrder = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Select(c => c.Id).ToArray();
            _cardZonesOperations._cardZones.ReorderDrawPileTop(currentOrder, oldOrder, []);
            ContinueHellWanghun(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellWanghunCompletedEvent(frame.Id, receipt.LostSkillCount, receipt.InjectedCards));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HellWanghun = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40051)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40051)]
    internal bool CanContinueHellWanghun(ProgramSkillFrame frame)
    {
        if (frame.HellWanghun is null)
            return false;
        return true;
    }

    internal SkillProgramStepOutcome IssueHellWangshiBan(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "hell:wangshi" || frame.TriggerId != "forbid-enemy-category" || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } seat } context || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault()is not { } parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || parent.OwnerSeat != seat || seat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != seat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Wangshi requires the exact real turn-start owner.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !HellEnemies(frame.OwnerSeat, seat))
            return SkillProgramStepOutcome.Continue;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<HellWangshiCategoryBanEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Wangshi cannot choose a different category after its actual issuance.");
        var category = new[]
        {
            "basic",
            "trick",
            "equipment"
        }[_randomOperations._random.Next(3)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellWangshiCategoryBanEvent(frame.Id, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, seat, category));
        var categoryName = category switch
        {
            "basic" => "基本牌",
            "trick" => "锦囊牌",
            _ => "装备牌"
        };
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].General.Name)}发动【往事】：{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}本回合不能使用或打出{(categoryName)}。", frame.OwnerSeat, seat);
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsHellWangshiActionForbidden(int actorSeat, CardKind kind, CardActionType type)
    {
        if (type is not (CardActionType.Use or CardActionType.Response) || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BanHellWangshiCategory))
            return false;
        var category = CardUseCategoryCatalog.Get(kind) switch
        {
            CardUseCategories.Basic => "basic",
            CardUseCategories.Equipment => "equipment",
            _ => "trick"
        };
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HellWangshiCategoryBanEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == actorSeat && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Category == category && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(end => end.TurnNumber == e.TurnNumber && end.ActorSeat == e.TurnOwnerSeat));
    }

    internal bool HasHellShiyouCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.HellShiyouIntercept);

    internal CardLocation? HellShiyouOriginalSource(CardTransfer transfer, CardMoveReason reason)
    {
        if (transfer.To != CardLocation.DiscardPile)
            return null;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(transfer.From))
            return global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason) ? transfer.From : null;
        if (transfer.From != CardLocation.Processing)
            return null;
        var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == transfer.CardId);
        if (entry is null || entry.To != CardLocation.Processing || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(entry.From) || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(entry.Reason))
            return null;
        return reason == entry.Reason || reason.Value == entry.Reason.Value + "-finished" || reason.Value == entry.Reason.Value + "-discard" ? entry.From : null;
    }

    internal HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(IReadOnlyList<CardTransfer> input, CardMoveReason reason)
    {
        var transfers = input.ToArray();
        if (!HasHellShiyouCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase)
            return new()
            {
                Transfers = Array.AsReadOnly(transfers)
            };
        var redirects = new List<HellShiyouRedirectReceipt>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !p.IsHuman && p.Seat != phase.ActorSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, p.Seat)))
        {
            var policy = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.HellShiyouIntercept).FirstOrDefault();
            if (policy.Source is null)
                continue;
            var eligible = transfers.Select((transfer, index) => (Transfer: transfer, Index: index, Original: HellShiyouOriginalSource(transfer, reason))).Where(p => p.Original is { OwnerSeat: var source } && source == phase.ActorSeat).Select(p => (p.Transfer, p.Index, Original: p.Original!.Value, Card: _coreCardAttackStateOperations.GetAttackCard(p.Transfer.CardId))).GroupBy(p => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[phase.ActorSeat], p.Card)).Select(g => g.OrderByDescending(p => CardCatalog.Get(p.Card.Kind).HandKeepValue).ThenBy(p => p.Card.Id).First()).OrderBy(p => p.Index).ToArray();
            foreach (var item in eligible)
            {
                var destination = CardLocation.Hand(owner.Seat);
                redirects.Add(new(phase, new(policy.Source.SkillId, policy.Policy.Id, owner.Seat, policy.Source.SkillInstanceId), policy.Source.Program.GameplayHash, item.Card.Id, item.Card.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[phase.ActorSeat], item.Card), item.Original, item.Transfer.From, destination, reason));
                transfers[item.Index] = item.Transfer with
                {
                    To = destination
                };
            }
        }

        return new()
        {
            PlanId = redirects.Count == 0 ? 0 : ++_coreResolutionIdentityBoundaryOperations._resolutionSequence,
            Transfers = Array.AsReadOnly(transfers),
            Redirects = Array.AsReadOnly(redirects.ToArray())
        };
    }

    internal void CompleteHellShiyouDiscardTransfers(HellShiyouTransferPlan plan, IReadOnlyList<CardMovementRecord> movements)
    {
        foreach (var receipt in plan.Redirects)
        {
            var actual = movements.Where(m => m.CardId == receipt.CardId && m.CardKind == receipt.CardKind && m.From == receipt.ActualFrom && m.To == receipt.Destination && m.Reason == receipt.Reason).ToArray();
            if (plan.PlanId <= 0 || actual is not [var move] || receipt.Destination != CardLocation.Hand(receipt.Source.OwnerSeat) || receipt.OriginalSource.OwnerSeat != receipt.Phase.ActorSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<HellShiyouInterceptedEvent>().Any(e => e.PlanId == plan.PlanId && e.Receipt.CardId == receipt.CardId))
                throw new InvalidOperationException("Shiyou lost its exact pre-discard redirection and native movement.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellShiyouInterceptedEvent(plan.PlanId, move.Sequence, receipt));
        }
    }

    internal bool IsHellShiyouRedirectedDiscard(CardMovementRecord move) => _coreProgramEventHistoryOperations.ProgramEventHistory<HellShiyouInterceptedEvent>().Any(e => e.MovementSequence == move.Sequence && e.Receipt.CardId == move.CardId && e.Receipt.CardKind == move.CardKind && e.Receipt.ActualFrom == move.From && e.Receipt.Destination == move.To && e.Receipt.Reason == move.Reason && move.To == CardLocation.Hand(e.Receipt.Source.OwnerSeat) && e.Receipt.OriginalSource.OwnerSeat == e.Receipt.Phase.ActorSeat && e.Receipt.Phase.Kind == ActualDiscardRecoveryPhaseKind.Discard && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(e.Receipt.Phase));
    internal bool IsHellShiyouPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => move.Reason == expectedReason && IsHellShiyouRedirectedDiscard(move) && _coreProgramEventHistoryOperations.ProgramEventHistory<HellShiyouInterceptedEvent>().Any(e => e.MovementSequence == move.Sequence && e.Receipt.CardId == move.CardId && e.Receipt.OriginalSource == expectedSource && e.Receipt.Reason == expectedReason);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 30)]
    internal bool HellWangshiCardUseForbidden(ProgramCardUseQuery query) => IsHellWangshiActionForbidden(query.ActorSeat, query.Kind, query.ActionType);
}
