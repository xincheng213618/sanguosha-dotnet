#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeEquipmentInvocationsRuntime
{
    internal static CardKind NativeInvocationEquipment(NativeEquipmentInvocationKind kind) => kind switch
    {
        NativeEquipmentInvocationKind.Bagua or NativeEquipmentInvocationKind.FactionBagua => CardKind.BaguaFormation,
        NativeEquipmentInvocationKind.IceSword => CardKind.IceSword,
        NativeEquipmentInvocationKind.StoneAxe => CardKind.StoneAxe,
        NativeEquipmentInvocationKind.QilinBow => CardKind.QilinBow,
        NativeEquipmentInvocationKind.Cixiong => CardKind.CixiongDoubleSwords,
        NativeEquipmentInvocationKind.Qinglong => CardKind.QinglongCrescentBlade,
        _ => throw new InvalidOperationException("Unknown native equipment invocation.")};
    internal static string NativeEquipmentFingerprint(NativeEquipmentInvocationReceipt r) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(r))));
    internal NativeEquipmentInvocationReceipt CaptureNativeEquipmentInvocation(NativeOwners.CardAttackHandle attack, NativeEquipmentInvocationKind kind, int actorSeat, int targetSeat)
    {
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() ?? throw new InvalidOperationException("Equipment invocation requires a live native owner.");
        var equipment = NativeInvocationEquipment(kind);
        var cardId = _characterHuCheErMouOperations.KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[actorSeat]).FirstOrDefault(c => c.Kind == equipment)?.Id ?? 0;
        var ordinal = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeEquipmentInvocationIssuedEvent>().Count(e => e.ParentFrameId == parent.Id && e.Kind == kind && e.ActorSeat == actorSeat);
        var nonce = $"{(_randomOperations._random.Next(int.MaxValue)):X8}{(_randomOperations._random.Next(int.MaxValue)):X8}";
        return new(parent.Id, attack.ResolutionId, kind, actorSeat, targetSeat, CardCatalog.Get(equipment).DisplayName, $"native-equipment:{(attack.ResolutionId)}:{(kind)}:{(actorSeat)}:{(ordinal)}", cardId, CommitmentNonce: nonce);
    }

    internal bool TryBeginNativeEquipmentInvocation(NativeEquipmentInvocationReceipt receipt)
    {
        if (!_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != receipt.ParentFrameId || parent.NativeEquipmentInvocation is not null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[receipt.ActorSeat].IsAlive || receipt.PhysicalCardIds is not System.Collections.IList { IsReadOnly: true })
            throw new InvalidOperationException("A native equipment activation requires its original clear owner and immutable accepted materials.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NativeEquipmentInvocation = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEquipmentInvocationIssuedEvent(receipt.ParentFrameId, receipt.AttackId, receipt.Kind, receipt.ActorSeat, receipt.TargetSeat, receipt.SkillKey, receipt.OccurrenceKey, NativeEquipmentFingerprint(receipt)));
        if (!_characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(parent.Id, receipt.ActorSeat, receipt.SkillKey, receipt.OccurrenceKey, returnKind: SkillInvocationReturnKind.NativeEquipment))
            throw new InvalidOperationException("A newly issued equipment occurrence did not acquire its exact invocation child.");
        return true;
    }

    internal bool TryBeginBaguaEquipmentInvocation(NativeOwners.CardAttackHandle attack, CharacterState defender, bool factionDefense = false)
    {
        if (!_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers)
            return false;
        return TryBeginNativeEquipmentInvocation(CaptureNativeEquipmentInvocation(attack, factionDefense ? NativeEquipmentInvocationKind.FactionBagua : NativeEquipmentInvocationKind.Bagua, defender.Seat, attack.TargetSeat));
    }

    internal bool TryBeginIceSwordEquipmentInvocation(NativeOwners.IceSwordHandle pending) => !_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers ? false : TryBeginNativeEquipmentInvocation(CaptureNativeEquipmentInvocation(pending.Attack, NativeEquipmentInvocationKind.IceSword, pending.Attack.SourceSeat, pending.Attack.TargetSeat));
    internal bool TryBeginStoneAxeEquipmentInvocation(NativeOwners.CardAttackHandle attack) => !_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers ? false : TryBeginNativeEquipmentInvocation(CaptureNativeEquipmentInvocation(attack, NativeEquipmentInvocationKind.StoneAxe, attack.SourceSeat, attack.TargetSeat));
    internal bool TryBeginQilinBowEquipmentInvocation(NativeOwners.CardAttackHandle attack, Card discardedMount) => !_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers ? false : TryBeginNativeEquipmentInvocation(CaptureNativeEquipmentInvocation(attack, NativeEquipmentInvocationKind.QilinBow, attack.SourceSeat, attack.TargetSeat)with { SelectedCardId = discardedMount.Id });
    internal bool TryBeginCixiongEquipmentInvocation(NativeOwners.CixiongDoubleSwordsHandle pending) => !_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers ? false : TryBeginNativeEquipmentInvocation(CaptureNativeEquipmentInvocation(pending.Attack, NativeEquipmentInvocationKind.Cixiong, pending.Attack.SourceSeat, pending.Attack.TargetSeat));
    internal bool TryBeginQinglongEquipmentInvocation(NativeOwners.QinglongCrescentBladeHandle pending, Card slash, CardKind effectiveSlashKind, int physicalOwnerSeat, IReadOnlyList<Card> physicalCards, NativeOwners.FactionCardRequestHandle? jijiang = null, CardConversionSource? conversionSource = null)
    {
        if (!_characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers)
            return false;
        var selected = conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        var r = CaptureNativeEquipmentInvocation(pending.Attack, NativeEquipmentInvocationKind.Qinglong, pending.Attack.SourceSeat, pending.Attack.TargetSeat)with
        {
            SlashCard = slash,
            SelectedCardId = slash.Id,
            EffectiveSlashKind = effectiveSlashKind,
            PhysicalOwnerSeat = physicalOwnerSeat,
            PhysicalCardIds = physicalCards.Select(c => c.Id).ToArray(),
            PhysicalFrom = physicalCards.Select(c => _cardZonesOperations._cardZones.GetLocation(c.Id)).ToArray(),
            FactionRequestId = jijiang?.OwnerFrameId,
            ProviderRewardCount = jijiang is null ? 0 : _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(jijiang).DrawCount,
            ConversionSource = selected
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        return TryBeginNativeEquipmentInvocation(r);
    }

    internal GameEngine.NativeEquipmentSuspensionProjection? FindNativeEquipmentInvocation(long attackId)
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            var parent = _resolutionStackOperations._resolutionStack[index];
            if (parent.NativeEquipmentInvocation is not { } r || r.AttackId != attackId || r.ParentFrameId != parent.Id || !ValidNativeEquipmentReceipt(r) || _resolutionStackOperations._resolutionStack[index + 1] is not SkillInvocationFrame invocation || invocation.ReturnKind != SkillInvocationReturnKind.NativeEquipment || invocation.ParentFrameId != parent.Id || invocation.ActorSeat != r.ActorSeat || invocation.SkillKey != r.SkillKey || invocation.OccurrenceKey != r.OccurrenceKey || !_characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(parent, invocation) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeEquipmentInvocationReturnedEvent>().Any(e => e.InvocationId == invocation.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeEquipmentInvocationIssuedEvent>().Count(e => e == new NativeEquipmentInvocationIssuedEvent(parent.Id, r.AttackId, r.Kind, r.ActorSeat, r.TargetSeat, r.SkillKey, r.OccurrenceKey, NativeEquipmentFingerprint(r))) != 1)
                continue;
            var root = parent as CardUseFrame;
            if (r.Kind == NativeEquipmentInvocationKind.FactionBagua && index > 0 && parent is ResponseWindowFrame response && response.ParentFrameId == attackId && _resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame responseRoot)
                root = responseRoot;
            if (root is null || root.Id != attackId || !NativeEquipmentHostMatches(parent, root, r) || !_coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => this.NativeEquipmentObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)))
                continue;
            return new(parent, root, invocation, r, index);
        }

        return null;
    }

    internal static bool ValidNativeEquipmentReceipt(NativeEquipmentInvocationReceipt r) => Enum.IsDefined(r.Kind) && r.PhysicalCardIds is System.Collections.IList { IsReadOnly: true } && r.PhysicalFrom is System.Collections.IList { IsReadOnly: true } && r.PhysicalFrom.Count == r.PhysicalCardIds.Count && r.PhysicalCardIds.Distinct().Count() == r.PhysicalCardIds.Count && r.CommitmentNonce.Length == 16 && r.CommitmentNonce.All(Uri.IsHexDigit) && r.SkillKey == CardCatalog.Get(NativeInvocationEquipment(r.Kind)).DisplayName && !string.IsNullOrWhiteSpace(r.OccurrenceKey);
    internal bool NativeEquipmentHostMatches(ResolutionFrame parent, CardUseFrame root, NativeEquipmentInvocationReceipt r)
    {
        if (root.CardAttack is not { } attack || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || attack.TargetSeat != r.TargetSeat || attack.EffectiveCardKind != root.CardKind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || (r.Kind is NativeEquipmentInvocationKind.Bagua or NativeEquipmentInvocationKind.FactionBagua ? !CoreNativeCardClassificationsRuntime.IsSlashCard(root.CardKind) && root.CardKind != CardKind.ArrowBarrage : !CoreNativeCardClassificationsRuntime.IsSlashCard(root.CardKind)))
            return false;
        var c = root.Continuations;
        if (r.Kind == NativeEquipmentInvocationKind.FactionBagua)
            return parent is ResponseWindowFrame { Step: ResolutionFrameStep.AwaitingResponse } response && response.ParentFrameId == root.Id && response.SourceSeat == attack.SourceSeat && response.IncomingCard == root.CardKind && response.ResponderSeat == attack.TargetSeat && root.Step == ResolutionFrameStep.AwaitingResponse && !attack.IgnoresArmor && attack.FactionDefenseAttempted && c.FactionDefense is { Active: true } defense && defense.OwnerSeat == attack.TargetSeat && defense.CandidateIndex >= 0 && defense.CandidateIndex < defense.CandidateSeats.Count && defense.CandidateSeats[defense.CandidateIndex] == r.ActorSeat;
        if (parent.Id != root.Id || parent is not CardUseFrame)
            return false;
        if (r.Kind == NativeEquipmentInvocationKind.Bagua)
            return r.ActorSeat == attack.TargetSeat && !attack.IgnoresArmor && root.Step == ResolutionFrameStep.ResolvingEffect;
        if (r.ActorSeat != attack.SourceSeat)
            return false;
        return r.Kind switch
        {
            NativeEquipmentInvocationKind.IceSword => root.Step == ResolutionFrameStep.AwaitingResponse && attack.IceSwordAttempted && c.IceSword is { Active: true, Activated: true, DiscardedCardIds.Count: 0 },
            NativeEquipmentInvocationKind.StoneAxe => root.Step == ResolutionFrameStep.ResolvingEffect && c.StoneAxe is not { Active: true } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<StoneAxeResolvedEvent>().LastOrDefault(e => e.ResolutionId == root.Id)is { Used: true, DiscardedCardIds.Count: 2 } paid && paid.SourceSeat == attack.SourceSeat && paid.TargetSeat == attack.TargetSeat && paid.DiscardedCardIds.Distinct().Count() == 2,
            NativeEquipmentInvocationKind.QilinBow => root.Step == ResolutionFrameStep.ResolvingEffect && attack.QilinBowAttempted && c.QilinBow is not { Active: true } && r.SelectedCardId > 0,
            NativeEquipmentInvocationKind.Cixiong => root.Step == ResolutionFrameStep.Declared && c.CixiongDoubleSwords is { Active: true, Stage: CixiongDoubleSwordsStage.TargetChoice },
            NativeEquipmentInvocationKind.Qinglong => root.Step == ResolutionFrameStep.AwaitingResponse && c.QinglongCrescentBlade is { Active: true } && r.SlashCard is not null && r.SelectedCardId == r.SlashCard.Id && r.EffectiveSlashKind is { } effective && CoreNativeCardClassificationsRuntime.IsSlashCard(effective) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.PhysicalOwnerSeat),
            _ => false
        };
    }

    internal bool NativeEquipmentObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        // Invocation-owned costs/returns are stricter than the generic movement
        // edges; an unrecognized child of that native frame must be rejected.
        if (parent is SkillInvocationFrame || child is SkillInvocationFrame)
            return _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(parent, child);
        return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreObserverSubtreesOperations.IsMovementObserverEdge(index);
    }
    internal ResolutionFrame? NativeEquipmentInvariantTop(ResolutionFrame? actualTop)
    {
        if (actualTop?.Id != _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id || actualTop is DyingFrame or JudgmentFrame or CardUseFrame)
            return actualTop;
        return _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && FindNativeEquipmentInvocation(attack.ResolutionId)is { } projection ? projection.Parent : actualTop;
    }

    internal void ContinueNativeEquipmentInvocation(SkillInvocationFrame frame)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { NativeEquipmentInvocation: { } r } parent || parent.Id != frame.ParentFrameId || frame.ReturnKind != SkillInvocationReturnKind.NativeEquipment || frame.ActorSeat != r.ActorSeat || frame.SkillKey != r.SkillKey || frame.OccurrenceKey != r.OccurrenceKey || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeEquipmentInvocationIssuedEvent>().Count(e => e == new NativeEquipmentInvocationIssuedEvent(parent.Id, r.AttackId, r.Kind, r.ActorSeat, r.TargetSeat, r.SkillKey, r.OccurrenceKey, NativeEquipmentFingerprint(r))) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeEquipmentInvocationReturnedEvent>().Any(e => e.InvocationId == frame.Id) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SkillInvocationCompletedEvent>().Any(e => e.FrameId == frame.Id && e.ParentFrameId == parent.Id && e.OccurrenceKey == r.OccurrenceKey))
            throw new InvalidOperationException("A native equipment activation lost its original exact invocation return.");
        NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle(r.AttackId);
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || r.TargetSeat != attack.TargetSeat)
            throw new InvalidOperationException("Native equipment resumed a different attack or target.");
        var canceled = !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NativeEquipmentInvocation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEquipmentInvocationReturnedEvent(frame.Id, parent.Id, r.AttackId, r.Kind, r.OccurrenceKey, canceled));
        if (canceled)
        {
            CancelNativeEquipmentContinuation(r, attack);
            return;
        }

        switch (r.Kind)
        {
            case NativeEquipmentInvocationKind.Bagua:
            case NativeEquipmentInvocationKind.FactionBagua:
            {
                var faction = r.Kind == NativeEquipmentInvocationKind.FactionBagua;
                if (faction && (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not { } defense || defense.CurrentCandidateSeat != r.ActorSeat || !CoreNativeDamageHandlesRuntime.SameAttackOwner(defense.Attack, attack)))
                    throw new InvalidOperationException("Faction Bagua lost its original provider cursor.");
                var result = _coreNativeJudgmentOperations.BeginJudgment(attack, r.ActorSeat, JudgmentReasons.BaguaDefense, attack.ResolutionId, attack.EffectiveCardKind, faction ? JudgmentContinuationKind.FactionDefenseBagua : JudgmentContinuationKind.Bagua);
                if (result is { } succeeded)
                {
                    if (faction)
                        _coreNativeDodgeOperations.CompleteFactionDefenseBaguaResponse(attack, succeeded);
                    else
                        _coreNativeJudgmentOperations.CompleteBaguaResponse(attack, succeeded);
                }

                return;
            }

            case NativeEquipmentInvocationKind.IceSword:
                if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is not { Activated: true } ice || !CoreNativeDamageHandlesRuntime.SameAttackOwner(ice.Attack, attack))
                    throw new InvalidOperationException("Ice Sword lost its accepted activation.");
                if (_coreNativeIceSwordOperations.GetIceSwordTargetCardCount(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], r.ActorSeat) == 0)
                    _coreNativeIceSwordOperations.FinishIceSwordPrevention(ice);
                else
                    _coreNativeIceSwordOperations.PublishIceSwordChoice(ice);
                return;
            case NativeEquipmentInvocationKind.StoneAxe:
                if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<StoneAxeResolvedEvent>().Any(e => e.ResolutionId == r.AttackId && e.Used && e.DiscardedCardIds.Count == 2))
                    throw new InvalidOperationException("Stone Axe lost its real paid two-card cost.");
                if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                return;
            case NativeEquipmentInvocationKind.QilinBow:
                ContinueInvokedQilinBow(attack, _coreNativeQilinBowOperations.GetQilinBowTargetMounts(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).SingleOrDefault(c => c.Id == r.SelectedCardId));
                return;
            case NativeEquipmentInvocationKind.Cixiong:
                if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is not { Stage: CixiongDoubleSwordsStage.TargetChoice } cixiong || !CoreNativeDamageHandlesRuntime.SameAttackOwner(cixiong.Attack, attack))
                    throw new InvalidOperationException("Cixiong lost its accepted target-choice stage.");
                PublishCixiongAcceptedTargetChoice(cixiong);
                return;
            case NativeEquipmentInvocationKind.Qinglong:
                ContinueInvokedQinglong(r, attack);
                return;
            default:
                throw new InvalidOperationException("No continuation for the accepted native equipment.");
        }
    }

    internal void CancelNativeEquipmentContinuation(NativeEquipmentInvocationReceipt r, NativeOwners.CardAttackHandle attack)
    {
        if (r.Kind == NativeEquipmentInvocationKind.FactionBagua && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense)
        {
            defense.CandidateIndex++;
            _coreNativeFactionDefenseOperations.AdvanceFactionDefenseCandidate();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is { } ice && CoreNativeDamageHandlesRuntime.SameAttackOwner(ice.Attack, attack))
            _coreNativeCardContinuationCursorsOperations.ActiveIceSword = null;
        if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { } cixiong && CoreNativeDamageHandlesRuntime.SameAttackOwner(cixiong.Attack, attack))
            _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords = null;
        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, attack))
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
        if (r.Kind == NativeEquipmentInvocationKind.Qinglong)
            _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(attack.ResolutionId);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == attack.ResolutionId)
            _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void PublishCixiongAcceptedTargetChoice(NativeOwners.CixiongDoubleSwordsHandle pending)
    {
        NativeOwners.CardAttackHandle attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords, pending) || pending.Stage != CixiongDoubleSwordsStage.TargetChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Cixiong target choice requires the exact already accepted activation.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).OrderBy(c => c.Id).ToArray();
        var choices = hand.Select(card => new PromptChoice(new($"cixiong.discard.card-{(card.Id)}"), $"弃置【{(card.DisplayName)}】（{(card.RankText)}）", [card.Id], [], new Dictionary<string, string> { ["action"] = "cixiong-discard" })).Append(new(new("cixiong.allow-draw"), $"令 {(source.Name)} 摸一张牌。", [], [], new Dictionary<string, string> { ["action"] = "cixiong-draw" })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.CixiongDoubleSwords, target.Seat, $"{(source.Name)} 发动【雌雄双股剑】，请选择弃置一张手牌，或令其摸一张牌。", hand.Select(c => c.Id).ToArray(), [], source.Seat, attack.EffectiveCardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = target.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ContinueInvokedQilinBow(NativeOwners.CardAttackHandle attack, Card? mount)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (mount is not null && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(source.Seat, mount, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard))
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(source.Seat, source.Seat, mount, CardLocation.Equipment(target.Seat), CardMoveReasons.QilinBowDiscard, parentFrameId: attack.ResolutionId);
        else
            mount = null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QilinBowResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, mount is not null, mount?.Id));
        _coreMatchLogOperations.AddLog(mount is null ? "EquipmentSkipped" : "EquipmentEffect", mount is null ? $"{(source.Name)} 未发动【麒麟弓】，继续对 {(target.Name)} 结算伤害。" : $"{(source.Name)} 发动【麒麟弓】，弃置 {(target.Name)} 的【{(mount.DisplayName)}】。", source.Seat, target.Seat);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void ContinueInvokedQinglong(NativeEquipmentInvocationReceipt r, NativeOwners.CardAttackHandle attack)
    {
        NativeOwners.QinglongCrescentBladeHandle pending = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade;
        if (pending is null || !CoreNativeDamageHandlesRuntime.SameAttackOwner(pending.Attack, attack) || r.SlashCard is null || r.EffectiveSlashKind is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.PhysicalOwnerSeat))
            throw new InvalidOperationException("Qinglong lost its exact accepted Slash materials.");
        var materials = r.PhysicalCardIds.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray();
        var provider = _coreMatchParticipantsPlayersAccess._players[r.PhysicalOwnerSeat];
        var legal = provider.IsAlive && materials.Select((c, i) => _cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat == r.PhysicalOwnerSeat && _cardZonesOperations._cardZones.GetLocation(c.Id) == r.PhysicalFrom[i]).All(x => x) && !_coreCardUseModulesOperations.IsCardUseForbidden(r.ActorSeat, r.EffectiveSlashKind.Value, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(r.ActorSeat, r.TargetSeat, r.EffectiveSlashKind.Value);
        if (!legal)
        {
            CancelNativeEquipmentContinuation(r, attack);
            return;
        }

        NativeOwners.FactionCardRequestHandle? jijiang = r.FactionRequestId is { } requestId ? _kernelCoreMainOperations.CreateFactionCardRequestHandle(requestId) : null;
        // Only this verified typed return may bypass the original invocation
        // gate. Actual card payment and Slash settlement remain native.
        _coreNativeQinglongOperations.BeginQinglongCrescentBladeFollowup(pending, r.SlashCard, r.EffectiveSlashKind.Value, r.PhysicalOwnerSeat, materials, jijiang, r.ConversionSource, skipInvocation: true);
        if (r.FactionRequestId is { } rewardedRequest && r.ProviderRewardCount > 0)
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(new(rewardedRequest, r.ProviderRewardCount), r.PhysicalOwnerSeat);
    }
}
