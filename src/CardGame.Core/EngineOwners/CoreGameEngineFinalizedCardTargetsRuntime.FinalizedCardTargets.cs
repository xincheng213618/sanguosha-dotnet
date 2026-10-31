#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFinalizedCardTargetsRuntime
{
    internal bool HasFinalizedTargetProgram(CardKind kind)
    {
        if (_coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.HasCardWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, new(kind)))
            return true;
        if (_modeRulesOperations._modeRules.RuntimeRequirements.TracksFinalizedCardTargets)
            return true;
        if (_coreActivityOrdinaryTricksOperations.HasActivityOrdinaryTrickProjection && (kind is CardKind.AllianceFeast or CardKind.SowBeans or CardKind.TransplantWood or CardKind.ReturnSoul || CoreGameEngineWenheCardsRuntime.IsWenheCard(kind) || CoreGameEngineDragonBoatCardsRuntime.IsDragonBoatTrick(kind) || CoreGameEngineGuanduCardsRuntime.IsGuanduTrick(kind)) && _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Any(player => _coreNationalSkillsOperations.GetSkillBindingShard(player).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(binding => binding.Trigger.CardKinds.Contains(kind))))
            return true;
        var category = CardCatalog.Get(kind).CategoryName switch
        {
            "基本牌" => SkillProgramCardCategory.Basic,
            "装备牌" => SkillProgramCardCategory.Equipment,
            _ => SkillProgramCardCategory.Trick
        };
        // Response exchanges and paired responses need the original trick's
        // accepted Use identity even without a target-changing subscriber.
        return _coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.HasCardWindow(ProgramTargetWindowStage.FinalizedAfterCategory, new(kind, category)) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasFinalizedCardTrigger(kind, category) || (_coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.HasCardWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, new(kind, category)) && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasFinalizedCardTrigger(kind, SkillProgramCardCategory.InstantTrick));
    }

    internal IReadOnlyList<int> GetImplicitSelfCardUseTargets(CardKind kind, int sourceSeat, IReadOnlyList<int> targets) => _coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.HasCardWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, new(kind)) && kind == CardKind.DrawTwo && targets.Count == 0 ? [sourceSeat] : targets.Count == 0 && (HasFinalizedTargetProgram(kind) && (kind == CardKind.Alcohol || CardCatalog.Get(kind).CategoryName == "装备牌") || _coreProgramCardUseNullificationOperations.HasBeforeTargetCardProgram(kind) && kind is CardKind.Alcohol or CardKind.DrawTwo) ? [sourceSeat] : targets;
    internal bool TryBeginEquipmentTargetPrograms(CharacterState source, Card equipment, long frameId)
    {
        if (_characterWeiYanHappyGodOperations.TryHappyWeiYanEquipmentCommitted(frameId, equipment) || _characterChengYuMouOperations.TryMouCyEquipmentCommitted(frameId, equipment) || _characterHuangZhongAnniversaryGodOperations.TryAnniversaryHzEquipmentCommitted(frameId, equipment) || _characterCaoZhenBoundaryMobileOperations.TryBoundarySidiEquipmentCommitted(frameId, equipment))
            return true;
        if (!HasFinalizedTargetProgram(equipment.Kind))
            return false;
        // Equipment commitment observes its installed slot, after target hooks finish.
        if (!TryBeginFinalizedSimpleCardPrograms(frameId, new(equipment.Id, SimpleCardUseEffect.EquipmentPlacement)))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(source, equipment, frameId);
        return true;
    }

    internal bool TryBeginFinalizedSimpleCardPrograms(long frameId, ProgramSimpleCardContinuation continuation)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == frameId);
        if (continuation.Effect == SimpleCardUseEffect.Equipment || !HasFinalizedTargetProgram(use.CardKind) || !_coreCardUseLifecycleOperations.TryMarkFinalizedSimpleProgramsStarted(frameId))
            return false;
        var action = use.Action ?? throw new InvalidOperationException("A finalized card use lost its action.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseTargetsFinalized, action.TargetSeats, ProgramCardContinuation.FinalizedSimpleCard, simpleContinuation: continuation);
    }
}
