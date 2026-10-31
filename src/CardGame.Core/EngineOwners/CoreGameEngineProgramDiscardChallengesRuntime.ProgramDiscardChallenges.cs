#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDiscardChallengesRuntime
{

    [ProgramAiChoice(SkillProgramEffectOp.ChooseCategoryAlternativeDiscard, SkillProgramEffectOp.EscalatingDiscardOrDamage)]
    internal PromptChoice SelectAiProgramDiscardChallenge(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.DiscardChallenge!;
        var finish = decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "finish");
        if (finish is not null)
            return finish;
        var cards = decision.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "card").ToArray();
        if (draft.Mode == "escalating" && cards.Length + draft.SelectedIds.Count <= draft.PreviousCount)
            return decision.Choices.Single(c => c.Parameters.GetValueOrDefault("branch") == "damage");
        if (draft.Mode == "category" && !draft.ComplementOnly)
        {
            var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
            var primary = cards.FirstOrDefault(c => effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Cards[0])).Single(card => card.Id == c.Cards[0]).Kind, category)));
            if (primary is not null)
                return primary;
        }

        return cards.FirstOrDefault() ?? decision.Choices.First();
    }

    SkillProgramStepOutcome IDiscardChallengeProgramHost.ExecuteDiscardChallenge(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => ExecuteDiscardChallenge(effect, frame, targetSeat);
    internal SkillProgramStepOutcome ExecuteDiscardChallenge(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat)
    {
        var escalating = effect.Op == SkillProgramEffectOp.EscalatingDiscardOrDamage;
        var draft = frame.DiscardChallenge ?? new ProgramDiscardChallengeDraft(escalating ? "escalating" : "category", targetSeat, 0, 0, -1, false, []);
        if (escalating)
        {
            var cursor = draft.Cursor;
            while (cursor < _coreMatchDefinitionPlayerCountAccess._playerCount - 1 && !_coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(frame.OwnerSeat, cursor + 1)].IsAlive)
                cursor++;
            if (cursor >= _coreMatchDefinitionPlayerCountAccess._playerCount - 1)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = null });
                return SkillProgramStepOutcome.Continue;
            }

            draft = draft with
            {
                Cursor = cursor,
                ChooserSeat = _coreTableOrderOperations.TableStepSeat(frame.OwnerSeat, cursor + 1)
            };
        }
        else if (draft.Remaining == 0 || !_coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsAlive)
        {
            frame = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(frame);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = null });
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = draft });
        PublishDiscardChallenge(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), effect);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishDiscardChallenge(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var draft = frame.DiscardChallenge!;
        var choices = new List<PromptChoice>();
        foreach (var zone in effect.Zones)
            foreach (var card in _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, draft.ChooserSeat)))
            {
                if (_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(draft.ChooserSeat, card, new(zone, draft.ChooserSeat), OwnedCardMoveIntent.Discard) || draft.SelectedIds.Contains(card.Id) || draft.Mode == "category" && draft.ComplementOnly && effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(card.Kind, category)))
                    continue;
                choices.Add(new(new ChoiceId($"discard-challenge.card-{(card.Id)}"), $"{((draft.Mode == "escalating" ? "选择" : "弃置"))}【{(card.DisplayName)}】", [card.Id], [draft.ChooserSeat], new Dictionary<string, string> { ["program-action"] = "discard-challenge", ["branch"] = "card" }));
            }

        if (draft.Mode == "escalating")
        {
            if (draft.SelectedIds.Count > draft.PreviousCount)
                choices.Add(new(new ChoiceId("discard-challenge.finish"), $"弃置所选的{(draft.SelectedIds.Count)}张牌", [], [], new Dictionary<string, string> { ["program-action"] = "discard-challenge", ["branch"] = "finish" }));
            choices.Add(new(new ChoiceId("discard-challenge.damage"), $"受到{(effect.Amount)}点{((effect.DamageNature == DamageNature.Fire ? "火焰" : effect.DamageNature == DamageNature.Thunder ? "雷电" : ""))}伤害", [], [], new Dictionary<string, string> { ["program-action"] = "discard-challenge", ["branch"] = "damage" }));
        }
        else if (choices.Count == 0)
        {
            frame = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(frame);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.ChooserSeat, draft.Mode == "escalating" ? $"【{(skill.Name)}】弃置至少{(draft.PreviousCount + 1)}张牌，或受到伤害。" : $"【{(skill.Name)}】弃置一张锦囊，或依次弃置两张非锦囊。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [draft.ChooserSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.ToArray(),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("discard-challenge")]
    internal void ResolveProgramDiscardChallengeChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing discard challenge frame.");
        var draft = frame.DiscardChallenge ?? throw new InvalidOperationException("Missing discard challenge state.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (choice.Parameters.GetValueOrDefault("program-action") != "discard-challenge" || effect.Op is not (SkillProgramEffectOp.ChooseCategoryAlternativeDiscard or SkillProgramEffectOp.EscalatingDiscardOrDamage) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.ChooserSeat)
            throw new InvalidOperationException("Mismatched discard challenge choice.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "弃牌参与者或技能实例失效。");
            return;
        }

        if (branch == "card")
        {
            if (choice.Cards.Count != 1 || choice.Targets.Count != 1 || choice.Targets[0] != draft.ChooserSeat || draft.SelectedIds.Contains(choice.Cards[0]))
                throw new InvalidOperationException("Invalid discard challenge card.");
            var location = _cardZonesOperations._cardZones.GetLocation(choice.Cards[0]);
            if (location.OwnerSeat != draft.ChooserSeat || !effect.Zones.Contains(location.Zone))
                throw new InvalidOperationException("Discard card left its owner.");
            var card = _cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == choice.Cards[0]);
            var primary = effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(card.Kind, category));
            if (draft.Mode == "category" && draft.ComplementOnly && primary)
                throw new InvalidOperationException("Wrong discard category.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (draft.Mode == "escalating")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = draft with { SelectedIds = draft.SelectedIds.Append(card.Id).ToArray() } });
                PublishDiscardChallenge(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), effect);
                return;
            }

            var remaining = draft.Remaining < 0 ? (primary ? 0 : effect.MinimumValue - 1) : draft.Remaining - 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = draft with { Remaining = remaining, ComplementOnly = !primary }, ReexecuteParticipantInstruction = true, PendingMovementContinuation = new(draft.ChooserSeat, 0, null) });
            if (_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(draft.ChooserSeat, card, location, OwnedCardMoveIntent.Discard))
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "当前手牌不能自行弃置，费用未支付。");
                return;
            }

            var logicalReason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
            frame = _coreNativeLogicalDiscardsOperations.AppendFrameNativeLogicalDiscard(frame, draft.ChooserSeat, [card.Id], logicalReason);
            _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, logicalReason);
            if (remaining == 0)
                _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
            return;
        }

        if (draft.Mode != "escalating" || choice.Cards.Count != 0 || choice.Targets.Count != 0 || branch is not ("finish" or "damage") || branch == "finish" && draft.SelectedIds.Count <= draft.PreviousCount)
            throw new InvalidOperationException("Invalid discard challenge branch.");
        var physical = draft.SelectedIds.Select(id => (Id: id, Location: _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
        if (branch == "finish" && physical.Any(c => c.Location.OwnerSeat != draft.ChooserSeat || !effect.Zones.Contains(c.Location.Zone) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(draft.ChooserSeat, _cardZonesOperations._cardZones.CardsAt(c.Location).Single(card => card.Id == c.Id), c.Location, OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Staged discard card moved.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardChallenge = draft with { Cursor = draft.Cursor + 1, PreviousCount = branch == "finish" ? draft.SelectedIds.Count : 0, SelectedIds = [] }, ReexecuteParticipantInstruction = true, PendingMovementContinuation = branch == "finish" ? new(draft.ChooserSeat, 0, null) : null });
        if (branch == "damage")
        {
            if (_coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), draft.ChooserSeat, effect.Amount, nature: effect.DamageNature) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, draft.ChooserSeat, physical.Select(item => item.Id).ToArray(), new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }
}
