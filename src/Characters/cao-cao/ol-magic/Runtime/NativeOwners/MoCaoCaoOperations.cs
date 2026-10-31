#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MoCaoCaoOperations(Func<DeterministicRandom> getDeterministicRandom, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CharacterCaoCaoOlMagicEntries> getCharacterCaoCaoOlMagicRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IMoCaoCaoOperations
{
    public int NextRandom(int count) => getDeterministicRandom().Next(count);
    public void PublishState() => advanceRulesAndPublishState();
    public void LogEquipment(int seat, CardKind weapon, CardKind armor) => getCoreMatchLogRuntime().AddLog("Fuzai", $"{matchParticipantsState.ReferenceToPlayers[seat].Name} 的覆载装备为【{getEquipmentContentRegistry().Get(weapon).DisplayName}】、【{getEquipmentContentRegistry().Get(armor).DisplayName}】。", seat);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public MoCaoCaoMovement Transfer(ProgramSkillFrame f, int payer, int id, string reason)
    {
        var before = cardMovementJournalState.ReferenceToMovementSequence;
        var from = getCardZoneStore().GetLocation(id);
        if (from.OwnerSeat != payer || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Bachao/Baibi lost the actual HE payment.");
        getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), from, CardLocation.Hand(f.OwnerSeat), new CardMoveReason(reason), m =>
        {
            if (getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == f.Id)is not { } parent || !getCharacterCaoCaoOlMagicRuntime().MoCaoCaoBachao.Valid(parent) || m.CardId != id || m.From != from || m.Reason.Value != reason || m.Sequence <= before)
                throw new InvalidOperationException("Magic Cao Cao payment lost its owning ancestor or actual movement.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(parent.Id, parent.MoCaoCaoBachao is { } r ? parent with { MoCaoCaoBachao = r with { After = m.Sequence } } : parent with { MoCaoCaoBaibi = parent.MoCaoCaoBaibi!with { After = m.Sequence } });
            if (parent.MoCaoCaoBachao is not null)
                getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, payer, f.OwnerSeat, [id], before);
        }, disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        var after = cardMovementJournalState.ReferenceToMovementSequence;
        return new(before, after, cardMovementJournalState.ReferenceToCardMovements.Any(m => m.CardId == id && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == reason && m.Sequence > before && m.Sequence <= after));
    }

    public void Damage(ProgramSkillFrame f, int target) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, 1);
    public void EnterRumo(ProgramSkillFrame f) => getCharacterSharedGameEngineOlMoRumoRuntime().EnterOlMoRumo(f);
    public bool AdvanceObserverChildren() => getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(true);
}
