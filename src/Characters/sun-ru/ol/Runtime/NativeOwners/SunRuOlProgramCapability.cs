#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunRuOlProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunRuOlRuntime>> getCharacterSunRuOlRuntime) : ISunRuOlProgramHost
{
    public SkillProgramStepOutcome RewardCurrentTurnLastZoneLoss(ProgramSkillFrame f) => getCharacterSunRuOlRuntime().Value.BeginSunRuOlChishi(f);
    public SkillProgramStepOutcome AbolishEquipmentSlotsForDistinctBenefits(ProgramSkillFrame f) => getCharacterSunRuOlRuntime().Value.BeginSunRuOlWeimian(f);
}
