#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenyiPeachSuppliesRuntime
{
    internal const string WenyiPeachSupplyAction = "wenyi-peach-supply";
    internal bool IsWenyiPeachSupplyQuery(CharacterState actor, int target) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && _characterHuanShuJunOlOperations.IsWenyiPeachRequest(f) && _characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(f, out var requestedActor, out var requestedTarget) && requestedActor == actor.Seat && requestedTarget == target;
    internal bool WenyiPeachUseLegal(int actorSeat, int targetSeat, IReadOnlyList<Card> materials, CardConversionSource? source = null)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[targetSeat]) || _coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, CardKind.Peach, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, targetSeat, CardKind.Peach) || source is not null && _corePlayCardHistoryOperations.ViewAsRule(source)is { } rule && (rule.ExcludeOwnerEffects && actorSeat == targetSeat || rule.NoDying && (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Hp <= 0))) || actorSeat == targetSeat && _coreHandComparisonOperations.HasSelfCardTargetProhibition(actorSeat) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actorSeat, materials.Select(c => c.Id).ToArray()))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        if (materials.Any(card => _coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)))
            return false;
        Suit? suit = materials.Count == 0 ? Suit.None : materials.Count == 1 ? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, materials[0], source)) : _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, materials);
        var color = materials.Count == 0 ? null : materials.Count == 1 ? CoreGameEnginePublicPileColorsRuntime.SuitColor(suit!.Value) : _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, materials);
        return !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actorSeat, targetSeat, suit) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Peach, suit, color) && (suit is null || !_coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actorSeat, suit.Value));
    }

    internal IReadOnlyList<PromptChoice> WenyiPeachSupplyChoices(ProgramSkillFrame f)
    {
        if (!_characterHuanShuJunOlOperations.IsWenyiPeachRequest(f) || !_characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(f, out var actorSeat, out var target))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var result = new List<PromptChoice>();
        Dictionary<string, string> Args(string kind) => new()
        {
            ["program-action"] = WenyiPeachSupplyAction,
            ["wenyi-supply"] = kind,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        void Add(string kind, IReadOnlyList<Card> cards, CardConversionSource? source, string label)
        {
            if (!WenyiPeachUseLegal(actorSeat, target, cards, source))
                return;
            var args = Args(kind);
            if (source is not null)
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            if (kind == "original-pile")
                args["native-original-public-pile"] = "true";
            result.Add(new(new($"wenyi.peach.{(f.Id)}.{(kind)}.{(source?.SkillId)}.{(source?.BindingId)}.{(source?.SkillInstanceId)}.{(string.Join('-', cards.Select(c => c.Id)))}"), label, cards.Select(c => c.Id).ToArray(), [target], args));
        }

        var natural = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id)is var from && (from == CardLocation.Hand(actorSeat) || from == CardLocation.WoodenOxGrain(actorSeat)) && c.Kind == CardKind.Peach && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, c));
        foreach (var card in natural)
            Add("native", [card], null, "对其使用【桃】");
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(actor, c, CardKind.Peach)))
            foreach (var identity in _characterGuanYuWumiaoOperations.WuyouIdentitySources(actor, card, CardKind.Peach))
                Add("identity", [card], identity, "使用武佑原牌【桃】");
        var publicRequest = _coreNativePublicPileOriginalCardsOperations.NativePublicPileUseRequest(actorSeat, actorSeat, CardKind.Peach, f.Id);
        foreach (var selection in _coreNativePublicPileOriginalCardsOperations.GetNativePublicPileOriginalCards(publicRequest).Where(s => s.Card.Kind == CardKind.Peach))
            Add("original-pile", [selection.Card], null, "使用公共粮原牌【桃】");
        var inputs = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).DistinctBy(c => c.Id).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c)).ToArray();
        foreach (var card in inputs)
            foreach (var source in _coreCardConversionsOperations.FindProgramViewAsConversions(actor, card, CardKind.Peach, false, requestedUseTarget: target).Concat(_coreCardConversionsOperations.GetProgramCardIdentitySources(actor, card, CardKind.Peach, false)).Distinct())
                Add("single", [card], source, $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，将此牌当【桃】对其使用");
        foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, CardKind.Peach, false, requestedUseTarget: target))
            Add("multi", selection.Cards, selection.Source, $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将{(selection.Cards.Count)}张牌当【桃】对其使用");
        foreach (var(source, rule, _)in _coreTieredRoundZeroUsesOperations.TieredRoundZeroRules(actor, CardKind.Peach))
        {
            if (rule.ExcludeOwnerEffects && actorSeat == target)
                continue;
            if (_characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId))
            {
                var paid = _characterWoLongFengChuOlOperations.UnconsumedEquipmentSlotPayment(source);
                if (paid is not null)
                    continue; // Only the exact typed return can redeem this request's paid invoice.
                if (!Enum.GetValues<EquipmentSlot>().Any(slot => _characterWoLongFengChuOlOperations.EquipmentSlotNamedChoiceIsPayable(actor, source, CardKind.Peach, EquipmentSlotNamedUseIntent.WenyiPeach, [target], slot)))
                    continue;
            }

            Add("tiered-zero", [], source, $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为对其使用【桃】");
        }

        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    [ProgramAnswer(WenyiPeachSupplyAction)]
    internal void ResolveWenyiPeachSupply(PromptChoice choice)
    {
        if (!TryResolveWenyiPeachSupply(choice))
            throw new InvalidOperationException("Wenyi requires its exact published Peach supply.");
    }

    internal bool TryResolveWenyiPeachSupply(PromptChoice choice)
    {
        if (choice.Parameters.GetValueOrDefault("program-action") != WenyiPeachSupplyAction)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !_characterHuanShuJunOlOperations.IsWenyiPeachRequest(f) || !_characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(f, out var actorSeat, out var target) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != actorSeat || prompt.SkillPrompt?.SkillId != f.SkillId || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || !WenyiPeachSupplyChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Wenyi Peach changed its native parent, target, source or selected entities.");
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var branch = choice.Parameters["wenyi-supply"];
        CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source);
        var cards = choice.Cards.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id)).ToArray();
        if (branch == "tiered-zero" && source is not null && _characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId))
        {
            // Reserve the accepted Wenyi right before the real slot cost and its children.
            _characterHuanShuJunOlOperations.PrepareWenyiSuppliedPeach(f, actorSeat, [], source);
            _characterWoLongFengChuOlOperations.BeginEquipmentSlotNamedUse(actor, source, CardKind.Peach, EquipmentSlotNamedUseIntent.WenyiPeach, prompt, null, choice);
            return true;
        }

        if (branch == "original-pile")
        {
            var request = _coreNativePublicPileOriginalCardsOperations.NativePublicPileUseRequest(actorSeat, actorSeat, CardKind.Peach, f.Id);
            var selected = _coreNativePublicPileOriginalCardsOperations.GetNativePublicPileOriginalCards(request).Single(s => s.Card.Id == cards.Single().Id);
            _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPilePayment(f, selected);
        }

        _characterHuanShuJunOlOperations.PrepareWenyiSuppliedPeach(f, actorSeat, cards, source);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var appearance = cards.Length == 0 ? new Card(0, CardKind.Peach, Suit.None, 0) : cards.Length == 1 ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, cards[0], source) : cards[0];
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, _coreMatchParticipantsPlayersAccess._players[target], appearance, "桃", CardKind.Peach, recoveryAmount: 1 + (source is null ? 0 : _corePlayCardHistoryOperations.ViewAsRule(source)?.RecoveryBonus ?? 0), conversionSource: source, physicalCards: cards);
        return true;
    }

    internal bool IsReservedWenyiPeachSupply(ProgramSkillFrame f, int actor, int target, CardConversionSource source) => _characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(f, out var actualActor, out var actualTarget) && actualActor == actor && actualTarget == target && f.HuanShuJun is { Paid: true, Stage: HuanShuJunStage.NativeIssuing, Kind: CardKind.Peach, Materials.Count: 0 } r && r.Sources.SequenceEqual([source]) && source.OwnerSeat == actor && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiPaidEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == actor && e.TargetSeat == target && e.HealthWindowId == r.WindowId && e.Branch == "peach") == 1;
    internal bool WenyiDrawFundedParentMatches(ProgramSkillFrame f, DrawFundedDistinctBasicPayment p) => p.Intent == DrawFundedDistinctBasicIntent.WenyiPeach && p.EffectiveKind == CardKind.Peach && p.ParentFrameId == f.Id && p.RequestFrameId == f.Id && p.Cursor == f.InstructionIndex && p.TargetSeat is { } target && IsReservedWenyiPeachSupply(f, p.Source.OwnerSeat, target, p.Source);
    internal void ReserveWenyiDrawFundedPeach(PendingDecision original, GameEngine.DrawFundedBasicNeed need, CardConversionSource source, CardKind kind)
    {
        if (kind != CardKind.Peach || need.Intent != DrawFundedDistinctBasicIntent.WenyiPeach || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || parent.Id != need.ParentFrameId || need.RequestFrameId != parent.Id || parent.InstructionIndex != need.Cursor || !_characterHuanShuJunOlOperations.IsWenyiPeachRequest(parent) || !_characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(parent, out var actor, out var target) || actor != source.OwnerSeat || original.PlayerSeat != actor || need.FixedTargetSeat != target || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId != original.PromptId)
            throw new InvalidOperationException("Wenyi cannot reserve a changed draw-funded Peach request.");
        _characterHuanShuJunOlOperations.PrepareWenyiSuppliedPeach(parent, actor, [], source);
    }

    internal bool WenyiDrawFundedNeedStillLegal(DrawFundedDistinctBasicFrame paid) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.Payment.ParentFrameId)is ProgramSkillFrame parent && WenyiDrawFundedParentMatches(parent, paid.Payment) && WenyiPeachUseLegal(paid.Payment.Source.OwnerSeat, paid.Payment.TargetSeat!.Value, [], paid.Payment.Source);
    internal void ExecuteWenyiDrawFundedPeach(DrawFundedDistinctBasicFrame paid)
    {
        var p = paid.Payment;
        if (!_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || !WenyiDrawFundedParentMatches(parent, p))
            throw new InvalidOperationException("Wenyi lost its once-paid native draw supplier.");
        if (!WenyiPeachUseLegal(p.Source.OwnerSeat, p.TargetSeat!.Value, [], p.Source))
        {
            _characterHuanShuJunOlOperations.FinishUnissuedWenyiPeach(parent);
            return;
        }

        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[p.TargetSeat.Value], new(0, CardKind.Peach, Suit.None, 0), "桃", CardKind.Peach, conversionSource: p.Source, physicalCards: [], drawFundedPayment: p);
    }

    internal bool TryCancelWenyiPeachSupply(long? parentId)
    {
        if (parentId is null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != parentId || f.HuanShuJun is not { Paid: true, Stage: HuanShuJunStage.NativeIssuing, Kind: CardKind.Peach, Materials.Count: 0 })
            return false;
        _characterHuanShuJunOlOperations.FinishUnissuedWenyiPeach(f);
        return true;
    }

    internal int[] WenyiRequestedDeckPeachTargets(RequestedDeckBasicFrame view, Card card) => view.Receipt.Intent == RequestedDeckBasicIntent.WenyiPeach && card.Kind == CardKind.Peach && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == view.Receipt.OwnerFrameId)is ProgramSkillFrame parent && parent.Id == view.Receipt.RequestFrameId && parent.InstructionIndex == view.Receipt.Cursor && _characterHuanShuJunOlOperations.IsWenyiPeachRequest(parent) && _characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(parent, out var actor, out var target) && actor == view.Receipt.ActorSeat && WenyiPeachUseLegal(actor, target, [card]) ? [target] : [];
    internal void ExecuteWenyiRequestedDeckPeach(Card card, IReadOnlyList<int> targets)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || !_characterHuanShuJunOlOperations.IsWenyiPeachRequest(parent) || !_characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(parent, out var actor, out var target) || !targets.SequenceEqual([target]) || parent.RequestedDeckBasicMaterial is not { Intent: RequestedDeckBasicIntent.WenyiPeach, Claiming: true } receipt || receipt.OwnerFrameId != parent.Id || receipt.RequestFrameId != parent.Id || receipt.Cursor != parent.InstructionIndex || receipt.ActorSeat != actor || receipt.SelectedCardId != card.Id || receipt.SelectedKind != CardKind.Peach || card.Kind != CardKind.Peach || !_coreRequestedDeckBasicOwnershipOperations.HasValidRequestedDeckBasicMaterial(parent) || !WenyiPeachUseLegal(actor, target, [card]))
            throw new InvalidOperationException("Wenyi lost its exact privately selected native DrawPile Peach.");
        _characterHuanShuJunOlOperations.PrepareWenyiSuppliedPeach(parent, actor, [card], null);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], card, "桃", CardKind.Peach, physicalCards: [card]);
    }

    internal bool WenyiEquipmentSlotPaymentRidesOn(ProgramSkillFrame f, ResolutionFrame child) => child is ProgramSkillFrame { EquipmentSlotNamedUse: { Intent: EquipmentSlotNamedUseIntent.WenyiPeach } d } slot && d.OriginalParentId == f.Id && slot.OwnerSeat == f.OwnerSeat && d.EffectiveKind == CardKind.Peach && d.ResponseChoice is { Cards.Count: 0, Targets: [var target] } choice && IsReservedWenyiPeachSupply(f, slot.OwnerSeat, target, d.Source) && d.TargetSignature == CharacterWoLongFengChuOlRuntime.EquipmentSlotNamedTargetSignature(null, choice) && d.OriginalDecision.PlayerSeat == slot.OwnerSeat && d.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && d.OriginalDecision.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice]));
    internal bool ResumeWenyiEquipmentSlotPeach(EquipmentSlotNamedUseDraft draft)
    {
        if (draft.Intent != EquipmentSlotNamedUseIntent.WenyiPeach)
            return false;
        if (draft.Payment is not { } paid || draft.OriginalParentId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || parent.Id != parentId || draft.ResponseChoice is not { Cards.Count: 0, Targets: [var target] } choice || !IsReservedWenyiPeachSupply(parent, paid.Source.OwnerSeat, target, paid.Source) || paid.Intent != draft.Intent || paid.EffectiveKind != CardKind.Peach || paid.Source != draft.Source || paid.GameplayHash != draft.GameplayHash || paid.OriginalParentId != parentId || paid.TargetSignature != draft.TargetSignature || paid.TargetSignature != CharacterWoLongFengChuOlRuntime.EquipmentSlotNamedTargetSignature(null, choice) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<EquipmentSlotNamedUsePaidEvent>().Count(e => e.Payment == paid) != 1)
            throw new InvalidOperationException("Wenyi lost the exact completed equipment-slot supplier invoice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!WenyiPeachUseLegal(paid.Source.OwnerSeat, target, [], paid.Source))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseCancelledEvent(paid.FrameId));
            _characterHuanShuJunOlOperations.FinishUnissuedWenyiPeach(parent);
            return true;
        }

        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(_coreMatchParticipantsPlayersAccess._players[paid.Source.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target], new(0, CardKind.Peach, Suit.None, 0), "桃", CardKind.Peach, conversionSource: paid.Source, physicalCards: []);
        return true;
    }

    internal bool TryGetWenyiWeijiePeachNeed(ProgramSkillFrame f, int provider, out int target)
    {
        target = f.HuanShuJun?.TargetSeat ?? -1;
        return f.OwnerSeat == provider && f.HuanShuJun is { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid, Paid: false, Stage: HuanShuJunStage.WenyiPeachRequest } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _characterHuanShuJunOlOperations.WenyiHpRoot(f);
    }

    internal bool WenyiWeijiePeachLegal(WeijieNeed need, int target)
    {
        if (need.Intent != WeijieIntent.WenyiPeach || need.ActorSeat != need.ProviderSeat || need.RequesterSeat is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == need.OwnerFrameId)is not ProgramSkillFrame parent || need.RequestFrameId != parent.Id || need.InstructionIndex != parent.InstructionIndex || need.RequestOp != SkillProgramEffectOp.ResolveWenyiHealthOneAid || !need.Targets.SequenceEqual([target]) || !TryGetWenyiWeijiePeachNeed(parent, need.ActorSeat, out var originalTarget) || originalTarget != target || !WenyiPeachUseLegal(need.ActorSeat, target, []))
            return false;
        if (_characterHuanShuJunOlOperations.IsWenyiPeachRequest(parent))
            return true;
        return _characterGuoTuOlOperations.ActiveWeijieDraft()is { Weijie: { Paid: { } paid } } && CharacterGuoTuOlRuntime.SameWeijieNeed(paid.Need, need) && _characterHuanShuJunOlOperations.IsWenyiPeachRequestAfterPaidSupply(parent, paid);
    }

    internal void ExecuteWenyiWeijiePeach(WeijiePaidOrigin paid)
    {
        if (paid.Need.Intent != WeijieIntent.WenyiPeach || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || paid.Need.OwnerFrameId != parent.Id || !_characterHuanShuJunOlOperations.IsWenyiPeachRequestAfterPaidSupply(parent, paid) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == paid.AttemptId && e.Matched && e.NativeLegal) != 1)
            throw new InvalidOperationException("Wenyi lost its successful once-paid Weijie Peach supplier.");
        if (!WenyiPeachUseLegal(paid.Need.ActorSeat, paid.TargetSeat, []))
        {
            _characterHuanShuJunOlOperations.PublishHuanShuJun(parent);
            return;
        }

        _characterHuanShuJunOlOperations.PrepareWenyiSuppliedPeach(parent, paid.Need.ActorSeat, [], paid.Source, paidWeijie: paid);
        _characterGuoTuOlOperations.DispatchWeijieRecovery(paid, false);
    }

    internal void RepublishWenyiAfterWeijie(WeijieReceipt receipt)
    {
        var need = receipt.Need;
        if (need.Intent != WeijieIntent.WenyiPeach || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || need.OwnerFrameId != parent.Id || need.RequestFrameId != parent.Id || parent.InstructionIndex != need.InstructionIndex || !TryGetWenyiWeijiePeachNeed(parent, need.ActorSeat, out var target) || !need.Targets.SequenceEqual([target]))
            throw new InvalidOperationException("Wenyi's failed supplier lost its original unconsumed request.");
        if (_characterHuanShuJunOlOperations.IsWenyiPeachRequest(parent))
            _characterHuanShuJunOlOperations.PublishHuanShuJun(parent);
        else
            _characterHuanShuJunOlOperations.FinishHuanShuJun(parent);
    }
}
