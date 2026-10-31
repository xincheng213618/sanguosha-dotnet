#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileDianWeiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDianWeiBoundaryMobileEntries> getCharacterDianWeiBoundaryMobileRuntime) : IMobileDianWeiProgramHost
{
    public void ConsumeTarget(ProgramSkillFrame frame) => getCharacterDianWeiBoundaryMobileRuntime().ConsumeMobileDianWeiTarget(frame);
}
