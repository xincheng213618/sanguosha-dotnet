#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    internal void BeginLeChengyanBorrowedWithAuxiliary(CardUseFrame supplied)
    {
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        var p = u.LeChengyanEffect!;
        var cursor = p.BorrowedCursor;
        while (cursor < p.Targets.Count && (!_coreMatchParticipantsPlayersAccess._players[p.Targets[cursor]].IsAlive || _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[p.Targets[cursor]])is null || !_coreMatchParticipantsPlayersAccess._players.Any(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[p.Targets[cursor]], v))))
            cursor++;
        if (cursor == p.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { LeChengyanEffect = p with { Finished = true, AwaitingVictim = false, AwaitingBorrowedEffect = false } });
            _coreNativeCardUseCompletionOperations.FinishCardUse(u.Id, LeChengyanOriginalCard(u), u.CardKind);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { TargetIndex = cursor, LeChengyanEffect = p with { BorrowedCursor = cursor, AwaitingVictim = true, AwaitingBorrowedEffect = false, AuxiliaryVictimSeat = null } });
        var choices = LeChengyanBorrowedChoices(u);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, u.SourceSeat, $"乘烟·借刀杀人：为{(_coreMatchParticipantsPlayersAccess._players[p.Targets[cursor]].Name)}选择杀的目标。", [], choices.SelectMany(c => c.Targets).ToArray(), u.SourceSeat, CardKind.BorrowedSword)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = u.SourceSeat,
            Choices = choices,
            SkillPrompt = new(LeChengyan, "乘烟", "乘烟 · 借刀杀人", "原用牌的目标不变，此处仅补充借刀的辅助受害者。")
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[u.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> LeChengyanBorrowedChoices(CardUseFrame u)
    {
        var p = u.LeChengyanEffect!;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Targets[p.BorrowedCursor]];
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, v)).Select(v => new PromptChoice(new($"le-chengyan.{(u.Id)}.victim.{(p.BorrowedCursor)}.{(v.Seat)}"), $"令{(actor.Name)}对{(v.Name)}使用杀", [], [v.Seat], new Dictionary<string, string> { ["program-action"] = "le-chengyan-borrowed-victim", ["frame-id"] = u.Id.ToString(CultureInfo.InvariantCulture), ["target-index"] = p.BorrowedCursor.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal bool HasLeChengyanBorrowedPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { LeChengyanEffect: { AwaitingVictim: true } } u && ValidLeChengyanEffect(u) && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } d && d.PlayerSeat == u.SourceSeat && d.Choices.Count > 0 && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(d.Choices, LeChengyanBorrowedChoices(u));
    [ProgramAnswer("le-chengyan-borrowed-victim")]
    internal void AnswerLeChengyanBorrowedVictim(PromptChoice selected)
    {
        if (!HasLeChengyanBorrowedPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { LeChengyanEffect: { } p } u || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], LeChengyanBorrowedChoices(u).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Chengyan auxiliary target must belong to its current paid Use and native Borrowed Sword option.");
        var victim = selected.Targets.Single();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { LeChengyanEffect = p with { AwaitingVictim = false, AwaitingBorrowedEffect = true, AuxiliaryVictimSeat = victim } });
        _coreNativeNullificationOperations.BeginNullificationWindow(u.Id, LeChengyanOriginalCard(u), u.SourceSeat, [p.Targets[p.BorrowedCursor], victim], LegalActionKind.BorrowedSword, playedCardKind: u.CardKind);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 30201)]
    internal bool AdvanceLeChengyanBorrowedAi()
    {
        if (!HasLeChengyanBorrowedPrompt() || _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat].IsHuman)
            return false;
        var d = _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(d.PlayerSeat);
        var choice = d.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[d.PlayerSeat].ScoreProgramTarget(view, c.Targets.Single(), new(OwnerDraw: 0, OwnerRecovery: 0, OwnerHpLoss: 0, TargetDraw: 0, TargetRecovery: 0, TargetHpLoss: 1, GivesSelected: false, DiscardsSelected: false))).ThenBy(c => c.Targets.Single()).First();
        AnswerLeChengyanBorrowedVictim(choice);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal bool FinishLeChengyanBorrowedNative(NativeOwners.BorrowedSwordHandle borrowed)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(borrowed.ResolutionId)is not { LeChengyanEffect: { AwaitingBorrowedEffect: true, EffectKind: CardKind.BorrowedSword } p } u || !ValidLeChengyanEffect(u))
            return false;
        if (borrowed.WeaponOwnerSeat != p.Targets[p.BorrowedCursor] || borrowed.SlashTargetSeat != p.AuxiliaryVictimSeat)
            throw new InvalidOperationException("Chengyan Borrowed Sword returned to a different original target or auxiliary victim.");
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        if (CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.BorrowedSword, borrowed))
            _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _coreNativeCardUseCompletionOperations.FinishCardUse(u.Id, LeChengyanOriginalCard(u), u.CardKind);
        return true;
    }

    internal bool KeepLeChengyanBorrowedMaterial(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { LeChengyanEffect: { Started: true, Finished: false, EffectKind: CardKind.BorrowedSword } } u && ValidLeChengyanEffect(u);
    internal bool TryContinueLeChengyanBorrowed(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { LeChengyanEffect: { Finished: false, AwaitingBorrowedEffect: true, EffectKind: CardKind.BorrowedSword } p } u || !ValidLeChengyanEffect(u))
            return false;
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, u = u with { LeChengyanEffect = p with { BorrowedCursor = p.BorrowedCursor + 1, AwaitingBorrowedEffect = false, AuxiliaryVictimSeat = null } });
        BeginLeChengyanBorrowedWithAuxiliary(u);
        return true;
    }
}
