#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicOffersRuntime
{
    internal const string DrawFundedBasicChoiceMarker = RuntimeSkills.RuntimeRules_Core_DrawFundedDistinctBasicOffers.DrawFundedBasicChoiceMarker;
    internal const string DrawFundedBasicDrawReason = "skill-program.draw-funded-distinct-basic.draw";
    internal bool DrawFundedDistinctBasicQualified(CharacterState actor)
    {
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Any(instance => instance.Program.ViewAs.Any(rule => rule.DrawFundedDistinctBasic is not null)))
            return false;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(actor);
        if (hand.Count == 0 || hand.Count != actor.Hp)
            return false;
        var color = CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, hand[0]));
        return color is not null && hand.All(card => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) == color);
    }

    internal bool DrawFundedDistinctBasicNameAvailable(int actorSeat, string ledger, CardKind kind) => !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicIssuedEvent>().Any(e => e.ActorSeat == actorSeat && e.MethodLedgerId == ledger && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.NormalizedName == CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind)) && !_resolutionStackOperations._resolutionStack.OfType<DrawFundedDistinctBasicFrame>().Any(f => f.Payment.Source.OwnerSeat == actorSeat && f.Payment.MethodLedgerId == ledger && f.Payment.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && f.Payment.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && f.Payment.NormalizedName == CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind));
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicRules(CharacterState actor, CardKind? output = null, bool responseUse = false, bool dyingUse = false)
    {
        if (!DrawFundedDistinctBasicQualified(actor))
            yield break;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs.Where(r => r.DrawFundedDistinctBasic is not null))
                if ((output is null || rule.OutputKind == output) && (dyingUse ? rule.ForPlay && !rule.NoDying : responseUse ? rule.ForResponse : rule.ForPlay) && rule.InputCount == 0 && rule.UseOnly && rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor)) && DrawFundedDistinctBasicNameAvailable(actor.Seat, rule.DrawFundedDistinctBasic!.MethodLedgerId, rule.OutputKind) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, rule.OutputKind, CardActionType.Use) && _characterZhouYuOlMouOperations.IsMouGuquCardlessAllowed(actor, rule.OutputKind, responseUse, dyingUse))
                    yield return (new(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId), rule, instance.Program.GameplayHash);
    }

    internal IReadOnlyList<LegalAction> DrawFundedDistinctBasicPlayActions(CharacterState actor)
    {
        if (!actor.IsAlive || (_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || _resolutionStackOperations._resolutionStack.Count != 0) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor))
            return[];
        var actions = new List<LegalAction>();
        foreach (var(source, rule, _)in DrawFundedDistinctBasicRules(actor))
            actions.AddRange(DrawFundedDistinctBasicNativePlayActions(actor, source, rule.OutputKind));
        return Array.AsReadOnly(actions.ToArray());
    }

    internal IReadOnlyList<LegalAction> DrawFundedDistinctBasicNativePlayActions(CharacterState actor, CardConversionSource source, CardKind kind)
    {
        var result = new List<LegalAction>();
        if (!actor.IsAlive || (_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat != actor.Seat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return result;
        var card = new Card(0, kind, Suit.None, 0);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: kind);
            foreach (var target in targets)
                result.Add(new(LegalActionKind.Slash, 0, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"摸一张牌，视为使用【{(card.DisplayName)}】攻击 {(target.Name)}"), kind) { ConversionSource = source });
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(result, actor, card, targets, card.DisplayName, kind, source, null);
        }
        else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
            result.Add(new(LegalActionKind.Peach, 0, null, _coreCardConversionsOperations.DescribeConversion(source, "摸一张牌，视为使用【桃】"), kind) { ConversionSource = source });
        else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || (_coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor)) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
            result.Add(new(LegalActionKind.Alcohol, 0, null, _coreCardConversionsOperations.DescribeConversion(source, "摸一张牌，视为使用【酒】"), kind) { ConversionSource = source });
        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(result, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(result, actor);
        return Array.AsReadOnly(result.Select(a => a with { DrawFundedDistinctBasicUse = _corePlayCardHistoryOperations.ViewAsRule(source)!.DrawFundedDistinctBasic }).ToArray());
    }

    internal bool TrySubmitDrawFundedDistinctBasicPlay(PlayCardCommand command, out CommandResult result)
    {
        result = null !;
        if (command.CardId != 0 || command.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.DrawFundedDistinctBasic is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = DrawFundedDistinctBasicPlayActions(actor).SingleOrDefault(a => a.ConversionSource == source && a.PlayedCardKind == command.PlayedCardKind && a.TargetCardId == command.TargetCardId && a.TargetSeats.SequenceEqual(command.TargetSeats ?? []));
        if (action is null || command.AdditionalConversionSources is { Count: > 0 } || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.PlayCard } original)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The draw-funded use lost its exact published source, qualification, name or native targets.");
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasic(actor, source, action.PlayedCardKind!.Value, action.TargetSeats, original, new(DrawFundedDistinctBasicIntent.Play, null, null, null, 0, action.TargetSeat));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    internal bool TryExecuteDrawFundedDistinctBasicPlay(CharacterState actor, LegalAction action)
    {
        if (action.CardId != 0 || action.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.DrawFundedDistinctBasic is null)
            return false;
        if (!DrawFundedDistinctBasicPlayActions(actor).Any(a => a.Kind == action.Kind && a.ConversionSource == source && a.PlayedCardKind == action.PlayedCardKind && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("An AI draw-funded use changed its published action before acceptance.");
        var original = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? new PendingDecision(DecisionKind.PlayCard, actor.Seat, "已发布合法出牌动作", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Revision = _coreCommandSessionQueriesOperations.Revision
        };
        // The exact selection belongs to the payment frame; a draw child must
        // not borrow the outer use's temporary target selection.
        _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = null;
        _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasic(actor, source, action.PlayedCardKind!.Value, action.TargetSeats, original, new(DrawFundedDistinctBasicIntent.Play, null, null, null, 0, action.TargetSeat));
        return true;
    }

    internal GameEngine.DrawFundedBasicNeed? DrawFundedDistinctBasicContext(PendingDecision prompt, ResolutionFrame? suppliedTop = null)
    {
        var top = suppliedTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (top is null || top is DrawFundedDistinctBasicFrame or RequestedDeckBasicFrame)
            return null;
        var ownerId = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        var parentAction = _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)?.Action?.ActionId;
        if (prompt.Kind == DecisionKind.RespondDodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && top is ResponseWindowFrame && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == prompt.PlayerSeat && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat], CardKind.Dodge) && ownerId == attack.ResolutionId)
            return new(DrawFundedDistinctBasicIntent.OwnSlashDodge, ownerId, top.Id, parentAction, attack.SuccessfulDodgeResponses, attack.SourceSeat);
        if (prompt.Kind == DecisionKind.RescueDying && _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && top.Id == dying.FrameId && dying.ResponderSeat == prompt.PlayerSeat)
            return new(DrawFundedDistinctBasicIntent.Dying, dying.FrameId, top.Id, null, dying.ResponderIndex, dying.VictimSeat);
        if (prompt.Kind == DecisionKind.RespondSlash && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } borrowed && borrowed.WeaponOwnerSeat == prompt.PlayerSeat && borrowed.ResolutionId == ownerId && _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat])is not null)
            return new(DrawFundedDistinctBasicIntent.BorrowedSword, ownerId, top.Id, parentAction, 0, borrowed.SlashTargetSeat);
        if (prompt.Kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } blade && blade.Attack.SourceSeat == prompt.PlayerSeat && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack) && blade.Attack.ResolutionId == ownerId)
            return new(DrawFundedDistinctBasicIntent.Qinglong, ownerId, top.Id, parentAction, 0, blade.Attack.TargetSeat);
        if (prompt.Kind != DecisionKind.ProgramTrigger || top is not ProgramSkillFrame program)
            return null;
        if (_characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(program, out var wenyiActor, out var wenyiTarget) && prompt.PlayerSeat == wenyiActor && program.HuanShuJun is { Stage: HuanShuJunStage.WenyiPeachRequest or HuanShuJunStage.NativeIssuing })
            return new(DrawFundedDistinctBasicIntent.WenyiPeach, program.Id, program.Id, null, program.InstructionIndex, wenyiTarget);
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(program) && _characterSunYuanOlOperations.TryGetSayingSlashRequest(program, out var sayingActor, out var sayingTargets) && prompt.PlayerSeat == sayingActor)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, sayingTargets.Single());
        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(program) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(program, out var requestedActor, out var requestedTargets) && prompt.PlayerSeat == requestedActor)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, requestedTargets.Single());
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(program) && prompt.PlayerSeat == program.LvKuangLvXiang!.Actor)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, program.LvKuangLvXiang.Target);
        if (_characterWuJingOlOperations.IsWuJingHejiRequest(program) && prompt.PlayerSeat == program.OwnerSeat)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, program.WuJing!.TargetSeat);
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(program) && prompt.PlayerSeat == program.OwnerSeat)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, null);
        var paused = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
        if (paused.Op == SkillProgramEffectOp.RequestSlashByTarget && program.SelectedTargetSeats.SequenceEqual([prompt.PlayerSeat]) && prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "request-slash-decline"))
            return new(DrawFundedDistinctBasicIntent.ProgramSlash, program.Id, program.Id, null, program.InstructionIndex, program.OwnerSeat);
        if (paused.Op == SkillProgramEffectOp.RequestSlashByNearest && prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "request-slash-nearest-decline"))
            return new(DrawFundedDistinctBasicIntent.ProgramNearestSlash, program.Id, program.Id, null, program.InstructionIndex, null);
        if (paused.Op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget && program.AssistedSlashRequest is { TargetSeat: { } target } assisted && assisted.ActorSeat == prompt.PlayerSeat)
            return new(DrawFundedDistinctBasicIntent.AssistedSlash, program.Id, program.Id, null, program.InstructionIndex, target);
        if (paused.Op == SkillProgramEffectOp.RequestLegalSlashByNearest && program.NearestLegalSlashRequest is { AwaitingFaction: false } nearest && nearest.ActorSeat == prompt.PlayerSeat)
            return new(DrawFundedDistinctBasicIntent.NearestLegalSlash, program.Id, program.Id, null, program.InstructionIndex, null);
        return null;
    }

    internal bool DrawFundedDistinctBasicNativeResponseLegal(CharacterState actor, GameEngine.DrawFundedBasicNeed need, CardKind kind, int? targetSeat)
    {
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return false;
        if (need.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge)
            return kind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.ResolutionId == need.ParentFrameId && a.SuccessfulDodgeResponses == need.Cursor && a.SourceSeat == targetSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null;
        if (targetSeat is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, seat, Suit.None))
            return false;
        if (need.Intent == DrawFundedDistinctBasicIntent.WenyiPeach)
            return kind == CardKind.Peach && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == need.ParentFrameId)is ProgramSkillFrame wenyi && need.RequestFrameId == wenyi.Id && wenyi.InstructionIndex == need.Cursor && _characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(wenyi, out var wenyiActor, out var wenyiTarget) && wenyiActor == actor.Seat && wenyiTarget == seat && _coreWenyiPeachSuppliesOperations.WenyiPeachUseLegal(actor.Seat, seat, []);
        if (need.Intent == DrawFundedDistinctBasicIntent.Dying)
            return _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && d.FrameId == need.ParentFrameId && d.ResponderIndex == need.Cursor && d.ResponderSeat == actor.Seat && d.VictimSeat == seat && _coreMatchParticipantsPlayersAccess._players[seat].Hp <= 0 && (kind == CardKind.Peach && _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(actor.Seat, seat) || kind == CardKind.Alcohol && actor.Seat == seat && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat));
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], kind, Suit.None))
            return false;
        if (need.Intent == DrawFundedDistinctBasicIntent.BorrowedSword)
            return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true, ActiveAttack: null } b && b.ResolutionId == need.ParentFrameId && b.WeaponOwnerSeat == actor.Seat && b.SlashTargetSeat == seat && _coreNativeBorrowedSwordOperations.GetWeapon(actor)is not null && _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], kind, Suit.None, false, null, 0, []);
        if (need.Intent == DrawFundedDistinctBasicIntent.Qinglong)
            return _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == need.ParentFrameId && q.Attack.SourceSeat == actor.Seat && q.Attack.TargetSeat == seat && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, q.Attack) && _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], kind, null, false, Suit.None, 0);
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == need.ParentFrameId)is not ProgramSkillFrame program || program.InstructionIndex != need.Cursor)
            return false;
        if (_characterSunYuanOlOperations.TryGetSayingSlashRequest(program, out var sayingActor, out var sayingTargets))
            return need.Intent == DrawFundedDistinctBasicIntent.AssistedSlash && need.RequestFrameId == program.Id && actor.Seat == sayingActor && sayingTargets.Contains(seat) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: []);
        if (_characterLiYiOlOperations.TryGetChanshuangSlashRequest(program, out var requestedActor, out var requestedTargets))
            return need.Intent == DrawFundedDistinctBasicIntent.AssistedSlash && need.RequestFrameId == program.Id && actor.Seat == requestedActor && requestedTargets.Contains(seat) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: []);
        if (_characterWuJingOlOperations.IsWuJingHejiRequest(program))
            return _characterWuJingOlOperations.WuJingHejiAdapterActor(program, actor.Seat, seat, need.Cursor, need.ParentFrameId, need.RequestFrameId) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: [], ignoreDistance: true);
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(program))
            return _characterLvKuangLvXiangOlOperations.QigongAdapterActor(program, actor.Seat, seat, need.Cursor, need.ParentFrameId, need.RequestFrameId) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: [], ignoreDistance: true);
        if (!_coreMatchParticipantsPlayersAccess._players[program.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[program.OwnerSeat], program.SkillId, program.SkillInstanceId) && !_characterCaoChunOlOperations.IsCaoChunInitialRequest(program) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(program))
            return false;
        if (need.FixedTargetSeat is { } fixedTarget && fixedTarget != seat || need.Intent is DrawFundedDistinctBasicIntent.ProgramNearestSlash or DrawFundedDistinctBasicIntent.NearestLegalSlash && !_coreProgramSlashRequestOperations.GetNearestLivingCharacterSeats(actor.Seat).Contains(seat))
            return false;
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: []);
    }

    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => DrawFundedDistinctBasicRules(actor).Where(r => CoreNativeCardClassificationsRuntime.IsSlashCard(r.Rule.OutputKind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, r.Rule.OutputKind, Suit.None) && (qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, r.Rule.OutputKind, null, false, Suit.None, 0) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, r.Rule.OutputKind, Suit.None, false, null, 0, [])));
    internal bool HasDrawFundedDistinctBasicProgramSlash(CharacterState actor, IEnumerable<int> targets) => DrawFundedDistinctBasicRules(actor).Any(r => CoreNativeCardClassificationsRuntime.IsSlashCard(r.Rule.OutputKind) && targets.Any(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, s, Suit.None) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[s], new(0, r.Rule.OutputKind, Suit.None, 0), effectiveKind: r.Rule.OutputKind, noEffectiveRank: true, physicalCardIds: [])));
    internal IReadOnlyList<PromptChoice> DrawFundedDistinctBasicResponseChoices(PendingDecision prompt, GameEngine.DrawFundedBasicNeed need)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var choices = new List<PromptChoice>();
        foreach (var(source, rule, _)in DrawFundedDistinctBasicRules(actor, responseUse: need.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge, dyingUse: need.Intent == DrawFundedDistinctBasicIntent.Dying))
        {
            var targets = need.FixedTargetSeat is { } target ? new[]
            {
                target
            }

            : prompt.ValidTargetSeats.ToArray();
            foreach (var seat in targets.Where(seat => DrawFundedDistinctBasicNativeResponseLegal(actor, need, rule.OutputKind, seat)))
            {
                var p = new Dictionary<string, string>
                {
                    [DrawFundedBasicChoiceMarker] = need.Intent.ToString(),
                    ["output-kind"] = rule.OutputKind.ToString(),
                    ["original-frame-id"] = need.ParentFrameId!.Value.ToString(CultureInfo.InvariantCulture),
                    ["native-target-seat"] = seat.ToString(CultureInfo.InvariantCulture)
                };
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
                choices.Add(new(new($"draw-funded.{(need.Intent)}.{(need.ParentFrameId)}.{(source.BindingId)}.{(source.SkillInstanceId)}.{(seat)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，摸一张牌，视为使用【{(CardCatalog.Get(rule.OutputKind).DisplayName)}】。", [], (need.Intent is DrawFundedDistinctBasicIntent.OwnSlashDodge or DrawFundedDistinctBasicIntent.Dying) ? [] : [seat], p));
            }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal PendingDecision? AddDrawFundedDistinctBasicChoices(PendingDecision? prompt)
    {
        if (prompt is null || !_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started || prompt.Choices.Any(IsDrawFundedDistinctBasicAnswer) || DrawFundedDistinctBasicContext(prompt)is not { } need)
            return prompt;
        var choices = DrawFundedDistinctBasicResponseChoices(prompt, need);
        return choices.Count == 0 ? prompt : prompt with
        {
            Choices = Array.AsReadOnly(prompt.Choices.Concat(choices).ToArray())
        };
    }

    internal static bool IsDrawFundedDistinctBasicAnswer(PromptChoice choice) => RuntimeSkills.RuntimeRules_Core_DrawFundedDistinctBasicOffers.IsDrawFundedDistinctBasicAnswer(choice);
    internal static PendingDecision DrawFundedDistinctBasicNativeDecision(PendingDecision prompt) => RuntimeSkills.RuntimeRules_Core_DrawFundedDistinctBasicOffers.DrawFundedDistinctBasicNativeDecision(prompt);
    internal bool HasDrawFundedDistinctBasicDodge(CharacterState actor) => _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge) && DrawFundedDistinctBasicRules(actor, CardKind.Dodge, true).Any();
    internal bool HasDrawFundedDistinctBasicDyingResponse(CharacterState actor) => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && DrawFundedDistinctBasicRules(actor, dyingUse: true).Any(r => DrawFundedDistinctBasicNativeResponseLegal(actor, new(DrawFundedDistinctBasicIntent.Dying, d.FrameId, d.FrameId, null, d.ResponderIndex, d.VictimSeat), r.Rule.OutputKind, d.VictimSeat));
}
