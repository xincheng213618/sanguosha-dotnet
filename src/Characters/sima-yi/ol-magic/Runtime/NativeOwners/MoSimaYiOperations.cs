#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoSimaYiOperations(Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ContentRegistry> getContentRegistry, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime, Func<DeterministicRandom> getDeterministicRandom, Func<RuntimeEntries.CharacterSimaYiOlMagicEntries> getCharacterSimaYiOlMagicRuntime, Func<RuntimeEntries.CoreGameEngineProgramVirtualSlashOffersEntries> getCoreGameEngineProgramVirtualSlashOffersRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreNativeDamageCompletionEntries> getCoreNativeDamageCompletionRuntime, ProgramAttackHandleFactory createProgramAttackHandle, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMoSimaYiNative
{
    public void ObtainDodge(ProgramSkillFrame f, MoSimaYiDodge entity)
    {
        if (entity.CardId <= 0 || getCardZoneStore().GetLocation(entity.CardId) != entity.From || getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(entity.CardId).Kind != CardKind.Dodge)
            throw new InvalidOperationException("Guifu needs the actual stock Dodge frozen for this obtain.");
        getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(entity.CardId), entity.From, CardLocation.Hand(f.OwnerSeat), new("skill-program.ol:mo-guifu.obtain"), m => advanceEventRulesAndQueueFact(new MoSimaYiDodgeMarkedEvent(f.Id, f.OwnerSeat, m.CardId, m.From, m.Sequence)), disclosure: entity.From == CardLocation.DrawPile ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
    }

    public bool DrainChildren(long id) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(id);
    public void GrantSkill(ProgramSkillFrame f, string skill, bool temporary)
    {
        if (!matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].IsAlive || matchOutcomeState.ReferenceToWinner != Winner.None)
            return;
        _ = getContentRegistry().GetSkill(skill);
        var player = matchParticipantsState.ReferenceToPlayers[f.OwnerSeat];
        var source = temporary ? $"turn:{actualTurnProgressState.ReferenceToTurnNumber}:mo-sima-yi:{f.Id}:{skill}" : $"mo-sima-yi:{f.Id}:{skill}";
        if (getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MoSimaYiSkillGrantedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == skill))
            throw new InvalidOperationException("A frozen Sima Yi skill grant cannot be paid twice.");
        var old = getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(player).ActiveGrants.FirstOrDefault(g => g.SkillId == skill);
        var expiry = temporary ? new SkillGrantNextActualTurnStartExpiry(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, player.Seat) : null;
        player.SkillGrants.Grant(new SkillGrant($"{source}:{skill}", skill, old?.SkillInstanceId ?? $"mo-sima-yi-shared:{player.Seat}:{skill}", source) { NextActualTurnStartExpiry = expiry });
        advanceEventRulesAndQueueFact(new ProgramTurnSkillsGrantedEvent(f.Id, f.SkillId, EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, Array.AsReadOnly(new[] { skill })) { RecipientSeat = player.Seat, GrantSourceId = source, NextActualTurnStartExpiry = expiry });
        advanceEventRulesAndQueueFact(new MoSimaYiSkillGrantedEvent(f.Id, skill, source, temporary));
    }

    public void EnterRumo(ProgramSkillFrame f) => getCharacterSharedGameEngineOlMoRumoRuntime().EnterOlMoRumo(f);
    public IReadOnlyList<string> Sample(IReadOnlyList<string> skills, int count, long frameId)
    {
        var pool = skills.ToList();
        var result = new List<string>(count);
        var before = getDeterministicRandom().State;
        while (result.Count < count)
        {
            var index = getDeterministicRandom().Next(pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        advanceEventRulesAndQueueFact(new MoSimaYiSampledEvent(frameId, before, getDeterministicRandom().State, count));
        return Array.AsReadOnly(result.ToArray());
    }

    public bool BeginSlash(ProgramSkillFrame f, int actor, int target)
    {
        if (!getCharacterSimaYiOlMagicRuntime().CanMoSimaYiRetaliationSlash(actor, target))
            return false;
        getCoreGameEngineProgramVirtualSlashOffersRuntime().BeginOfferedProgramVirtualSlash(f, actor, target);
        return true;
    }

    public void ApplyLieshi(ProgramSkillFrame f, int actor, int option)
    {
        if (option == 0)
        {
            var player = matchParticipantsState.ReferenceToPlayers[actor];
            if (player.JudgmentAreaAbolished)
                throw new InvalidOperationException("Lieshi cannot abolish an absent judgment area.");
            player.JudgmentAreaAbolished = true;
            advanceEventRulesAndQueueFact(new PlayerAreasAbolishedEvent(actor, player.EquipmentAreaAbolished, true));
            getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, actor, 1, nature: DamageNature.Fire);
            return;
        }

        var kind = option == 1 ? CardKind.Dodge : CardKind.Slash;
        var playerHand = getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[actor]);
        var cards = playerHand.Where(c => kind == CardKind.Slash ? EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) : c.Kind == kind).ToArray();
        var reason = new CardMoveReason("skill-program.ol:mo-lieshi.discard");
        var logical = getCoreGameEngineNativeLogicalDiscardsRuntime().CaptureNativeLogicalDiscardInvocation(actor, actor, cards.Select(c => c.Id).ToArray(), reason, f.Id);
        getCorePhysicalMovementFacadesRuntime().MoveCards(cards, CardLocation.Hand(actor), CardLocation.DiscardPile, reason);
        getCoreGameEngineNativeLogicalDiscardsRuntime().CompleteNativeLogicalDiscardInvocation(logical);
    }

    public bool FinishDamage(ProgramSkillFrame f)
    {
        if (f.AttackAttempt is not null)
        {
            getCoreNativeDamageCompletionRuntime().CompleteDamageAttack(createProgramAttackHandle(f.Id));
            return true;
        }

        if (f.AttackReturn is not null)
            throw new InvalidOperationException("Lieshi cannot skip its native damage return.");
        return false;
    }

    public void Complete(ProgramSkillFrame f)
    {
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(f);
        getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    }

    public void PublishState() => advanceRulesAndPublishState();
}
