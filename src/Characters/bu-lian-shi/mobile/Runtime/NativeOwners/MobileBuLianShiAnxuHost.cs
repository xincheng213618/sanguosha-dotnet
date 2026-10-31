#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileBuLianShiAnxuHost(Func<RuntimeEntries.CharacterBuLianShiMobileEntries> getCharacterBuLianShiMobileRuntime) : IMobileBuLianShiAnxuHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterBuLianShiMobileRuntime().PrepareMobileBuLianShiAnxu(frame);
}
