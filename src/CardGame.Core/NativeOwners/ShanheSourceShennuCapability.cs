#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceShennuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceShennuRuntime>> getCoreGameEngineShanheSourceShennuRuntime) : IShanheSourceShennuHost
{
    public void ReceiveDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceShennuRuntime().Value.ReceiveShanheSourceShennuDamage(frame);
}
