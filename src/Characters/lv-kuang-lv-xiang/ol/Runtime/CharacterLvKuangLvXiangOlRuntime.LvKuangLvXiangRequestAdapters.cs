#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    internal const string QigongFactionBind = "qigong-native-faction";
    internal bool IsQigongRequest(ProgramSkillFrame f) => f.LvKuangLvXiang is { Stage: "slash", OriginalUseId: not null, Actor: >= 0 } r && r.Actor != r.Target && r.Source == LvKuangLvXiangSource(f) && r.GameplayHash == f.GameplayHash && r.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber;
    internal bool QigongRequestParticipantsLive(ProgramSkillFrame f) => f.LvKuangLvXiang is { Actor: >= 0 } r && _coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
    internal void AddQigongNativeAdapterChoices(ProgramSkillFrame f, List<PromptChoice> choices)
    {
        var r = f.LvKuangLvXiang!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        if (_coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(actor, CardKind.Slash)is { } policy && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is null && _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actor.Seat, policy.FactionId).Count > 0 && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[r.Target], new(0, CardKind.Slash, Suit.None, 0), effectiveKind: CardKind.Slash, ignoreDistance: true, noEffectiveRank: true, physicalCardIds: []))
            choices.Add(LvChoice(f, "faction", "faction", "请求势力角色提供杀", targets: [r.Target]));
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor))
            foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false).Where(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.DeclarationValidation is not null))
                {
                    if (!_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[r.Target], card, source, kind, ignoreDistance: true, physicalCardIds: [card.Id]) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [card.Id]))
                        continue;
                    var args = new Dictionary<string, string>
                    {
                        {
                            "effective-kind",
                            kind.ToString()
                        }
                    };
                    CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
                    choices.Add(LvChoice(f, "declaration", $"declare-{(card.Id)}-{(source.SkillInstanceId)}-{(source.BindingId)}", "以真实手牌声明杀", [card.Id], [r.Target], args));
                }
    }

    internal bool TryBeginQigongNativeAdapter(ProgramSkillFrame f, PromptChoice choice)
    {
        if (choice.Parameters["branch"] == "declaration")
        {
            BeginQigongDeclaration(f, choice);
            return true;
        }

        if (choice.Parameters["branch"] != "faction")
            return false;
        if (!IsQigongRequest(f) || !QigongRequestParticipantsLive(f) || !LvKuangLvXiangCurrent(f) || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null)
            throw new InvalidOperationException("Qigong faction need is no longer current.");
        var r = f.LvKuangLvXiang!;
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[r.Actor], CardKind.Slash)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Faction = new(f.Id, r.Actor, r.Target, policy) } });
        var seats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(r.Actor, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, r.Actor, seats, policy.FactionId, targetSeat: r.Target, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: QigongFactionBind);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.AuthorizeProgramRequest(f.InstructionIndex, ignoreDistance: true, requestIdentity: _slashRequests.GetIdentity(f));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, r.Actor, seats, policy.SkillId, IsActiveUse: true, r.Target));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
        return true;
    }

    internal bool ValidateQigongFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.LvKuangLvXiang?.Faction is not { } r)
            return false;
        if (r.FrameId != f.Id || r.Actor != f.LvKuangLvXiang.Actor || r.Target != f.LvKuangLvXiang.Target || pending.ProgramSkillFrameId != f.Id || pending.ResolutionId != f.Id || pending.OwnerSeat != r.Actor || pending.TargetSeat != r.Target || pending.PolicySource != r.Policy || pending.AssistedResultBind != QigongFactionBind || !pending.IsAssistedProgramUse || f.LvKuangLvXiang.Stage is not ("slash" or "issuing" or "native") || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FactionSlashRequestedEvent>().Count(e => e.ResolutionId == f.Id && e.OwnerSeat == r.Actor && e.TargetSeat == r.Target && e.SkillId == r.Policy.SkillId) != 1)
            throw new InvalidOperationException("Qigong faction provider lost the original exact actor/policy/request.");
        return true;
    }

    internal bool FinishQigongFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvKuangLvXiang: { Faction: not null } } f || f.Id != id)
            return false;
        ValidateQigongFactionSlashParent(pending, f);
        FinishLvKuangLvXiang(f);
        return true;
    }
}
