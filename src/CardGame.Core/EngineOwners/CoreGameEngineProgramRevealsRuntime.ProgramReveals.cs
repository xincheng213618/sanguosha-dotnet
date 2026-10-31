#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramRevealsRuntime
{
    internal SkillProgramStepOutcome RevealProgramTargetHandCard(ProgramSkillFrame frame, ProgramParticipantReference chooserRef, ProgramParticipantReference cardOwnerRef, string resultBind, SkillProgramRevealMode mode, IReadOnlyList<Suit> eligibleSuits, bool allowDecline)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.RevealCardSelection is not null || active.CardSetBindings.Any(binding => binding.Name == resultBind))
            throw new InvalidOperationException("A hand reveal cannot overwrite an existing draft or binding.");
        if (active.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("A hand reveal requires exactly one selected target.");
        var chooserSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, chooserRef);
        var holderSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, cardOwnerRef);
        if (chooserSeat != frame.OwnerSeat || holderSeat == frame.OwnerSeat && mode != SkillProgramRevealMode.SelfChoiceOtherwiseRandom && !(mode == SkillProgramRevealMode.Chooser && _characterLuMengHappyGodOperations.IsExactHappyGongxinSelfReveal(active)))
            throw new InvalidOperationException("A hand reveal must target another character's hand and be chosen by the skill owner.");
        var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holderSeat]).Select(card => card.Id).ToArray();
        if (candidates.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "目标没有手牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        // The chooser always views the whole hand; the suit filter only limits
        // which cards may actually be revealed.
        var eligible = eligibleSuits.Count == 0 ? candidates : candidates.Where(cardId => eligibleSuits.Contains(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[holderSeat], _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(holderSeat)).Single(card => card.Id == cardId)))).ToArray();
        if ((mode is SkillProgramRevealMode.Random or SkillProgramRevealMode.SelfChoiceOtherwiseRandom) && eligible.Length == 0 || mode == SkillProgramRevealMode.Chooser && eligible.Length == 0 && !allowDecline)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "目标没有符合条件的牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (mode == SkillProgramRevealMode.Random || mode == SkillProgramRevealMode.SelfChoiceOtherwiseRandom && holderSeat != chooserSeat)
        {
            CommitProgramHandReveal(frame.Id, holderSeat, resultBind, [eligible[_randomOperations._random.Next(eligible.Length)]]);
            return SkillProgramStepOutcome.Continue;
        }

        active = active with
        {
            RevealCardSelection = new(holderSeat, chooserSeat, resultBind, Array.AsReadOnly(candidates), Array.AsReadOnly(eligible), allowDecline)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramRevealCardSelection(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramRevealCardSelection(ProgramSkillFrame frame)
    {
        var draft = frame.RevealCardSelection ?? throw new InvalidOperationException("Missing reveal draft.");
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var hand = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(draft.HolderSeat));
        var choices = draft.EligibleCardIds.Select(cardId =>
        {
            var card = hand.Single(item => item.Id == cardId);
            return new PromptChoice(new ChoiceId($"program-reveal.frame-{(frame.Id)}.card-{(cardId)}"), $"观看并展示【{(card.DisplayName)}】（{(card.RankText)}）。", [cardId], [], new Dictionary<string, string> { ["program-action"] = "reveal-target-hand-card", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind });
        }).ToList();
        if (draft.AllowDecline)
            choices.Add(new PromptChoice(new ChoiceId($"program-reveal.frame-{(frame.Id)}.decline"), "不展示其手牌。", [], [], new Dictionary<string, string> { ["program-action"] = "reveal-target-hand-card-decline", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, draft.ChooserSeat, $"请选择要展示的 {(_coreMatchParticipantsPlayersAccess._players[draft.HolderSeat].Name)} 的一张手牌。", draft.CandidateCardIds.ToArray(), [], draft.ChooserSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.HolderSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 观看手牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveProgramRevealCardSelection(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The reveal choice lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var draft = frame.RevealCardSelection ?? throw new InvalidOperationException("The reveal choice lost its suspended draft.");
        var decline = selected.Parameters.GetValueOrDefault("program-action") == "reveal-target-hand-card-decline";
        if (effect.Op != SkillProgramEffectOp.RevealTargetHandCard || effect.ResultBind != draft.ResultBind || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("result-bind") != draft.ResultBind || decline && (!draft.AllowDecline || selected.Cards.Count != 0) || !decline && (selected.Cards.Count != 1 || !draft.EligibleCardIds.Contains(selected.Cards[0])))
            throw new InvalidOperationException("The reveal choice does not match its suspended instruction.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.HolderSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || draft.CandidateCardIds.Any(cardId => _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(draft.HolderSeat)))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "观看参与者或技能实例已失效，技能结算已取消。");
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RevealCardSelection = null });
        CommitProgramHandReveal(frame.Id, draft.HolderSeat, draft.ResultBind, decline ? [] : [selected.Cards[0]]);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CommitProgramHandReveal(long frameId, int holderSeat, string resultBind, IReadOnlyList<int> cardIds)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (cardIds.Count == 0)
        {
            // 攻心-style decline: the viewing happened, but nothing is revealed,
            // so downstream suit-gated effects see an empty private binding.
            _coreProgramLifecycleOperations.SetProgramCardSet(frameId, resultBind, [], SkillProgramCardSetVisibility.Private, []);
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 观看了 {(_coreMatchParticipantsPlayersAccess._players[holderSeat].Name)} 的手牌，但没有展示任何牌。", frame.OwnerSeat, holderSeat);
            return;
        }

        _coreProgramLifecycleOperations.SetProgramCardSet(frameId, resultBind, cardIds, SkillProgramCardSetVisibility.Private, cardIds.Select(_ => CardLocation.Hand(holderSeat)).ToArray());
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(frame, resultBind);
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 展示了 {(_coreMatchParticipantsPlayersAccess._players[holderSeat].Name)} 的一张手牌。", frame.OwnerSeat, holderSeat);
    }
}
