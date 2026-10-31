#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoCaoCaoOperations(global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoCaoOlMagicEntries> getCharacterCaoCaoOlMagicRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IMoCaoCaoOperations
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
