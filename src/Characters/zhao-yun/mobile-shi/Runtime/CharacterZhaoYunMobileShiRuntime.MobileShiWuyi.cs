#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYunMobileShiRuntime
{
    internal const string MobileShiWuyiSkill = "mobile:shi-wuyi";
    internal bool MobileShiWuyiAtLeast(CharacterState owner, int level) => _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(instance => instance.SkillId == MobileShiWuyiSkill && _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, instance.SkillId, instance.SkillInstanceId, CharacterZhaoYunMobileShiDefinitions.MobileShiWuyiLevels[level - 1]));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 67200)]
    internal void RecordMobileShiWuyiHandBasic(CardActionAcceptedEvent accepted)
    {
        var action = accepted.Action;
        if (action.ActorSeat != action.ProviderSeat || action.PhysicalCards.Count == 0 || action.PhysicalCards.Any(card => card.From != new CardLocation(CardZoneKind.Hand, action.ActorSeat)))
            return;
        var name = CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) ? CardKind.Slash : action.EffectiveKind;
        if (name is not (CardKind.Slash or CardKind.Dodge or CardKind.Peach or CardKind.Alcohol))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var instances = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(instance => instance.SkillId == MobileShiWuyiSkill).ToArray();
        if (!owner.IsAlive || instances.Length == 0)
            return;
        var seen = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileShiWuyiBasicNameRecordedEvent>().Where(item => item.OwnerSeat == owner.Seat).Select(item => item.Name).ToHashSet();
        if (!seen.Add(name))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiWuyiBasicNameRecordedEvent(owner.Seat, name, action.ActionId));
    }

    // Progress grants run on their original card program frame, through the
    // common setBooleanState instruction and its skill-change source guard.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67200)]
    internal bool QualifyMobileShiWuyiUnlock(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != MobileShiWuyiSkill || !gate.Trigger.Id.StartsWith("unlock-", StringComparison.Ordinal))
            return true;
        var level = gate.Trigger.Id[7] - '0';
        return level is >= 1 and <= 4 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileShiWuyiBasicNameRecordedEvent>().Where(item => item.OwnerSeat == gate.OwnerSeat).Select(item => item.Name).Distinct().Count() >= level;
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 67200)]
    internal bool MobileShiWuyiBasicDistance(ProgramCardDistanceQuery query) => query.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.Dodge or CardKind.Peach or CardKind.Alcohol && MobileShiWuyiAtLeast(query.Actor, 3);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 67200)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 67200)]
    internal bool MobileShiWuyiSlashDistance(ProgramSlashDistanceQuery query) => CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && MobileShiWuyiAtLeast(query.Actor, 3);
}
