#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoOlRuntime
{
    internal bool HasSunHaoCanshiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSunHaoCanshiUseDiscard);
}
