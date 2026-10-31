#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientCategoryMarksRuntime
{
    internal bool TracksRecipientCategoryMarks => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveHandAndGrantCategoryMark);

    internal static RecipientCategoryMarkKind RecipientMarkKind(SkillProgramEffectOp op) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecipientCategoryMarks.RecipientMarkKind(op);
    internal static PlayerMarkerKind RecipientPublicMarker(RecipientCategoryMarkKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecipientCategoryMarks.RecipientPublicMarker(kind);
    internal static CardConversionSource RecipientMarkSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecipientCategoryMarks.RecipientMarkSource(f);
    internal static string RecipientMarkReason(ProgramSkillFrame f, string suffix) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecipientCategoryMarks.RecipientMarkReason(f, suffix);
    internal long RecipientMarkSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static Dictionary<string, string> RecipientMarkParameters(ProgramSkillFrame f, string action) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecipientCategoryMarks.RecipientMarkParameters(f, action);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(CharacterState drawOwner, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        if (!TracksRecipientCategoryMarks)
            return existing;
        var added = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != drawOwner.Seat && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.DrawPhaseEnded).Any(binding => ProgramInstructionResolver.Default.Features(binding.Trigger).HasOperation(SkillProgramEffectOp.GiveHandAndGrantCategoryMark))).SelectMany(p => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(p, SkillProgramTriggerWindow.DrawPhaseEnded, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(p))).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.GiveHandAndGrantCategoryMark));
        return Array.AsReadOnly(existing.Concat(added).Distinct().OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(drawOwner.Seat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.BindingId, StringComparer.Ordinal).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1030)]
    internal bool CanOfferRecipientCategoryMarkProgram(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var e] || !RecipientCategoryMarksComposition.IsOperation(e.Op))
            return true;
        if (context.OwnerSeat != c.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return false;
        if (e.Op == SkillProgramEffectOp.GiveHandAndGrantCategoryMark)
            return context.Window == SkillProgramTriggerWindow.DrawPhaseEnded && context.SourceSeat is { } holder && context.TargetSeat == holder && holder != c.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Count > 0 && !_coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.ContainsKey(new(holder, c.SkillId, e.StateId!));
        if (!_coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.TryGetValue(new(c.OwnerSeat, e.SourceBind!, e.StateId!), out var token) || token.Kind != RecipientMarkKind(e.Op) || token.DerivedSkillId != c.SkillId)
            return false;
        return e.Op switch
        {
            SkillProgramEffectOp.SpendCategoryMarkForDyingRecovery => context.Window == SkillProgramTriggerWindow.DyingEntering && context.TargetSeat == c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp <= 0,
            SkillProgramEffectOp.SpendCategoryMarkForAreaDiscard => context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && context.SourceSeat == c.OwnerSeat && context.TargetSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatAccess._currentSeat == c.OwnerSeat,
            _ => _coreRecipientCategorySlashTargetsOperations.TryGetRecipientCategorySlashUse(c.OwnerSeat, context, out _, out _)};
    }

    internal SkillProgramStepOutcome ExecuteRecipientCategoryMark(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.RecipientCategoryMark is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferRecipientCategoryMarkProgram(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), context) || !_coreRecipientCategoryMarkObserversOperations.ExactRecipientCategoryParent(f))
            throw new InvalidOperationException("A category mark operation lost its exact native opportunity.");
        var gift = e.Op == SkillProgramEffectOp.GiveHandAndGrantCategoryMark;
        var holder = gift ? context.TargetSeat!.Value : f.OwnerSeat;
        var token = gift ? null : _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks[new(holder, e.SourceBind!, e.StateId!)];
        var r = new ProgramRecipientCategoryMarkReceipt
        {
            Source = RecipientMarkSource(f),
            GameplayHash = f.GameplayHash,
            SourceSkillId = gift ? f.SkillId : e.SourceBind!,
            StateId = e.StateId!,
            Operation = e.Op,
            HolderSeat = holder,
            ParentWindowId = context.ParentFrameId,
            Token = token,
            Stage = gift ? RecipientCategoryMarkStage.ChoosingGift : e.Op == SkillProgramEffectOp.SpendCategoryMarkForAreaDiscard ? RecipientCategoryMarkStage.ChoosingTarget : e.Op == SkillProgramEffectOp.SpendCategoryMarkForSlashTargets ? RecipientCategoryMarkStage.ChoosingSlashTargets : RecipientCategoryMarkStage.RecoveryChildren,
            Materials = gift ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Select((card, index) => new RecipientCategoryMarkMaterial(card.Id, CardLocation.Hand(holder), index, false)).ToArray() : [],
            CandidateSeats = e.Op == SkillProgramEffectOp.SpendCategoryMarkForAreaDiscard ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray() : []
        };
        if (e.Op == SkillProgramEffectOp.SpendCategoryMarkForDyingRecovery)
        {
            _coreDyingOwnedCardSelectionsOperations.ExactDyingOwnedCardEntry(f, out var dying, out var entry);
            r = r with
            {
                DyingFrameId = dying.Id,
                OriginalDyingCursor = _coreDyingSuitsAndHistoricalUseObserversOperations.CaptureDyingSuitsOriginalCursor(dying, entry)
            };
        }

        if (e.Op == SkillProgramEffectOp.SpendCategoryMarkForSlashTargets)
        {
            _coreRecipientCategorySlashTargetsOperations.TryGetRecipientCategorySlashUse(f.OwnerSeat, context, out var use, out _);
            r = r with
            {
                CardUseFrameId = use.Id,
                ActionId = use.Action!.ActionId,
                BaseTargetSeats = use.TargetSeats,
                CandidateSeats = _coreMatchParticipantsPlayersAccess._players.Where(p => _coreRecipientCategorySlashTargetsOperations.CanBeRecipientCategorySlashTarget(use, p)).Select(p => p.Seat).ToArray()
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r });
        _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarkLedgerStarted = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkStartedEvent(f.Id, r.Source, r.GameplayHash, r.SourceSkillId, r.StateId, r.Operation, holder, r.ParentWindowId, token?.TokenId));
        if (r.OriginalDyingCursor is { } original)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkDyingCursorIssuedEvent(f.Id, original.DyingFrameId, global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime.DyingSuitsCursorHash(original)));
        if (!gift)
        {
            _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.Remove(new(holder, r.SourceSkillId, r.StateId));
            ChangeRecipientCategoryMarker(f, holder, token!.Kind, -1);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r = r with { Consumed = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkConsumedEvent(f.Id, token.TokenId, holder, r.SourceSkillId, r.StateId, token.Kind));
        }

        if (e.Op == SkillProgramEffectOp.SpendCategoryMarkForDyingRecovery)
        {
            var amount = Math.Max(0, 1 - _coreMatchParticipantsPlayersAccess._players[holder].Hp);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { RecoveryIssued = true, RecoveryAmount = amount }, PendingMovementContinuation = new(holder, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkRecoveryIssuedEvent(f.Id, holder, amount));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, holder, holder, amount, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishRecipientCategoryMarkChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int IndependentRecipientCategoryMarkerCount(CharacterState holder, PlayerMarkerKind marker, int source) => source == holder.Seat ? _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.Values.Count(token => token.HolderSeat == holder.Seat && RecipientPublicMarker(token.Kind) == marker) : 0;
    internal void ChangeRecipientCategoryMarker(ProgramSkillFrame f, int holder, RecipientCategoryMarkKind kind, int delta)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[holder];
        var marker = RecipientPublicMarker(kind);
        var key = (marker, holder);
        var owned = checked(p.MarkerSourceCounts.GetValueOrDefault(key) + delta);
        var total = checked(p.Markers.GetValueOrDefault(marker) + delta);
        if (owned < 0 || total < 0)
            throw new InvalidOperationException("A category token cannot consume an absent public marker.");
        if (owned == 0)
            p.MarkerSourceCounts.Remove(key);
        else
            p.MarkerSourceCounts[key] = owned;
        if (total == 0)
            p.Markers.Remove(marker);
        else
            p.Markers[marker] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, holder, marker, delta, total, holder, RecipientMarkReason(f, "marker")));
    }

    internal IReadOnlyList<RecipientCategoryMarkMaterial> RecipientCategoryAreaMaterials(int chooser, int target) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment }.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, target)).Select((card, slot) => (card, slot, from: new CardLocation(zone, target)))).Where(x => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, x.card, x.from, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, x.card, x.from, OwnedCardMoveIntent.Discard)).Select(x => new RecipientCategoryMarkMaterial(x.card.Id, x.from, x.slot, x.from.Zone == CardZoneKind.Hand && chooser != target)).ToArray());
    internal bool RecipientCategoryMaterialStillLegal(ProgramSkillFrame f, RecipientCategoryMarkMaterial m)
    {
        if (_cardZonesOperations._cardZones.GetLocation(m.CardId) != m.From || m.From.OwnerSeat is not { } holder || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive)
            return false;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId);
        return !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, m.From, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, m.From, OwnedCardMoveIntent.Discard);
    }

    internal IReadOnlyList<PromptChoice> RecipientCategoryMarkChoices(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark!;
        var choices = new List<PromptChoice>();
        PromptChoice Finish(string stage) => new(new($"category-mark.{(f.Id)}.{(stage)}.finish"), "完成选择", [], [], RecipientMarkParameters(f, "finish"));
        if (r.Stage == RecipientCategoryMarkStage.ChoosingGift)
            foreach (var m in r.Materials)
                choices.Add(new(new($"category-mark.{(f.Id)}.gift.{(m.CardId)}"), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId)), [m.CardId], [], RecipientMarkParameters(f, "gift")));
        else if (r.Stage == RecipientCategoryMarkStage.ChoosingTarget)
            foreach (var seat in r.CandidateSeats)
                choices.Add(new(new($"category-mark.{(f.Id)}.discard-target.{(seat)}"), _coreMatchParticipantsPlayersAccess._players[seat].Name, [], [seat], RecipientMarkParameters(f, "discard-target")));
        else if (r.Stage == RecipientCategoryMarkStage.ChoosingCards)
        {
            foreach (var m in r.Materials.Where(m => !r.SelectedCardIds.Contains(m.CardId)))
            {
                var parameters = RecipientMarkParameters(f, "discard-card");
                parameters["source-zone"] = m.From.Zone.ToString();
                parameters["slot-index"] = m.SlotIndex.ToString(CultureInfo.InvariantCulture);
                parameters["card-owner-seat"] = r.TargetSeat!.Value.ToString(CultureInfo.InvariantCulture);
                choices.Add(new(new($"category-mark.{(f.Id)}.discard.{(m.From.Zone)}.{(m.SlotIndex)}"), m.Hidden ? $"第 {(m.SlotIndex + 1)} 张手牌" : global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId)), m.Hidden ? [] : [m.CardId], [], parameters));
            }

            choices.Add(Finish("discard"));
        }
        else if (r.Stage == RecipientCategoryMarkStage.ChoosingSlashTargets)
        {
            foreach (var seat in r.CandidateSeats.Where(s => !r.AddedTargetSeats.Contains(s)))
                choices.Add(new(new($"category-mark.{(f.Id)}.slash-target.{(seat)}"), _coreMatchParticipantsPlayersAccess._players[seat].Name, [], [seat], RecipientMarkParameters(f, "slash-target")));
            choices.Add(Finish("slash"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishRecipientCategoryMarkChoice(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark!;
        var chooser = r.Stage == RecipientCategoryMarkStage.ChoosingGift ? r.HolderSeat : f.OwnerSeat;
        var choices = RecipientCategoryMarkChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage switch
        {
            RecipientCategoryMarkStage.ChoosingGift => "正面朝上交出一张手牌，并获得其类别对应的标记。",
            RecipientCategoryMarkStage.ChoosingTarget => "选择一名角色，弃置其区域里至多两张牌。",
            RecipientCategoryMarkStage.ChoosingCards => "选择至多两张牌；他人暗手牌仅显示牌位。",
            _ => "为此杀多指定至多两个合法目标。"
        }, Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("recipient-category-mark")]
    internal void ResolveRecipientCategoryMarkChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Category mark choice lost its owning frame.");
        _coreRecipientCategoryMarkObserversOperations.AssertRecipientCategoryMark(f);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !_coreRecipientCategoryMarkObserversOperations.IsRecipientCategoryMarkChoice(f, prompt) || !RecipientCategoryMarkChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime.SameNameHandChoicesEqual(c, choice)))
            throw new InvalidOperationException("A category mark choice changed its exact published actor, material or target.");
        var r = f.RecipientCategoryMark!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == RecipientCategoryMarkStage.ChoosingGift)
        {
            PayRecipientCategoryGift(f, choice.Cards.Single());
            return;
        }

        if (r.Stage == RecipientCategoryMarkStage.ChoosingTarget)
        {
            var target = choice.Targets.Single();
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Category discard target is no longer living.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { TargetSeat = target, Materials = RecipientCategoryAreaMaterials(f.OwnerSeat, target), Stage = RecipientCategoryMarkStage.ChoosingCards } });
            PublishRecipientCategoryMarkChoice(f);
            return;
        }

        if (r.Stage == RecipientCategoryMarkStage.ChoosingCards)
        {
            if (choice.Parameters["mark-action"] != "finish")
            {
                var m = r.Materials.Single(m => m.From.Zone.ToString() == choice.Parameters["source-zone"] && m.SlotIndex.ToString(CultureInfo.InvariantCulture) == choice.Parameters["slot-index"]);
                if (!RecipientCategoryMaterialStillLegal(f, m))
                    throw new InvalidOperationException("The frozen discard entity is no longer a legal payment.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r = r with { SelectedCardIds = r.SelectedCardIds.Append(m.CardId).ToArray() } });
                if (r.SelectedCardIds.Count < 2)
                {
                    PublishRecipientCategoryMarkChoice(f);
                    return;
                }
            }

            PayRecipientCategoryAreaDiscard(f);
            return;
        }

        if (choice.Parameters["mark-action"] != "finish")
        {
            var target = choice.Targets.Single();
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId!.Value)is not { } use || !_coreRecipientCategorySlashTargetsOperations.CanBeRecipientCategorySlashTarget(use, _coreMatchParticipantsPlayersAccess._players[target]))
                throw new InvalidOperationException("The frozen extra Slash target is no longer legal.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r = r with { AddedTargetSeats = r.AddedTargetSeats.Append(target).ToArray() } });
            if (r.AddedTargetSeats.Count < 2)
            {
                PublishRecipientCategoryMarkChoice(f);
                return;
            }
        }

        _coreRecipientCategorySlashTargetsOperations.CommitRecipientCategorySlashTargets(f);
    }

    internal void PayRecipientCategoryGift(ProgramSkillFrame f, int id)
    {
        var r = f.RecipientCategoryMark!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.HolderSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.ContainsKey(new(r.HolderSeat, r.SourceSkillId, r.StateId)) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.HolderSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "赠牌前来源、手牌或无标记条件已失效。");
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var category = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        var kind = category switch
        {
            SkillProgramCardCategory.Trick => RecipientCategoryMarkKind.Rescue,
            SkillProgramCardCategory.Equipment => RecipientCategoryMarkKind.Discard,
            _ => RecipientCategoryMarkKind.ExtraTargets
        };
        var e = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single();
        var before = RecipientMarkSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicHandRevealSelection = new("recipient-category-hand-gift", f.InstructionIndex, 0, id, card.Kind, CardLocation.Hand(r.HolderSeat), CardLocation.Hand(f.OwnerSeat), f.OwnerSeat, r.HolderSeat, EffectiveKind: card.Kind) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), r.HolderSeat, "category-mark-gift", Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "recipient-category-hand-gift"))
            return;
        ContinueRecipientCategoryPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void PayRecipientCategoryAreaDiscard(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark!;
        if (r.SelectedCardIds.Count == 0)
        {
            CompleteRecipientCategoryMark(f);
            return;
        }

        if (r.SelectedCardIds.Any(id => !RecipientCategoryMaterialStillLegal(f, r.Materials.Single(m => m.CardId == id))))
            throw new InvalidOperationException("Category discard cannot substitute a stale selected entity.");
        var before = RecipientMarkSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { Stage = RecipientCategoryMarkStage.DiscardChildren, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(r.TargetSeat!.Value, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.SelectedCardIds, new(RecipientMarkReason(f, "discard")), (batch, records) =>
        {
            foreach (var movement in records)
                _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var after = records.Max(m => m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RecipientCategoryMark = active.RecipientCategoryMark!with { SequenceAfter = after, BatchId = batch } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkMovementPaidEvent(f.Id, r.TargetSeat!.Value, records.Count, before, after, batch));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 140)]
    internal bool ResumeRecipientCategoryMark(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RecipientCategoryMark: { } r } f || f.Id != id)
            return false;
        _coreRecipientCategoryMarkObserversOperations.AssertRecipientCategoryMark(f);
        if (r.Stage is RecipientCategoryMarkStage.ChoosingGift or RecipientCategoryMarkStage.ChoosingTarget or RecipientCategoryMarkStage.ChoosingCards or RecipientCategoryMarkStage.ChoosingSlashTargets)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishRecipientCategoryMarkChoice(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        if (r.Stage == RecipientCategoryMarkStage.RecoveryChildren)
        {
            var before = RecipientMarkSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { Stage = RecipientCategoryMarkStage.DrawChildren, DrawIssued = true, DrawBefore = before, DrawAfter = before }, PendingMovementContinuation = new(r.HolderSeat, 0, null) });
            var actual = _coreMatchParticipantsPlayersAccess._players[r.HolderSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.HolderSeat], 1, true, new(RecipientMarkReason(f, "draw")), r.Source).Count : 0;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var after = RecipientMarkSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RecipientCategoryMark = active.RecipientCategoryMark!with { DrawActual = actual, DrawAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkDrawIssuedEvent(id, r.HolderSeat, actual, before, after));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        CompleteRecipientCategoryMark(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    internal void CompleteRecipientCategoryMark(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { Stage = RecipientCategoryMarkStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkCompletedEvent(f.Id, r.Operation, r.Consumed));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 120)]
    internal bool ReturnRecipientCategoryMarkMovement(ProgramSkillFrame f)
    {
        if (f.RecipientCategoryMark is not { Stage: RecipientCategoryMarkStage.GiftChildren or RecipientCategoryMarkStage.DiscardChildren or RecipientCategoryMarkStage.RecoveryChildren or RecipientCategoryMarkStage.DrawChildren })
            return false;
        _coreRecipientCategoryMarkObserversOperations.AssertRecipientCategoryMark(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 550)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 570)]
    internal bool CanContinueRecipientCategoryMark(ProgramSkillFrame f) => f.RecipientCategoryMark is { } r && (r.Consumed || r.Stage == RecipientCategoryMarkStage.GiftChildren) && _coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(f);
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 160)]
    internal bool IsRecipientCategoryMarkMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.RecipientCategoryMark is { } r && effect?.Op == r.Operation && f.PendingMovementContinuation == p && p.BeforeCount == 0 && p.CoverageResultBind is null && p.SubjectSeat == (r.Stage == RecipientCategoryMarkStage.DiscardChildren ? r.TargetSeat : r.HolderSeat) && _coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.GiveHandAndGrantCategoryMark, SkillProgramEffectOp.SpendCategoryMarkForDyingRecovery, SkillProgramEffectOp.SpendCategoryMarkForAreaDiscard, SkillProgramEffectOp.SpendCategoryMarkForSlashTargets)]
    internal PromptChoice SelectAiRecipientCategoryMarkChoice(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mark-action") != "finish" && (c.Targets.Count == 0 || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]))) ?? p.Choices[0];
    SkillProgramStepOutcome global::CardGame.Core.IRecipientCategoryMarksProgramHost.ExecuteRecipientCategoryMark(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteRecipientCategoryMark(f, e);
    [PublicHandRevealReturn("recipient-category-hand-gift")]
    internal void ContinueRecipientCategoryPublicReveal(ProgramSkillFrame f)
    {
        var r = f.RecipientCategoryMark ?? throw new InvalidOperationException("Category gift lost its native owner.");
        var payment = f.PublicHandRevealSelection ?? throw new InvalidOperationException("Category gift lost its selected Hand entity.");
        if (payment.ReturnKey != "recipient-category-hand-gift" || payment.InstructionIndex != f.InstructionIndex || payment.From != CardLocation.Hand(r.HolderSeat))
            throw new InvalidOperationException("Category gift changed its original holder.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicHandRevealSelection = null });
        var id = payment.CardId;
        if (_cardZonesOperations._cardZones.GetLocation(id) != payment.From || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompleteRecipientCategoryMark(f);
            return;
        }

        var category = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(payment.EffectiveKind!.Value);
        var kind = category switch
        {
            SkillProgramCardCategory.Trick => RecipientCategoryMarkKind.Rescue,
            SkillProgramCardCategory.Equipment => RecipientCategoryMarkKind.Discard,
            _ => RecipientCategoryMarkKind.ExtraTargets
        };
        var e = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single();
        var before = RecipientMarkSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RecipientCategoryMark = r with { Stage = RecipientCategoryMarkStage.GiftChildren, SelectedCardIds = [id], SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(r.HolderSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(f.OwnerSeat), new(RecipientMarkReason(f, "gift")), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.HolderSeat, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, records) =>
        {
            var token = new RecipientCategoryMarkToken(f.Id, r.HolderSeat, r.SourceSkillId, r.StateId, kind, e.SkillIds[(int)kind], r.Source, r.GameplayHash, records.Single().Sequence);
            _coreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks.Add(new(r.HolderSeat, r.SourceSkillId, r.StateId), token);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { RecipientCategoryMark = active.RecipientCategoryMark!with { Token = token, BatchId = batch, SequenceAfter = token.GiftSequence } });
            ChangeRecipientCategoryMarker(active, r.HolderSeat, kind, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkMovementPaidEvent(f.Id, r.HolderSeat, 1, before, token.GiftSequence, batch));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecipientCategoryMarkGrantedEvent(f.Id, token));
            // The holder permanently owns this grant. Origin identity stays on the token, never on grant qualification.
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[r.HolderSeat], $"recipient-category:{(r.HolderSeat)}:{(r.SourceSkillId)}:{(r.StateId)}", [token.DerivedSkillId]);
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
