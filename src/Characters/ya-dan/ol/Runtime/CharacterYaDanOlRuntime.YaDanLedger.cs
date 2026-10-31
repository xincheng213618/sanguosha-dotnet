#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{
    internal const string QingyaDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.QingyaDiscardReason;
    internal bool HasYaDanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards);
    internal static CardConversionSource YaDanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ya_dan_ol_YaDanLedger.YaDanSource(f);
}
