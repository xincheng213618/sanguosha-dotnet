#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal const string MouXqRequestAction = "mou-xiao-qiao-requested-slash";
    internal IReadOnlyList<PromptChoice> MouXqProgramSlashChoices(ProgramSkillFrame f, int actorSeat, IReadOnlyList<int> targets, string? resultBind = null)
    {
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f);
        if (effect is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || f.MouXiaoQiaoJueyanResponseGrant is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(f) && !_characterLiYiOlOperations.IsChanshuangSlashRequest(f) && !_characterSunYuanOlOperations.IsSayingSlashRequest(f)) || !_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, effect.Op, actorSeat) || resultBind is not null && effect.ResultBind != resultBind)
            return[];
        var allowed = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(f, actorSeat, effect.Op);
        if (allowed.Length == 0 || targets.Any(t => !allowed.Contains(t)))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var candidate = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(actor, SkillProgramTriggerWindow.PublicHandRevealed).FirstOrDefault(c => c.SkillId == MouXiaoQiaoOlRuntime.Jueyan);
        if (candidate is null)
            return[];
        var source = new CardConversionSource(candidate.SkillId, "retained-hand-use", actorSeat, candidate.SkillInstanceId);
        var choices = new List<PromptChoice>();
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, c)) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && MouXiaoQiaoOl.JueyanAvailable(actorSeat, c.Id)))
            foreach (var target in targets.Distinct())
            {
                var kind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card);
                if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == actorSeat || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actorSeat, target, Suit.None) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], new(0, kind, Suit.None, 0), source, kind, ignoreDistance: f.FixedTargetSlash?.IgnoreDistance == true, noEffectiveRank: true, physicalCardIds: []))
                    continue;
                var p = new Dictionary<string, string>
                {
                    ["program-action"] = MouXqRequestAction,
                    ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                    ["instruction-index"] = f.InstructionIndex.ToString(CultureInfo.InvariantCulture),
                    ["output-kind"] = kind.ToString()
                };
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
                choices.Add(new(new($"jueyan.request.{(f.Id)}.{(f.InstructionIndex)}.{(actorSeat)}.{(card.Id)}.{(target)}"), $"绝颜：展示并保留【{(card.DisplayName)}】，视为对{(_coreMatchParticipantsPlayersAccess._players[target].Name)}使用之。", [card.Id], [target], p));
            }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal bool HasMouXqProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && MouXqProgramSlashChoices(f, actor.Seat, targets.ToArray()).Count > 0;
    [ProgramAnswer(MouXqRequestAction)]
    internal void BeginMouXqProgramSlash(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || choice.Cards is not [var id] || choice.Targets is not [var target] || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || !p.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jueyan lost its actual native invited Slash choice.");
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f) ?? throw new InvalidOperationException("Jueyan lost its actual native Slash instruction.");
        if (!MouXqProgramSlashChoices(f, p.PlayerSeat, [target], effect.ResultBind).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Jueyan cannot change a native invitation's real actor or targets.");
        StartMouXqRequestedReveal(p, CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice), id, new(f.Id, DecisionKind.ProgramTrigger, kind, p.PlayerSeat, target, f.Id, effect.Op, f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, effect.ResultBind));
    }

    internal bool IsMouXqProgramRequest(ProgramSkillFrame f, MouXiaoQiaoJueyanResponseRequest r) => r.DecisionKind == DecisionKind.ProgramTrigger && r.ParentFrameId == f.Id && r.RequestInstruction == f.InstructionIndex && r.RequestSource == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && r.RequestHash == f.GameplayHash && r.RequestOperation is { } op && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.GameplayHash == f.GameplayHash && _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, op, r.ActorSeat) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.ResultBind == r.ResultBind && _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargets(f, r.ActorSeat, op).Contains(r.TargetSeat);
    internal void PrepareMouXqProgramSlash(MouXiaoQiaoJueyanResponseRequest r)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(r.ParentFrameId);
        if (!IsMouXqProgramRequest(f, r))
            throw new InvalidOperationException("Jueyan's accepted invitation lost its original producer.");
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(f))
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(f, r.ActorSeat, r.ActorSeat, r.TargetSeat, r.OutputKind, [], f.MouXiaoQiaoJueyanResponseGrant!.Source);
        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(f))
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(f, r.ActorSeat, r.ActorSeat, r.TargetSeat, r.OutputKind, [], f.MouXiaoQiaoJueyanResponseGrant!.Source);
        if (r.RequestOperation == SkillProgramEffectOp.RequestSlashByTarget)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, r.ResultBind!, "used-slash", r.ActorSeat, "绝颜：展示并视为使用原手牌。");
        else if (r.RequestOperation == SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, r.ResultBind!, "used-slash", r.ActorSeat, "绝颜：展示并视为使用原手牌。");
        }
        else if (r.RequestOperation is SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FixedTargetSlash = f.FixedTargetSlash!with { Issued = true } });
        else if (r.RequestOperation == SkillProgramEffectOp.RequestLegalSlashByNearest)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { NearestLegalSlashRequest = null });
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 34001)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 34001)]
    internal bool PreserveMouXqRequestedSlash(ProgramSkillFrame f) => f.MouXiaoQiaoJueyanResponseGrant is { Request.DecisionKind: DecisionKind.ProgramTrigger } g && g.Request.ParentFrameId == f.Id && g.Request.RequestSource == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && g.Request.RequestHash == f.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanResponseGrantedEvent>().Count(e => e.Grant == (g with { ActionId = 0, Returned = false })) == 1;
    [NativeUseReturn(34001)]
    internal void ReturnMouXqProgramSlash(CardUseFrame use)
    {
        if (use.MouXiaoQiaoJueyanRescueOrigin is not { Request.DecisionKind: DecisionKind.ProgramTrigger } g)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(g.Request.ParentFrameId);
        if (!PreserveMouXqRequestedSlash(f) || f.MouXiaoQiaoJueyanResponseGrant!.Returned || use.Action?.ActionId != g.ActionId)
            throw new InvalidOperationException("Jueyan's native invited Slash cannot return twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouXiaoQiaoJueyanResponseGrant = g with { Returned = true } });
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 34001)]
    internal bool ResumeMouXqProgramSlash(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXiaoQiaoJueyanResponseGrant: { Returned: true } g } f || f.Id != id)
            return false;
        if (!PreserveMouXqRequestedSlash(f))
            throw new InvalidOperationException("Jueyan's native invitation lost its completed whole-use receipt.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiaoQiaoJueyanResponseGrant = null });
        if (_characterSunYuanOlOperations.ResumeSayingSuppliedSlash(f) || _characterLiYiOlOperations.ResumeChanshuangSuppliedSlash(f))
            return true;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(f))
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        else if (f.FixedTargetSlash is { Issued: true })
            _coreFixedSlashAndDeclaredDeckProgramsOperations.TryResumeFixedSlashAndDeclaredDeck(id);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }
}
