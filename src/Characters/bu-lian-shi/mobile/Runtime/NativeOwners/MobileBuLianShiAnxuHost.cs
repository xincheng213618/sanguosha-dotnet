#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileBuLianShiAnxuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBuLianShiMobileEntries> getCharacterBuLianShiMobileRuntime) : IMobileBuLianShiAnxuHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterBuLianShiMobileRuntime().PrepareMobileBuLianShiAnxu(frame);
}
