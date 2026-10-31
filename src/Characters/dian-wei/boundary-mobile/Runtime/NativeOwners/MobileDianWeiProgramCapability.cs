#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileDianWeiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDianWeiBoundaryMobileRuntime>> getCharacterDianWeiBoundaryMobileRuntime) : IMobileDianWeiProgramHost
{
    public void ConsumeTarget(ProgramSkillFrame frame) => getCharacterDianWeiBoundaryMobileRuntime().Value.ConsumeMobileDianWeiTarget(frame);
}
