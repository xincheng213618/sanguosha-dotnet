#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WenheRuleFlowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreWenheActivityProgramsEntries> getCoreWenheActivityProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries> getCoreGameEngineActivityBattleRulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramNativeDrawEntries> getCoreGameEngineProgramNativeDrawRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries> getCoreGameEngineAdvancedWeaponsRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom) : IWenheRuleFlowHost
{
    private static ModeWenheFlowCursor Cursor(ProgramSkillFrame frame) => new(frame.Id, frame.OwnerSeat, frame.WenheRuleFlow ?? throw new InvalidOperationException("The activity flow lost its owning receipt."));
    public ModeWenheFlowCursor? BeginContinuation(long frameId)
    {
        if (getFrameStore().LastOrDefault()is not ProgramSkillFrame { WenheRuleFlow: not null } frame || frame.Id != frameId)
            return null;
        return Cursor(frame);
    }

    public ModeWenheFlowCursor Current(long frameId) => Cursor(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId));
    public void AdvanceCursor(long frameId, int expectedIndex, int expectedStage, int nextIndex, int nextStage)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        var receipt = frame.WenheRuleFlow ?? throw new InvalidOperationException("The activity flow lost its owning receipt.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { WenheRuleFlow = receipt with { Index = nextIndex, Stage = nextStage } });
    }

    public void ClearFlow(long frameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        if (frame.WenheRuleFlow is not { } receipt || receipt.Index != receipt.Seats.Count)
            throw new InvalidOperationException("An unfinished activity flow cannot be cleared.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { WenheRuleFlow = null });
    }

    public bool DrainChildren(long frameId) => getCoreGameEngineActivityBattleRulesRuntime().DrainActivityRuleChildren(frameId);
    public bool DrainHpChildren(long frameId) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program);
    public void IncreaseMaximumHp(long frameId, int amount) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), amount);
    public bool LoseHp(long frameId, int seat, int amount)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        return createProgramSkillHost().LoseHp(frameId, frame.SkillId, seat, amount) == SkillProgramStepOutcome.AwaitChild;
    }

    public void BeginDraw(long frameId, int seat, int amount) => getCoreGameEngineProgramNativeDrawRuntime().BeginOperationProgramDraw(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), seat, amount, new("wenhe.rule.draw"));
    public void EquipRandomAvailable(long frameId, int seat)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        if (frame.WenheRuleFlow is not { Stage: 2 } receipt || receipt.Index >= receipt.Seats.Count || receipt.Seats[receipt.Index] != seat)
            throw new InvalidOperationException("An activity equipment effect lost its exact recipient cursor.");
        var equipment = getCardZoneStore().CardsAt(CardLocation.DrawPile).Where(card => getEquipmentContentRegistry().IsEquipment(card.Kind) && matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(getEquipmentContentRegistry().Get(card.Kind).Slot) > 0).ToArray();
        if (equipment.Length > 0)
            getCoreGameEngineAdvancedWeaponsRuntime().PlaceAdvancedEquipment(equipment[getDeterministicRandom().Next(equipment.Length)], seat);
    }
}
