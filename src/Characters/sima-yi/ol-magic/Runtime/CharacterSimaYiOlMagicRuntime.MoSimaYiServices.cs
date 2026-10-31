#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiOlMagicRuntime
{
    internal MoSimaYiRuntime MoSimaYi => _moSimaYiOperations.Get();
    internal bool HasMoSimaYi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AcquireMoSimaYiGuifuDodge);

    int IMoSimaYiFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IMoSimaYiFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    bool IMoSimaYiFacts.IsAlive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMoSimaYiFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool IMoSimaYiFacts.IsGameOver => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    IReadOnlyList<T> IMoSimaYiFacts.History<T>() => Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>().ToArray());
    IReadOnlyList<CardConversionSource> IMoSimaYiFacts.GuiFuOwners() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(p, MoSimaYiComposition.Guifu)).Select(p => new CardConversionSource(MoSimaYiComposition.Guifu, "acquire-round", p.Seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(p, MoSimaYiComposition.Guifu))).ToArray());
    IReadOnlyList<string> IMoSimaYiFacts.EffectiveSkills(int owner) => Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ActiveGrants.Select(g => g.SkillId).Distinct(StringComparer.Ordinal).ToArray());
    IReadOnlyList<int> IMoSimaYiFacts.RetaliationActors(int owner, int round) => Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoSimaYiDamageCauseEvent>().Where(e => e.Cause.Round == round && e.Cause.SourceSeat == owner).Select(e => e.Cause.TargetSeat).Where(((IMoSimaYiFacts)this).IsAlive).Distinct().OrderBy(seat => (seat - owner + _coreMatchParticipantsPlayersAccess._players.Count) % _coreMatchParticipantsPlayersAccess._players.Count).ToArray());
    MoSimaYiDodge? IMoSimaYiFacts.FindDodge()
    {
        foreach (var location in new[]
        {
            CardLocation.DrawPile,
            CardLocation.DiscardPile
        }

        )
            if (_cardZonesOperations._cardZones.CardsAt(location).FirstOrDefault(c => c.Kind == CardKind.Dodge)is { } card)
                return new(card.Id, location);
        return null;
    }

    string IMoSimaYiFacts.SkillName(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Name;
    bool IMoSimaYiFacts.HasJudgmentArea(int actor) => !_coreMatchParticipantsPlayersAccess._players[actor].JudgmentAreaAbolished;
    void IMoSimaYiNative.ObtainDodge(ProgramSkillFrame f, MoSimaYiDodge entity)
    {
        if (entity.CardId <= 0 || _cardZonesOperations._cardZones.GetLocation(entity.CardId) != entity.From || _coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId).Kind != CardKind.Dodge)
            throw new InvalidOperationException("Guifu needs the actual stock Dodge frozen for this obtain.");
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), entity.From, CardLocation.Hand(f.OwnerSeat), new("skill-program.ol:mo-guifu.obtain"), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoSimaYiDodgeMarkedEvent(f.Id, f.OwnerSeat, m.CardId, m.From, m.Sequence)), disclosure: entity.From == CardLocation.DrawPile ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    bool IMoSimaYiNative.DrainChildren(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void IMoSimaYiNative.GrantSkill(ProgramSkillFrame f, string skill, bool temporary)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        _ = _contentRegistryOperations._contentRegistry.GetSkill(skill);
        var player = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var source = temporary ? $"turn:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)}:mo-sima-yi:{(f.Id)}:{(skill)}" : $"mo-sima-yi:{(f.Id)}:{(skill)}";
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoSimaYiSkillGrantedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == skill))
            throw new InvalidOperationException("A frozen Sima Yi skill grant cannot be paid twice.");
        var old = _coreNationalSkillsOperations.GetSkillBindingShard(player).ActiveGrants.FirstOrDefault(g => g.SkillId == skill);
        var expiry = temporary ? new SkillGrantNextActualTurnStartExpiry(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, player.Seat) : null;
        player.SkillGrants.Grant(new SkillGrant($"{(source)}:{(skill)}", skill, old?.SkillInstanceId ?? $"mo-sima-yi-shared:{(player.Seat)}:{(skill)}", source) { NextActualTurnStartExpiry = expiry });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnSkillsGrantedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, Array.AsReadOnly(new[] { skill })) { RecipientSeat = player.Seat, GrantSourceId = source, NextActualTurnStartExpiry = expiry });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoSimaYiSkillGrantedEvent(f.Id, skill, source, temporary));
    }

    void IMoSimaYiNative.EnterRumo(ProgramSkillFrame f) => _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
    IReadOnlyList<string> IMoSimaYiNative.Sample(IReadOnlyList<string> skills, int count, long frameId)
    {
        var pool = skills.ToList();
        var result = new List<string>(count);
        var before = _randomOperations._random.State;
        while (result.Count < count)
        {
            var index = _randomOperations._random.Next(pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoSimaYiSampledEvent(frameId, before, _randomOperations._random.State, count));
        return Array.AsReadOnly(result.ToArray());
    }

    bool IMoSimaYiNative.BeginSlash(ProgramSkillFrame f, int actor, int target)
    {
        if (!CanMoSimaYiRetaliationSlash(actor, target))
            return false;
        _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, actor, target);
        return true;
    }

    void IMoSimaYiNative.ApplyLieshi(ProgramSkillFrame f, int actor, int option)
    {
        if (option == 0)
        {
            var player = _coreMatchParticipantsPlayersAccess._players[actor];
            if (player.JudgmentAreaAbolished)
                throw new InvalidOperationException("Lieshi cannot abolish an absent judgment area.");
            player.JudgmentAreaAbolished = true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerAreasAbolishedEvent(actor, player.EquipmentAreaAbolished, true));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, actor, 1, nature: DamageNature.Fire);
            return;
        }

        var kind = option == 1 ? CardKind.Dodge : CardKind.Slash;
        var playerHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]);
        var cards = playerHand.Where(c => kind == CardKind.Slash ? CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) : c.Kind == kind).ToArray();
        var reason = new CardMoveReason("skill-program.ol:mo-lieshi.discard");
        var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(actor, actor, cards.Select(c => c.Id).ToArray(), reason, f.Id);
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Hand(actor), CardLocation.DiscardPile, reason);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
    }

    bool IMoSimaYiNative.FinishDamage(ProgramSkillFrame f)
    {
        if (f.AttackAttempt is not null)
        {
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(f.Id));
            return true;
        }

        if (f.AttackReturn is not null)
            throw new InvalidOperationException("Lieshi cannot skip its native damage return.");
        return false;
    }

    void IMoSimaYiNative.Complete(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    void IMoSimaYiNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 33900, SkillProgramEffectOp.ResolveMoSimaYiMoubian, SkillProgramEffectOp.ResolveMoSimaYiRetaliation)]
    internal bool CanOfferMoSimaYi(ProgramTriggerLegalityContext c) => c.Candidate.SkillId switch
    {
        MoSimaYiComposition.Moubian => MoSimaYi.Records(c.Candidate.OwnerSeat).Count >= 3,
        MoSimaYiComposition.Zhouxi => _characterSimaYiOlMagicOperations.CreateMoSimaYiFacts().RetaliationActors(c.Candidate.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber).Count > 0,
        _ => true
    };
    [ProgramPipelineHook(ProgramPipelinePhase.RumoEntry, 33900)]
    internal bool IsMoSimaYiRumoEntry(ProgramSkillFrame f) => f.MoSimaYi is { Operation: SkillProgramEffectOp.ResolveMoSimaYiMoubian, Stage: MoSimaYiStage.Granting } && MoSimaYi.Valid(f);
    [ProgramPipelineHook(ProgramPipelinePhase.RumoOrigin, 33900)]
    internal bool IsMoSimaYiRumoOrigin(long id, CardConversionSource source, string hash) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoSimaYiStartedEvent>().Count(e => e.FrameId == id && e.Source == source && e.GameplayHash == hash && e.Operation == SkillProgramEffectOp.ResolveMoSimaYiMoubian) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoSimaYiSkillFrozenEvent>().Any(e => e.FrameId == id && e.SkillId == MoSimaYiComposition.Zhouxi);
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 33900)]
    internal bool RecordMoSimaYiDamage(GameEngine.IDamageAttempt attack, long damageFrameId)
    {
        if (!HasMoSimaYi || !attack.DamageWasApplied)
            return false;
        var kind = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.EffectiveKind ?? attack.EffectiveCardKind;
        var direct = kind is null ? _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == attack.ResolutionId && f.AttackAttempt is not null) : null;
        MoSimaYi.RecordDamage(new(damageFrameId, attack.ResolutionId, attack.IsSourceLess ? null : attack.SourceSeat, attack.TargetSeat, kind, direct?.SkillId, direct?.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber));
        return false;
    }
}
