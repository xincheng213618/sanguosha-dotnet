#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYanMobileShiRuntime
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiZhangYanPublicStates(CharacterState owner, string id)
    {
        if (id is not (ShiZyFeijing or ShiZyXiaoge))
            return[];
        var result = new List<ProgramPublicRuleStateSnapshot>();
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == id))
        {
            void Add(string key, int value) => result.Add(new(instance.SkillInstanceId, key, ProgramPublicRuleStateKind.GameActivationProgress, value));
            if (id == ShiZyFeijing)
            {
                Add("feijing-used", _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, id, "sole-slash-path@" + instance.SkillInstanceId, SkillUsageScope.Turn));
                var pending = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.OwnerSeat == owner.Seat && f.SkillId == id && f.ShiZhangYan is not null)?.ShiZhangYan;
                Add("feijing-path-count", pending?.Plans.Count ?? 0);
                Add("feijing-committed", pending?.Plans.Count(p => p.Committed) ?? 0);
            }
            else
            {
                Add("xiaoge-prevented", _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyPreventedEvent>().Count(e => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == owner.Seat && s.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber)));
                Add("xiaoge-duels", _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyDuelIssuedEvent>().Count(e => e.Origin.OwnerSeat == owner.Seat && e.Origin.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber));
            }
        }

        return result.ToArray();
    }
}
