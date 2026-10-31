#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaishiCiMouOlRuntime
{
    internal int SequentialDeckKindGrowthRequestedCount(SkillProgramEffect effect, CharacterState owner) => effect.NumberExpression == SkillProgramNumberExpression.OwnerMaxHp ? Math.Max(0, owner.MaxHp) : effect.Amount;
    internal int SequentialDeckKindGrowthGrantAmount(CardConversionSource source, long frameId, int shortfall)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        var effect = program.Triggers.Single(trigger => trigger.Id == source.BindingId).Effects[0];
        if (effect.CapGrowthByInitialPlayerCount != true)
            return shortfall;
        // Player slots are fixed at setup, unlike the live-player set. A named
        // skill's prior issued grants remain spent across skill-instance changes.
        var prior = _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthGrantedEvent>().TakeWhile(item => item.ProgramFrameId != frameId).Where(item => item.Source.OwnerSeat == source.OwnerSeat && item.Source.SkillId == source.SkillId).Aggregate(0, (sum, item) => checked(sum + item.Amount));
        return Math.Min(shortfall, Math.Max(0, _coreMatchParticipantsPlayersAccess._players.Count - prior));
    }
}
