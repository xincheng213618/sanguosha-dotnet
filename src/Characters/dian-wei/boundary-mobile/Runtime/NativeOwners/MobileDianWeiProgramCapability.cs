#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileDianWeiProgramCapability(Func<RuntimeEntries.CharacterDianWeiBoundaryMobileEntries> getCharacterDianWeiBoundaryMobileRuntime) : IMobileDianWeiProgramHost
{
    public void ConsumeTarget(ProgramSkillFrame frame) => getCharacterDianWeiBoundaryMobileRuntime().ConsumeMobileDianWeiTarget(frame);
}
