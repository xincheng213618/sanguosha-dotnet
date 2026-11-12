#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineChainedStateBasicConversionsRuntime
{
    internal const string ChainedStateBasicMarker = RuntimeSkills.RuntimeRules_Core_ChainedStateBasicConversions.ChainedStateBasicMarker;
    internal bool CanPayChainedStateBasic(CharacterState actor, bool desired) => actor.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && actor.IsChained != desired && _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(actor, desired || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.ForceChained)) == desired;
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> ChainedStateBasicRules(CharacterState actor, CardKind? output = null, bool dodge = false)
    {
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs.Where(r => r.ChainedStateCost.HasValue))
                if ((output is null || rule.OutputKind == output) && (dodge ? rule.ForResponse && rule.OutputKind == CardKind.Dodge : rule.ForPlay && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind)) && rule.UseOnly && rule.InputCount == 0 && rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor)) && CanPayChainedStateBasic(actor, rule.ChainedStateCost!.Value) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, rule.OutputKind, CardActionType.Use) && _characterZhouYuOlMouOperations.IsMouGuquCardlessAllowed(actor, rule.OutputKind, dodge))
                    yield return (new(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId), rule, instance.Program.GameplayHash);
    }

    internal IReadOnlyList<LegalAction> ChainedStateBasicNativePlayActions(CharacterState actor, CardConversionSource source, CardKind kind)
    {
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || (_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat != actor.Seat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _slashRequestPermissions.Declaration(actor.Seat) is not null) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor) || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return[];
        var card = new Card(0, kind, Suit.None, 0);
        var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: kind);
        var actions = targets.Select(t => new LegalAction(LegalActionKind.Slash, 0, t.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"解除连环状态，视为对 {(t.Name)} 使用【{(card.DisplayName)}】"), kind) { ConversionSource = source }).ToList();
        _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, card.DisplayName, kind, source, null);
        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(actions, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
        return Array.AsReadOnly(actions.Select(a => a with { ChainedStateBasicUse = false }).ToArray());
    }

    internal IReadOnlyList<LegalAction> ChainedStateBasicPlayActions(CharacterState actor)
    {
        if ((_resolutionStackOperations._resolutionStack.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat != actor.Seat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _slashRequestPermissions.Declaration(actor.Seat) is not null) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor))
            return[];
        return Array.AsReadOnly(ChainedStateBasicRules(actor).SelectMany(r => ChainedStateBasicNativePlayActions(actor, r.Source, r.Rule.OutputKind)).ToArray());
    }

    internal bool TrySubmitChainedStateBasicPlay(PlayCardCommand command, out CommandResult result)
    {
        result = null !;
        if (command.CardId != 0 || command.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.ChainedStateCost is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = ChainedStateBasicPlayActions(actor).SingleOrDefault(a => a.ConversionSource == source && a.PlayedCardKind == command.PlayedCardKind && a.TargetCardId == command.TargetCardId && a.TargetSeats.SequenceEqual(command.TargetSeats ?? []));
        if (action is null || command.AdditionalConversionSources is { Count: > 0 } || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.PlayCard } original)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The chained-state use changed its published cost, source, name or native targets.");
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            BeginChainedStateBasic(actor, source, action.PlayedCardKind!.Value, action.TargetSeats, original, new(ChainedStateBasicIntent.Play, null, null, null, 0, action.TargetSeat));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    internal bool TryExecuteChainedStateBasicPlay(CharacterState actor, LegalAction action, ProgramSlashRequestAcceptance? programSlashRequest = null, ProgramSlashRequestIdentity? programSlashRequestIdentity = null, PendingDecision? originalDecision = null)
    {
        if (action.CardId != 0 || action.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.ChainedStateCost is null)
            return false;
        if (programSlashRequest is { } accepted &&
            (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == accepted.Identity.FrameId) is not ProgramSkillFrame parent ||
             !_programSlashRequests.OwnsAcceptance(parent, accepted, actor.Seat) || !action.TargetSeats.Contains(accepted.TargetSeat) ||
             action.PlayedCardKind is not { } requestedKind || !CoreNativeCardClassificationsRuntime.IsSlashCard(requestedKind)))
            throw new InvalidOperationException("A native Play action lost its original accepted Slash request.");
        if (programSlashRequest is null && programSlashRequestIdentity is { } identity &&
            (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == identity.FrameId) is not ProgramSkillFrame offeredParent ||
             !_programSlashRequests.OwnsIssuedIdentity(offeredParent, identity, actor.Seat) ||
             !_programSlashRequests.OwnsOffer(offeredParent, actor.Seat, action.TargetSeat ?? action.TargetSeats.FirstOrDefault(-1)) ||
             action.PlayedCardKind is not { } offeredKind || !CoreNativeCardClassificationsRuntime.IsSlashCard(offeredKind)))
            throw new InvalidOperationException("A native Play action lost its original unpaid Slash request.");
        if (programSlashRequest is null && programSlashRequestIdentity is null && !ChainedStateBasicPlayActions(actor).Any(a => a.ConversionSource == source && a.PlayedCardKind == action.PlayedCardKind && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("An AI chained-state use changed its actual legal action.");
        if (originalDecision is null && (programSlashRequest is not null || programSlashRequestIdentity is not null))
            throw new InvalidOperationException("A requested native Play cost requires its actual original decision.");
        var original = originalDecision ?? _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? new PendingDecision(DecisionKind.PlayCard, actor.Seat, "已发布合法用牌动作", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Revision = _coreCommandSessionQueriesOperations.Revision
        };
        _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = null;
        BeginChainedStateBasic(actor, source, action.PlayedCardKind!.Value, action.TargetSeats, original, new(ChainedStateBasicIntent.Play, null, null, null, 0, action.TargetSeat), programSlashRequest, programSlashRequestIdentity);
        return true;
    }

    internal GameEngine.ChainedStateBasicNeed? ChainedStateBasicContext(PendingDecision prompt, ResolutionFrame? suppliedTop = null)
    {
        var top = suppliedTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (top is null || top is ChainedStateBasicFrame or DrawFundedDistinctBasicFrame or RequestedDeckBasicFrame)
            return null;
        var ownerId = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        var parentAction = _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)?.Action?.ActionId;
        if (prompt.Kind == DecisionKind.RespondDodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null && top is ResponseWindowFrame && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == prompt.PlayerSeat && ownerId == attack.ResolutionId && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat], CardKind.Dodge))
            return new(ChainedStateBasicIntent.OwnSlashDodge, ownerId, top.Id, parentAction, attack.SuccessfulDodgeResponses, attack.SourceSeat);
        if (prompt.Kind == DecisionKind.RespondSlash && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } borrowed && borrowed.ResolutionId == ownerId && borrowed.WeaponOwnerSeat == prompt.PlayerSeat && _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat])is not null)
            return new(ChainedStateBasicIntent.BorrowedSword, ownerId, top.Id, parentAction, 0, borrowed.SlashTargetSeat);
        if (prompt.Kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } blade && blade.Attack.ResolutionId == ownerId && blade.Attack.SourceSeat == prompt.PlayerSeat && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack))
            return new(ChainedStateBasicIntent.Qinglong, ownerId, top.Id, parentAction, 0, blade.Attack.TargetSeat);
        if (prompt.Kind != DecisionKind.ProgramTrigger || top is not ProgramSkillFrame program)
            return null;
        if (_programSlashRequests.GetOffer(program, prompt.PlayerSeat) is not { } request)
            return null;
        var paused = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
        var intent = paused.Op switch
        {
            SkillProgramEffectOp.RequestSlashByTarget => ChainedStateBasicIntent.ProgramSlash,
            SkillProgramEffectOp.RequestSlashByNearest => ChainedStateBasicIntent.ProgramNearestSlash,
            SkillProgramEffectOp.RequestLegalSlashByNearest => ChainedStateBasicIntent.NearestLegalSlash,
            _ => ChainedStateBasicIntent.AssistedSlash
        };
        var targets = _programSlashRequests.GetTargets(program, prompt.PlayerSeat);
        return new(intent, program.Id, program.Id, null, program.InstructionIndex,
            request.Spec.TargetSource == ProgramSlashRequestTargetSource.FrozenCandidates && targets.Count == 1 ? targets[0] : null);
    }

    internal bool ChainedStateBasicNativeResponseLegal(CharacterState actor, GameEngine.ChainedStateBasicNeed need, CardKind kind, int? targetSeat, CardConversionSource? source = null)
    {
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return false;
        if (need.Intent == ChainedStateBasicIntent.OwnSlashDodge)
            return kind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == need.ParentFrameId && attack.SuccessfulDodgeResponses == need.Cursor && attack.SourceSeat == targetSeat;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || targetSeat is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, seat, Suit.None) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], kind, Suit.None))
            return false;
        if (need.Intent == ChainedStateBasicIntent.BorrowedSword)
            return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == need.ParentFrameId && b.WeaponOwnerSeat == actor.Seat && b.SlashTargetSeat == seat && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], kind, Suit.None, false, null, 0, []);
        if (need.Intent == ChainedStateBasicIntent.Qinglong)
            return _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == need.ParentFrameId && q.Attack.SourceSeat == actor.Seat && q.Attack.TargetSeat == seat && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], kind, null, false, Suit.None, 0);
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == need.ParentFrameId)is not ProgramSkillFrame program || program.InstructionIndex != need.Cursor)
            return false;
        var request = _programSlashRequests.GetIdentity(program);
        if (request is null || request.ActorSeat != actor.Seat || need.RequestFrameId != program.Id ||
            !_programSlashRequests.GetTargets(program, actor.Seat).Contains(seat))
            return false;
        if (program?.SlashRequest?.Acceptance is { } acceptedRequest && !_programSlashRequests.CanExecuteAcceptedUse(program, acceptedRequest))
            return false;
        if (request.Spec.CountsTowardSlashLimit && !_slashQuota.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[seat], _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source), kind, physicalCardIds: []))
            return false;
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: [], ignoreDistance: request.Spec.IgnoreDistance);
    }

    internal IReadOnlyList<PromptChoice> ChainedStateBasicResponseChoices(PendingDecision prompt, GameEngine.ChainedStateBasicNeed need)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var choices = new List<PromptChoice>();
        foreach (var(source, rule, _)in ChainedStateBasicRules(actor, dodge: need.Intent == ChainedStateBasicIntent.OwnSlashDodge))
            foreach (var target in (need.FixedTargetSeat is { } fixedTarget ? new[]
            {
                fixedTarget
            }

            : prompt.ValidTargetSeats.ToArray()).Where(t => ChainedStateBasicNativeResponseLegal(actor, need, rule.OutputKind, t, source)))
            {
                var p = new Dictionary<string, string>
                {
                    [ChainedStateBasicMarker] = need.Intent.ToString(),
                    ["output-kind"] = rule.OutputKind.ToString(),
                    ["original-frame-id"] = need.ParentFrameId!.Value.ToString(CultureInfo.InvariantCulture),
                    ["native-target-seat"] = target.ToString(CultureInfo.InvariantCulture)
                };
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
                choices.Add(new(new($"chained-state.{(need.Intent)}.{(need.ParentFrameId)}.{(source.BindingId)}.{(source.SkillInstanceId)}.{(target)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，{((rule.ChainedStateCost == true ? "进入" : "解除"))}连环状态，视为使用【{(CardCatalog.Get(rule.OutputKind).DisplayName)}】。", [], need.Intent == ChainedStateBasicIntent.OwnSlashDodge ? [] : [target], p));
            }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal PendingDecision? AddChainedStateBasicChoices(PendingDecision? prompt)
    {
        if (prompt is null || !_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started || prompt.Choices.Any(IsChainedStateBasicAnswer) || ChainedStateBasicContext(prompt)is not { } need)
            return prompt;
        var choices = ChainedStateBasicResponseChoices(prompt, need);
        return choices.Count == 0 ? prompt : prompt with
        {
            Choices = Array.AsReadOnly(prompt.Choices.Concat(choices).ToArray())
        };
    }

    internal IReadOnlyList<PromptChoice> ChainedStateBasicDodgeChoices(CharacterState actor)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge))
            return[];
        var prompt = new PendingDecision(DecisionKind.RespondDodge, actor.Seat, "真实杀防御", [], [], attack.SourceSeat);
        return ChainedStateBasicResponseChoices(prompt, new(ChainedStateBasicIntent.OwnSlashDodge, use.Id, _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault(r => r.ParentFrameId == use.Id)?.Id, use.Action?.ActionId, attack.SuccessfulDodgeResponses, attack.SourceSeat));
    }

    internal bool HasChainedStateBasicDodge(CharacterState actor) => ChainedStateBasicDodgeChoices(actor).Count > 0;
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> ChainedStateBasicForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => ChainedStateBasicRules(actor).Where(r => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, r.Rule.OutputKind, Suit.None) && (qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, r.Rule.OutputKind, null, false, Suit.None, 0) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, r.Rule.OutputKind, Suit.None, false, null, 0, [])));
    internal IReadOnlyList<PromptChoice> ChainedStateBasicForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong)
    {
        var need = new GameEngine.ChainedStateBasicNeed(qinglong ? ChainedStateBasicIntent.Qinglong : ChainedStateBasicIntent.BorrowedSword, originalId, _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault(r => r.ParentFrameId == originalId)?.Id ?? originalId, _coreCardUseLifecycleOperations.LifecycleCardUse(originalId)?.Action?.ActionId, 0, target.Seat);
        return ChainedStateBasicResponseChoices(new(qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash, actor.Seat, "原生杀使用请求", [], [target.Seat]), need);
    }

    internal bool HasChainedStateBasicProgramSlash(CharacterState actor, IEnumerable<int> targets) => ChainedStateBasicRules(actor).Any(r => targets.Any(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreMatchParticipantsPlayersAccess._players[t].IsAlive && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t, Suit.None) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], new(0, r.Rule.OutputKind, Suit.None, 0), effectiveKind: r.Rule.OutputKind, noEffectiveRank: true, physicalCardIds: [])));
    internal static bool IsChainedStateBasicAnswer(PromptChoice choice) => RuntimeSkills.RuntimeRules_Core_ChainedStateBasicConversions.IsChainedStateBasicAnswer(choice);
    internal bool IsChainedStateBasicAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && IsChainedStateBasicAnswer(c)) == true;
    internal static PendingDecision ChainedStateBasicNativeDecision(PendingDecision prompt) => RuntimeSkills.RuntimeRules_Core_ChainedStateBasicConversions.ChainedStateBasicNativeDecision(prompt);
    internal CommandResult SubmitChainedStateBasicAnswer(int actorSeat, PromptId promptId, ChoiceId choiceId)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.PlayerSeat != actorSeat || prompt.PromptId != promptId)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "连环状态费用必须回答当前精确提示。");
        var choice = prompt.Choices.SingleOrDefault(c => c.Id == choiceId && IsChainedStateBasicAnswer(c));
        return choice is null ? _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "该连环转换选择未发布。") : SubmitChainedStateBasicAnswer(actorSeat, prompt, choice);
    }

    internal CommandResult SubmitChainedStateBasicAnswer(int actorSeat, PendingDecision prompt, PromptChoice choice)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsHuman || prompt.PlayerSeat != actorSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId != prompt.PromptId || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || choice.Cards.Count != 0 || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || !int.TryParse(choice.Parameters.GetValueOrDefault("native-target-seat"), out var target) || ChainedStateBasicContext(prompt)is not { } need || !ChainedStateBasicResponseChoices(prompt, need).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The chained-state answer changed its human actor, source, real cost or original native need.");
        return _kernelCoreMainOperations.Accept(() =>
        {
            BeginChainedStateBasic(_coreMatchParticipantsPlayersAccess._players[actorSeat], CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice), kind, need.Intent == ChainedStateBasicIntent.OwnSlashDodge ? [target] : choice.Targets, prompt, need);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void BeginChainedStateBasic(CharacterState actor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets, PendingDecision original, GameEngine.ChainedStateBasicNeed need, ProgramSlashRequestAcceptance? programSlashRequest = null, ProgramSlashRequestIdentity? programSlashRequestIdentity = null)
    {
        var accepted = ChainedStateBasicRules(actor, kind, need.Intent == ChainedStateBasicIntent.OwnSlashDodge).SingleOrDefault(r => r.Source == source);
        if (accepted.Rule is null || original.PlayerSeat != actor.Seat || _resolutionStackOperations._resolutionStack.OfType<ChainedStateBasicFrame>().Any(f => f.Payment.OriginalPromptId == original.PromptId))
            throw new InvalidOperationException("A chained-state cost cannot accept an unavailable source or repay an original need.");
        if (programSlashRequest is null && programSlashRequestIdentity is { } identity)
        {
            var originalParent = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == identity.FrameId) as ProgramSkillFrame
                ?? throw new InvalidOperationException("A native Play cost lost its original Slash request.");
            if (!_programSlashRequests.OwnsIssuedIdentity(originalParent, identity, actor.Seat))
                throw new InvalidOperationException("A native Play cost cannot accept another issued request.");
            programSlashRequest = _programSlashRequests.Accept(originalParent, actor.Seat, need.FixedTargetSeat ?? targets.First());
        }
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var p = new ChainedStateBasicPayment(id, source, accepted.Hash, actor.IsChained, accepted.Rule.ChainedStateCost!.Value, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, need.Intent, kind, need.ParentFrameId, need.RequestFrameId, need.ParentActionId, targets.Count == 1 ? targets[0] : need.FixedTargetSeat, need.Cursor, original.PromptId, original.Revision) { RequestAcceptance = programSlashRequest };
        if (need.ParentFrameId is { } requestId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == requestId) is ProgramSkillFrame request && p.TargetSeat is { } target && _programSlashRequests.GetOffer(request, actor.Seat) is not null)
            p = p with { RequestAcceptance = _programSlashRequests.Accept(request, actor.Seat, target) };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(new ChainedStateBasicFrame(id, p, original, targets));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedStateBasicStartedEvent(p));
        // Persist the paid stage before any native state-change observer can run.
        _coreRuntimeOperations.ReplaceRuntimeTop(((ChainedStateBasicFrame)_resolutionStackOperations._resolutionStack.Last())with { Stage = ChainedStateBasicStage.StateChildren });
        actor.IsChained = _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(actor, p.DesiredChained || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.ForceChained));
        if (actor.IsChained != p.DesiredChained || actor.IsChained == p.WasChained)
            throw new InvalidOperationException("A chained-state cost must actually change to its accepted destination.");
        if (!p.WasChained && p.DesiredChained)
            _coreCharacterStateProgramsOperations.RecordCharacterStateChange(actor.Seat, SkillProgramTriggerWindow.CharacterEnteredChain, id);
        var change = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().SingleOrDefault(e => e.Change.ParentFrameId == id);
        p = p with
        {
            CharacterStateChangeId = change?.Change.Id
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(((ChainedStateBasicFrame)_resolutionStackOperations._resolutionStack.Last())with { Payment = p });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedStateBasicPaidEvent(p));
        ContinueChainedStateBasic(id);
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 80, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ChainedStateBasicFrame), 390)]
    internal void ContinueChainedStateBasic(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ChainedStateBasicFrame frame || frame.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (frame.Stage != ChainedStateBasicStage.StateChildren || frame.ActiveChildFrameId is not null || !ValidChainedStateBasicPayment(frame.Payment))
            throw new InvalidOperationException("A chained-state cost resumed before its exact paid state child.");
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.ChainedStateBasic))
            return;
        var legal = ChainedStateBasicOriginalNeedCurrent(frame);
        if (!legal)
        {
            CancelChainedStateBasic(frame);
            return;
        }

        _coreRuntimeOperations.PopResolutionFrame(id, ResolutionFrameKind.ChainedStateBasic);
        _coreChainedStateBasicReturnsOperations.ExecuteChainedStateBasicPaid(frame);
    }

    internal void BeginChainedStateBasicChild(long parentId, long childId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ChainedStateBasicFrame frame || frame.Id != parentId || frame.ActiveChildFrameId is not null || frame.Payment.CharacterStateChangeId != childId || !ValidChainedStateBasicPayment(frame.Payment))
            throw new InvalidOperationException("A chained-state observer lost its exact once-paid owner.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActiveChildFrameId = childId });
    }

    internal void ResumeChainedStateBasicState(ProgramLifecycleTriggerWindowFrame child)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ChainedStateBasicFrame parent || parent.ActiveChildFrameId != child.Id || child.ResumeProgramFrameId != parent.Id || child.CharacterStateContinuation != CharacterStateContinuation.ChainedStateBasic || child.Window != SkillProgramTriggerWindow.CharacterEnteredChain || child.OwnerSeat != parent.Payment.Source.OwnerSeat || parent.Payment.CharacterStateChangeId != child.Id || !ValidChainedStateBasicPayment(parent.Payment))
            throw new InvalidOperationException("A chained-state child returned to another payment or native need.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { ActiveChildFrameId = null });
        ContinueChainedStateBasic(parent.Id);
    }

    internal bool ValidChainedStateBasicPayment(ChainedStateBasicPayment p)
    {
        if (p.PaymentFrameId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.Source.OwnerSeat) || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || p.WasChained == p.DesiredChained || p.DesiredChained != (p.EffectiveKind == CardKind.Dodge) || p.Cursor < 0 || !Enum.IsDefined(p.Intent) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } definition || definition.GameplayHash != p.GameplayHash || definition.ViewAs.SingleOrDefault(v => v.Id == p.Source.BindingId)is not { InputCount: 0, UseOnly: true, ChainedStateCost: { } desired } rule || desired != p.DesiredChained || rule.OutputKind != p.EffectiveKind || (p.Intent == ChainedStateBasicIntent.OwnSlashDodge) != p.DesiredChained)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ChainedStateBasicStartedEvent>().Where(e => e.Payment.PaymentFrameId == p.PaymentFrameId).ToArray()is not [var started] || started.Payment != (p with
        {
            CharacterStateChangeId = null
        }

        ) || history.OfType<ChainedStateBasicPaidEvent>().Count(e => e.Payment == p) != 1)
            return false;
        var changes = history.OfType<CharacterStateChangedEvent>().Where(e => e.Change.ParentFrameId == p.PaymentFrameId).ToArray();
        return p.CharacterStateChangeId is { } changeId ? p.DesiredChained && changes is [var changed] && changed.Change.Id == changeId && changed.Change.TargetSeat == p.Source.OwnerSeat && changed.Change.Window == SkillProgramTriggerWindow.CharacterEnteredChain : changes.Length == 0;
    }

    internal bool ChainedStateBasicOriginalNeedCurrent(ChainedStateBasicFrame frame)
    {
        var p = frame.Payment;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || p.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || p.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return false;
        if (p.Intent == ChainedStateBasicIntent.Play && p.RequestAcceptance is { } acceptedPlay)
            return _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == acceptedPlay.Identity.FrameId) is ProgramSkillFrame playParent &&
                _programSlashRequests.OwnsAcceptance(playParent, acceptedPlay, actor.Seat, p.TargetSeat) &&
                ChainedStateBasicNativeResponseLegal(actor, new(p.Intent, playParent.Id, playParent.Id, null, playParent.InstructionIndex, p.TargetSeat), p.EffectiveKind, p.TargetSeat, p.Source);
        if (p.Intent == ChainedStateBasicIntent.Play)
            return (_resolutionStackOperations._resolutionStack.Count == 1 || _characterKeBiNengOlOperations.IsKeBiNengPingduanPaymentFrame(frame) || _characterKongRongMouOlOperations.Value.IsMouKrLiwenPaymentFrame(frame) || _characterLuSuMouOperations.Value.IsMouLuSuPaymentFrame(frame)) && ChainedStateBasicNativePlayActions(actor, p.Source, p.EffectiveKind).Any(a => a.TargetSeats.SequenceEqual(frame.TargetSeats));
        if (p.RequestAcceptance is { } acceptance)
            return _resolutionStackOperations._resolutionStack.Count >= 2 &&
                _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame acceptedParent &&
                acceptedParent.Id == p.ParentFrameId && acceptedParent.Id == p.RequestFrameId &&
                _programSlashRequests.OwnsAcceptance(acceptedParent, acceptance, actor.Seat, p.TargetSeat) &&
                ChainedStateBasicNativeResponseLegal(actor, new(p.Intent, p.ParentFrameId, p.RequestFrameId, p.ParentActionId, p.Cursor, p.TargetSeat), p.EffectiveKind, p.TargetSeat, p.Source);
        var request = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.RequestFrameId);
        if (request is null || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != request.Id || ChainedStateBasicContext(frame.OriginalDecision, request)is not { } current || current.Intent != p.Intent || current.ParentFrameId != p.ParentFrameId || current.RequestFrameId != p.RequestFrameId || current.ParentActionId != p.ParentActionId || current.Cursor != p.Cursor)
            return false;
        if (request is ProgramSkillFrame program)
        {
            var effect = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
            if (!_coreChainedStateBasicReturnsOperations.IsChainedStateBasicProgramSelection(program, effect, frame))
                return false;
        }

        return ChainedStateBasicNativeResponseLegal(actor, current, p.EffectiveKind, p.TargetSeat, p.Source);
    }

    internal void CancelChainedStateBasic(ChainedStateBasicFrame frame)
    {
        var p = frame.Payment;
        ProgramSkillFrame? failedOwner = null;
        if (p.RequestAcceptance is { } failedAcceptance)
        {
            failedOwner = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == failedAcceptance.Identity.FrameId) as ProgramSkillFrame
                ?? throw new InvalidOperationException("A paid Slash failure lost its original request frame.");
            _programSlashRequests.FailAcceptedUse(failedOwner, failedAcceptance, frame.Id);
        }
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id)
            _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ChainedStateBasic);

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedStateBasicCancelledEvent(p));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (failedOwner is not null && p.RequestAcceptance is { } acceptedFailure)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                return;
            var original = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == failedOwner.Id);
            if (acceptedFailure.Identity.Spec.FailureReturnPolicy == ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction)
                _coreRuntimeOperations.AdvanceRuntimeProgram(original.Id);
            else
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(original, "原用牌请求失败；已支付费用不撤回。");
            return;
        }

        if (p.ParentFrameId is { } pingduanParent && _characterKeBiNengOlOperations.TryCancelKeBiNengPingduanBasicPayment(pingduanParent))
            return;
        if (p.ParentFrameId is { } luSuParent && _characterLuSuMouOperations.Value.TryCancelMouLuSuPayment(luSuParent))
            return;
        if (p.ParentFrameId is { } liwenParent && _characterKongRongMouOlOperations.Value.TryCancelMouKrLiwenPayment(liwenParent))
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        if (p.Intent == ChainedStateBasicIntent.BorrowedSword && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: null } b && b.ResolutionId == p.ParentFrameId)
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(b, transferWeapon: _coreMatchParticipantsPlayersAccess._players[b.WeaponOwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[b.SlashTargetSeat].IsAlive);
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.Qinglong && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == p.ParentFrameId)
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = ChainedStateBasicNativeDecision(frame.OriginalDecision);
            _coreNativeQinglongOperations.ResolveQinglongCrescentBladeChoice(frame.OriginalDecision.Choices.Single(c => c.Parameters.GetValueOrDefault("action") == "qinglong-skip"));
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.OwnSlashDodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == p.ParentFrameId)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == attack.ResolutionId)
                _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || !_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (p.Intent is ChainedStateBasicIntent.ProgramSlash or ChainedStateBasicIntent.ProgramNearestSlash or ChainedStateBasicIntent.AssistedSlash or ChainedStateBasicIntent.NearestLegalSlash && _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame program && program.Id == p.ParentFrameId && program.InstructionIndex == p.Cursor)
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(program, "原用牌请求已失效；已支付费用不撤回。");
    }

    internal bool TryAdvanceChainedStateBasicAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat].IsHuman || ChainedStateBasicContext(prompt)is not { } need)
            return false;
        var choice = prompt.Choices.FirstOrDefault(c => IsChainedStateBasicAnswer(c) && ChainedStateBasicResponseChoices(prompt, need).Any(v => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([v], [c])));
        if (choice is null || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || !int.TryParse(choice.Parameters.GetValueOrDefault("native-target-seat"), out var target))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        if (need.Intent == ChainedStateBasicIntent.OwnSlashDodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack)
        {
            var decision = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseDodgeResponse(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), attack.SourceSeat, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, true);
            _coreNativeAiDecisionsOperations.AddThought(decision.Item3);
            if (!decision.Item1)
                return false;
        }
        else if (need.Intent == ChainedStateBasicIntent.BorrowedSword && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } borrowed)
        {
            // The mature evaluator recognizes its native Slash-choice marker. Adapt only
            // this actor's already-published, revalidated option for evaluation; the
            // original prompt and the exact choice used for payment remain unchanged.
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat);
            var parameters = choice.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal);
            parameters["response"] = "borrowed-sword-slash";
            var evaluationPrompt = prompt with
            {
                Choices = Array.AsReadOnly(prompt.Choices.Select(candidate => candidate.Id == choice.Id ? candidate with { Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(parameters) } : candidate).ToArray())
            };
            var decision = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseBorrowedSwordResponse(view with { PendingDecision = evaluationPrompt }, borrowed.SourceSeat, target, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            _coreNativeAiDecisionsOperations.AddThought(decision.Thought);
            if (!decision.UseSlash)
                return false;
        }
        else if (need.Intent == ChainedStateBasicIntent.Qinglong)
        {
            var decision = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseChainedStateBasicTargetUse(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), target, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            _coreNativeAiDecisionsOperations.AddThought(decision.Thought);
            if (!decision.UseSlash)
                return false;
        }

        BeginChainedStateBasic(actor, CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice), kind, need.Intent == ChainedStateBasicIntent.OwnSlashDodge ? [target] : choice.Targets, prompt, need);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
