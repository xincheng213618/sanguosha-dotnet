#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetMountProgramsRuntime
{
    internal static bool IsPrintedMount(CardKind kind) => RuntimeSkills.RuntimeRules_Core_DamageTargetMountPrograms.IsPrintedMount(kind);
    internal CardUseFrame? ActualDamageTargetMountUse(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } || source != ownerSeat || _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is not { } damage || damage.Id != context.ParentFrameId || damage.ParentFrameId != context.DamageFrameId)
            return null;
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(damage);
        if (attack.IsSourceLess || !attack.DamageWasApplied || attack.SourceSeat != ownerSeat || attack.TargetSeat != target || attack.EffectiveCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash))
            return null;
        // The real action is stored on the owning use (also for prepared multi-target/chain attacks).
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Id == attack.ResolutionId && u.Action is { Type: CardActionType.Use } a && a.ActorSeat == ownerSeat && a.EffectiveKind == attack.EffectiveCardKind);
    }

    internal IReadOnlyList<PromptChoice> DamageTargetMountChoices(long frameId, int ownerSeat, int targetSeat, string? skillId = null)
    {
        if (ownerSeat != targetSeat)
            return _coreProgramOtherOwnedCardDiscardsOperations.BuildOtherOwnedCardDiscardChoices(frameId, ownerSeat, CharacterGongsunZanBoundaryMobileRuntime.DamageTargetMountZonesFor(skillId), new CardMoveReason("skill-program.damage-target-mount.discard")).Where(c => c.Targets.SequenceEqual([targetSeat])).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "damage-target-mount" } }).ToArray();
        // The current text permits the actual victim to be this same source through chain/transfer.
        // Keep the mature only-other helper unchanged; this opt-in alone adds self-owned slots.
        var result = new List<PromptChoice>();
        if (!_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            return result;
        foreach (var zone in CharacterGongsunZanBoundaryMobileRuntime.DamageTargetMountZonesFor(skillId))
        {
            var cards = _cardZonesOperations._cardZones.CardsAt(new(zone, targetSeat));
            for (var slot = 0; slot < cards.Count; slot++)
            {
                var card = cards[slot];
                var hidden = zone == CardZoneKind.Hand;
                result.Add(new(new ChoiceId($"damage-target-mount.frame-{(frameId)}.self.{(zone)}.{(slot)}"), hidden ? $"弃置自己的第{(slot + 1)}个手牌牌位。" : $"弃置自己的【{(card.DisplayName)}】。", hidden ? [] : [card.Id], [targetSeat], new Dictionary<string, string> { ["program-action"] = "damage-target-mount", ["frame-id"] = frameId.ToString(CultureInfo.InvariantCulture), ["card-owner-seat"] = targetSeat.ToString(CultureInfo.InvariantCulture), ["source-zone"] = zone.ToString(), ["slot-index"] = slot.ToString(CultureInfo.InvariantCulture), ["move-reason"] = "skill-program.damage-target-mount.discard" }));
            }
        }

        return result;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1130, SkillProgramEffectOp.DiscardDamageTargetAndClaimMount)]
    internal bool CanOfferDamageTargetMountEdition(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => CanOfferDamageTargetMount(candidate.OwnerSeat, context, candidate.SkillId);
    internal bool CanOfferDamageTargetMount(int ownerSeat, ProgramSkillWindowContext context, string? skillId = null) => ActualDamageTargetMountUse(ownerSeat, context)is { } use && _characterGongsunZanBoundaryMobileOperations.DamageTargetMountUseMatchesEdition(skillId, use) && context.TargetSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && DamageTargetMountChoices(1, ownerSeat, seat, skillId).Count > 0;
    internal SkillProgramStepOutcome BeginDamageTargetMount(ProgramSkillFrame f)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var context = active.WindowContext ?? throw new InvalidOperationException("Mount discard lost its damage window.");
        var use = ActualDamageTargetMountUse(f.OwnerSeat, context);
        if (use?.Action is not { } action || !CanOfferDamageTargetMount(f.OwnerSeat, context, f.SkillId))
            return SkillProgramStepOutcome.Continue;
        if (active.DamageTargetMount is not null)
            throw new InvalidOperationException("Mount discard cannot pay twice.");
        var target = context.TargetSeat!.Value;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { DamageTargetMount = new(active.InstructionIndex, active.OwnerSeat, target, context.ParentFrameId, context.DamageFrameId!.Value, use.Id, action.ActionId, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.SkillInstanceId, active.GameplayHash) });
        var choices = DamageTargetMountChoices(f.Id, f.OwnerSeat, target, f.SkillId);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var region = f.SkillId == "boundary:qiaomeng-mobile" ? "装备区" : "区域";
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} {(region)}里的一张牌；若为坐骑牌，你获得之。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [target], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name + " · 弃置牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("damage-target-mount")]
    internal void ResolveDamageTargetMount(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Mount discard lost its program.");
        var draft = f.DamageTargetMount ?? throw new InvalidOperationException("Mount discard lost its payment draft.");
        if (draft.Receipt is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !DamageTargetMountChoices(f.Id, f.OwnerSeat, draft.TargetSeat, f.SkillId).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Mount discard must name its exact published opaque slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferDamageTargetMount(f.OwnerSeat, f.WindowContext!, f.SkillId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "付款前技能来源或伤害目标已失效。");
            return;
        }

        var source = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), draft.TargetSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(source)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        var destination = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, source, card.IsGeneralWeapon && source.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile);
        var reason = new CardMoveReason("skill-program.damage-target-mount.discard");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var discardInvocation = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [card.Id], reason, f.Id);
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([source], [destination]);
        var records = new List<CardMovementRecord>(1);
        var committed = false;
        try
        {
            _cardZonesOperations._cardZones.Move(card.Id, source, destination);
            records.Add(_corePhysicalMovementFacadesOperations.RecordMovement(card, source, destination, reason, record =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { DamageTargetMount = draft with { Receipt = new(record.Sequence, record.CardId, record.CardKind, record.From, record.To, record.Reason.Value, IsPrintedMount(card.Kind)) } });
            }));
            _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(card, source, destination);
            _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(card, source, destination);
            _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(card, source, reason);
            _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(card, source, destination);
            _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(card, source, destination);
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, records, committed);
        }

        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discardInvocation);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PendingMovementContinuation = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        }
    }

    internal bool IsValidDamageTargetMount(ProgramSkillFrame f)
    {
        if (f.DamageTargetMount is not { } d || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c || d.InstructionIndex != f.InstructionIndex || d.InstructionIndex < 1 || d.OwnerSeat != f.OwnerSeat || d.TargetSeat != c.TargetSeat || d.DamageWindowId != c.ParentFrameId || d.DamageFrameId != c.DamageFrameId || c.SourceSeat != f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.TargetSeat) || d.SkillId != f.SkillId || d.BindingId != CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) || d.SkillInstanceId != f.SkillInstanceId || d.GameplayHash != f.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Instructions.Count != 1 || plan.Instructions[0].Op != SkillProgramEffectOp.DiscardDamageTargetAndClaimMount || plan.Trigger?.Subject != SkillProgramTriggerSubject.DamageSource || plan.Trigger.Optional != true)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == d.CardUseFrameId);
        if (use?.Action is not { Type: CardActionType.Use } action || action.ActionId != d.CardActionId || action.ActorSeat != d.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || !_characterGongsunZanBoundaryMobileOperations.DamageTargetMountUseMatchesEdition(f.SkillId, use))
            return false;
        if (d.Receipt is not { } r)
            return !d.ClaimIssued && d.ClaimMovementSequence is null;
        var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == r.MovementSequence);
        return actual is not null && actual.CardId == r.CardId && actual.CardKind == r.PrintedKind && actual.From == r.From && actual.To == r.To && actual.Reason.Value == r.Reason && r.Reason == "skill-program.damage-target-mount.discard" && r.From.OwnerSeat == d.TargetSeat && CharacterGongsunZanBoundaryMobileRuntime.DamageTargetMountZonesFor(f.SkillId).Contains(r.From.Zone) && r.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), r.From, r.PrintedKind == CardKind.GeneralWeapon && r.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) && r.IsMount == IsPrintedMount(r.PrintedKind) && (d.ClaimMovementSequence is null ? !d.ClaimIssued : d.ClaimIssued && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == d.ClaimMovementSequence && m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == r.To && (m.To == CardLocation.Hand(d.OwnerSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(d.OwnerSeat))) && m.Reason.Value == "skill-program.damage-target-mount.claim"));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 840)]
    internal bool ResumeDamageTargetMount(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.DamageTargetMount is not { Receipt: not null } d)
            return false;
        if (!IsValidDamageTargetMount(f))
            throw new InvalidOperationException("Mount claim lost its exact paid producer.");
        if (f.PendingMovementContinuation is not null)
        {
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        }

        if (!d.ClaimIssued && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && d.Receipt.IsMount && d.Receipt.To == CardLocation.DiscardPile && _cardZonesOperations._cardZones.GetLocation(d.Receipt.CardId) == d.Receipt.To)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetMount = d with { ClaimIssued = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var card = _cardZonesOperations._cardZones.CardsAt(d.Receipt.To).Single(c => c.Id == d.Receipt.CardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, d.Receipt.To, CardLocation.Hand(f.OwnerSeat), new CardMoveReason("skill-program.damage-target-mount.claim"), record =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { DamageTargetMount = current.DamageTargetMount!with { ClaimMovementSequence = record.Sequence } });
            });
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
            return ResumeDamageTargetMount(id);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetMount = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 800)]
    internal bool CanContinuePaidDamageTargetMount(ProgramSkillFrame f) => f.DamageTargetMount?.Receipt is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && IsValidDamageTargetMount(f);
    SkillProgramStepOutcome IDamageTargetMountProgramHost.DiscardDamageTargetAndClaimMount(ProgramSkillFrame f) => BeginDamageTargetMount(f);
}
