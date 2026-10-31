#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileBuLianShiAnxuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterBuLianShiMobileRuntime>> getCharacterBuLianShiMobileRuntime) : IMobileBuLianShiAnxuHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterBuLianShiMobileRuntime().Value.PrepareMobileBuLianShiAnxu(frame);
}
