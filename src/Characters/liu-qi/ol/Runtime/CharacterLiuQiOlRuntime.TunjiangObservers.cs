#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    internal bool HasTunjiangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawForUnskippedUntargetedPlay);
}
