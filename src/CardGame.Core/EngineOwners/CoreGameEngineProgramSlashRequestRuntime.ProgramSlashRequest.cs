#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSlashRequestRuntime
{
    internal SkillProgramStepOutcome RequestProgramSlashByTarget(ProgramSkillFrame frame, int targetSeat, string resultBind, bool keepCurrentRequest = false)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.ChoiceBindings.Any(binding => binding.Name == resultBind))
            throw new InvalidOperationException("A named program choice cannot be answered twice.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var user = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!owner.IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "技能拥有者已失效，技能剩余结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(user.Seat, CardKind.Slash) || _characterHuCheErAnniversaryOperations.HuCheErSlashBan(user.Seat, CardKind.Slash))
        {
            CommitProgramChoiceResult(frame.Id, resultBind, RequestSlashByTargetProgramOperationDescriptor.DeclinedOption, user.Seat, "没有可用的【杀】。");
            return SkillProgramStepOutcome.Continue;
        }

        var slashes = _corePhysicalOwnedCardQueriesOperations.GetHand(user).Concat(_coreRequestedDeckBasicsOperations.SelectedRequestedDeckBasicCards(user)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(user)).Where(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) && !_coreTurnHandCategoryRestrictionsOperations.IsTurnHandCategoryRestricted(user, card) && !_coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(user.Seat, card.Id)).ToArray();
        var pairChoices = _coreRoundDistinctBasicRequestsOperations.RoundDistinctBasicRequestedSlashChoices(active, user.Seat, [frame.OwnerSeat], resultBind);
        var cardlessChoices = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramSlashChoices(active, user.Seat, [frame.OwnerSeat], resultBind).Concat(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(active, user.Seat, [frame.OwnerSeat], resultBind)).Concat(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(active, user.Seat, [frame.OwnerSeat], resultBind)).ToList();
        if (!keepCurrentRequest && slashes.Length == 0 && pairChoices.Count == 0 && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(user, CardKind.Slash) && !_coreChainedStateBasicConversionsOperations.HasChainedStateBasicProgramSlash(user, [frame.OwnerSeat]) && !_coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicProgramSlash(user, [frame.OwnerSeat]) && !_coreRoundOnceCardlessProgramRequestsOperations.HasRoundOnceCardlessProgramSlash(user, [frame.OwnerSeat]) && !_characterMaXiuMaTieOlOperations.HasKenshangProgramSlash(user, [frame.OwnerSeat]) && !_characterChengJiChengCuiOlOperations.HasTousuiProgramSlash(user, [frame.OwnerSeat]) && !_characterGuoTuOlOperations.HasWeijieProgramSlash(user, [frame.OwnerSeat]) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengProgramSlash(user, [frame.OwnerSeat]) && !(_characterLiuZhangOlOperations.HasZonghuProgramSlash(user, [frame.OwnerSeat]) || _characterHuanShuJunOlOperations.HasHuanpeiProgramSlash(user, [frame.OwnerSeat])))
        {
            CommitProgramChoiceResult(frame.Id, resultBind, RequestSlashByTargetProgramOperationDescriptor.DeclinedOption, user.Seat, "没有可用的【杀】。");
            return SkillProgramStepOutcome.Continue;
        }

        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var choices = slashes.Select(card => new PromptChoice(new ChoiceId($"program-request-slash.frame-{(active.Id)}.card-{(card.Id)}"), $"使用【{(card.DisplayName)}】攻击 {(owner.Name)}。", [card.Id], [frame.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "request-slash", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind, ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        choices.AddRange(pairChoices);
        choices.AddRange(cardlessChoices);
        choices.Add(new PromptChoice(new ChoiceId($"program-request-slash.frame-{(active.Id)}.decline"), $"不使用【杀】。", [], [], new Dictionary<string, string> { ["program-action"] = "request-slash-decline", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, user.Seat, $"【{(skill.Name)}】需对 {(owner.Name)} 使用一张【杀】，否则其弃置你的一张牌。", pairChoices.Count == 0 ? slashes.Select(card => card.Id).Order().ToArray() : slashes.Select(card => card.Id).Concat(pairChoices.SelectMany(choice => choice.Cards)).Distinct().Order().ToArray(), [frame.OwnerSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = user.Seat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, skill.Name, $"{(skill.Name)} · 是否使用【杀】", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = user.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveProgramRequestSlashChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The slash request lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.RequestSlashByTarget || effect.ResultBind is not { } resultBind || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("result-bind") != resultBind)
            throw new InvalidOperationException("The slash request does not match its suspended instruction.");
        var userSeat = frame.SelectedTargetSeats.Single();
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != userSeat)
            throw new InvalidOperationException("The slash request responder changed while suspended.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[userSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "挑衅参与者或技能实例已失效，技能剩余步骤取消。");
            return;
        }

        var action = selected.Parameters.GetValueOrDefault("program-action");
        if (action != "request-slash-decline" && (_characterPangJiOlOperations.IsBiguoCategoryForbidden(userSeat, CardKind.Slash) || _characterHuCheErAnniversaryOperations.HuCheErSlashBan(userSeat, CardKind.Slash)))
            throw new InvalidOperationException("The requested new Slash is forbidden by the active type restriction.");
        if (selected.Parameters.ContainsKey("round-distinct-basic-request"))
        {
            var selection = _coreRoundDistinctBasicRequestsOperations.RequireRoundDistinctBasicRequestedSlash(frame, decision, selected, userSeat, [frame.OwnerSeat], resultBind);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            CommitProgramChoiceResult(frame.Id, resultBind, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, userSeat, $"使用【{(CardCatalog.Get(selection.OutputKind).DisplayName)}】。");
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[userSeat], _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], selection.Cards[0], selection.OutputKind, userSeat, physicalCards: selection.Cards, countsTowardSlashLimit: false, conversionSource: selection.Source, programSkillCardUseFrameId: frame.Id);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (action == "request-slash-decline")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The decline branch must not name a card or target.");
            CommitProgramChoiceResult(frame.Id, resultBind, RequestSlashByTargetProgramOperationDescriptor.DeclinedOption, userSeat, "不使用【杀】。");
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (action != "request-slash" || !int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var cardId) || selected.Cards.Count != 1 || selected.Cards[0] != cardId || selected.Targets.Count != 1 || selected.Targets[0] != frame.OwnerSeat)
            throw new InvalidOperationException("The slash request answer is malformed.");
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = location.Zone == CardZoneKind.Hand && location.OwnerSeat == userSeat || _coreRequestedDeckBasicsOperations.IsSelectedRequestedDeckBasicMaterial(userSeat, cardId) || _coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(_coreMatchParticipantsPlayersAccess._players[userSeat]).Any(c => c.Id == cardId) ? _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(item => item.Id == cardId) : null;
        if (card is null || !CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || _coreTurnHandCategoryRestrictionsOperations.IsTurnHandCategoryRestricted(_coreMatchParticipantsPlayersAccess._players[userSeat], card) || _coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(userSeat, card.Id))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "所选【杀】已离开用牌者手牌，技能剩余结算已取消。");
            return;
        }

        CommitProgramChoiceResult(frame.Id, resultBind, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, userSeat, $"使用【{(card.DisplayName)}】。");
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[userSeat], _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card, card.Kind, userSeat, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestSlashByTarget)]
    internal PromptChoice SelectAiProgramRequestSlash(PendingDecision decision, ProgramSkillFrame frame)
    {
        // A forced Slash is worth more than a guaranteed one-card loss; prefer the
        // first physical Slash in card-id order so the answer stays deterministic.
        return decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action")is "request-slash" or "round-once-cardless-request") ?? decision.Choices.First();
    }

    internal int[] GetNearestLivingCharacterSeats(int seat)
    {
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != seat).Select(player => (Seat: player.Seat, Distance: _coreNativeRuleQueriesOperations.GetCombatDistance(seat, player.Seat))).ToArray();
        if (candidates.Length == 0)
            return[];
        var minimum = candidates.Min(candidate => candidate.Distance);
        return candidates.Where(candidate => candidate.Distance == minimum).Select(candidate => candidate.Seat).Order().ToArray();
    }

    internal SkillProgramStepOutcome RequestProgramSlashByNearest(ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "技能拥有者已失效，技能剩余结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var participant = _coreMatchParticipantsPlayersAccess._players[participantSeat];
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(participantSeat, CardKind.Slash))
            return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, participantSeat, hpAmount);
        var nearest = GetNearestLivingCharacterSeats(participantSeat);
        var slashes = _corePhysicalOwnedCardQueriesOperations.GetHand(participant).Concat(_coreRequestedDeckBasicsOperations.SelectedRequestedDeckBasicCards(participant)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(participant)).Where(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind)).OrderBy(card => card.Id).ToArray();
        var pairChoices = _coreRoundDistinctBasicRequestsOperations.RoundDistinctBasicRequestedSlashChoices(active, participantSeat, nearest);
        var cardlessChoices = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramSlashChoices(active, participantSeat, nearest).Concat(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(active, participantSeat, nearest)).Concat(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(active, participantSeat, nearest)).ToList();
        if (nearest.Length == 0 || !keepCurrentRequest && slashes.Length == 0 && pairChoices.Count == 0 && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(participant, CardKind.Slash) && !_coreChainedStateBasicConversionsOperations.HasChainedStateBasicProgramSlash(participant, nearest) && !_coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicProgramSlash(participant, nearest) && !_coreRoundOnceCardlessProgramRequestsOperations.HasRoundOnceCardlessProgramSlash(participant, nearest) && !_characterMaXiuMaTieOlOperations.HasKenshangProgramSlash(participant, nearest) && !_characterChengJiChengCuiOlOperations.HasTousuiProgramSlash(participant, nearest) && !_characterGuoTuOlOperations.HasWeijieProgramSlash(participant, nearest) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengProgramSlash(participant, nearest) && !(_characterLiuZhangOlOperations.HasZonghuProgramSlash(participant, nearest) || _characterHuanShuJunOlOperations.HasHuanpeiProgramSlash(participant, nearest)))
            return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, participantSeat, hpAmount);
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var choices = new List<PromptChoice>();
        foreach (var card in slashes)
            foreach (var seat in nearest)
                choices.Add(new PromptChoice(new ChoiceId($"program-request-slash-nearest.frame-{(active.Id)}.card-{(card.Id)}.seat-{(seat)}"), $"使用【{(card.DisplayName)}】攻击 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", [card.Id], [seat], new Dictionary<string, string> { ["program-action"] = "request-slash-nearest", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["seat"] = participantSeat.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target-seat"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        choices.AddRange(pairChoices);
        choices.AddRange(cardlessChoices);
        choices.Add(new PromptChoice(new ChoiceId($"program-request-slash-nearest.frame-{(active.Id)}.decline"), $"不使用【杀】，失去{(hpAmount)}点体力。", [], [], new Dictionary<string, string> { ["program-action"] = "request-slash-nearest-decline", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["seat"] = participantSeat.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, participant.Seat, $"【{(skill.Name)}】需对距离最近的角色使用一张【杀】，否则失去{(hpAmount)}点体力。", pairChoices.Count == 0 ? slashes.Select(card => card.Id).Order().ToArray() : slashes.Select(card => card.Id).Concat(pairChoices.SelectMany(choice => choice.Cards)).Distinct().Order().ToArray(), nearest, frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = participant.Seat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, skill.Name, $"{(skill.Name)} · 是否使用【杀】", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = participant.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveProgramRequestSlashByNearestChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The nearest-character slash request lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.RequestSlashByNearest || !int.TryParse(selected.Parameters.GetValueOrDefault("seat"), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var participantSeat) || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The nearest-character slash request does not match its suspended instruction.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != participantSeat)
            throw new InvalidOperationException("The nearest-character slash request responder changed while suspended.");
        var action = selected.Parameters.GetValueOrDefault("program-action");
        if (action != "request-slash-nearest-decline" && _characterPangJiOlOperations.IsBiguoCategoryForbidden(participantSeat, CardKind.Slash))
            throw new InvalidOperationException("The requested new Slash is forbidden by the active type restriction.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[participantSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "乱武参与者或技能实例已失效，技能剩余步骤取消。");
            return;
        }

        if (selected.Parameters.ContainsKey("round-distinct-basic-request"))
        {
            var selection = _coreRoundDistinctBasicRequestsOperations.RequireRoundDistinctBasicRequestedSlash(frame, decision, selected, participantSeat, GetNearestLivingCharacterSeats(participantSeat));
            var pairTargetSeat = selected.Targets[0];
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[participantSeat], _coreMatchParticipantsPlayersAccess._players[pairTargetSeat], selection.Cards[0], selection.OutputKind, participantSeat, physicalCards: selection.Cards, countsTowardSlashLimit: false, conversionSource: selection.Source, programSkillCardUseFrameId: frame.Id);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (action == "request-slash-nearest-decline")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The decline branch must not name a card or target.");
            if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, participantSeat, effect.Amount) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (action != "request-slash-nearest" || !int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var cardId) || !int.TryParse(selected.Parameters.GetValueOrDefault("target-seat"), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var targetSeat) || selected.Cards.Count != 1 || selected.Cards[0] != cardId || selected.Targets.Count != 1 || selected.Targets[0] != targetSeat)
            throw new InvalidOperationException("The nearest-character slash request answer is malformed.");
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = location == CardLocation.Hand(participantSeat) || _coreRequestedDeckBasicsOperations.IsSelectedRequestedDeckBasicMaterial(participantSeat, cardId) || _coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(_coreMatchParticipantsPlayersAccess._players[participantSeat]).Any(c => c.Id == cardId) ? _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(item => item.Id == cardId) : null;
        if (card is null || !CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || !GetNearestLivingCharacterSeats(participantSeat).Contains(targetSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "所选【杀】或最近角色已失效，技能剩余结算已取消。");
            return;
        }

        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[participantSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], card, card.Kind, participantSeat, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestSlashByNearest)]
    internal PromptChoice SelectAiProgramRequestSlashByNearest(PendingDecision decision) => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action")is "request-slash-nearest" or "round-once-cardless-request") ?? decision.Choices.First();
    internal void CommitProgramChoiceResult(long frameId, string bind, string optionId, int chooserSeat, string label)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(bind, optionId, chooserSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramOptionChosenEvent(frameId, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, bind, optionId, chooserSeat, label));
    }
}
