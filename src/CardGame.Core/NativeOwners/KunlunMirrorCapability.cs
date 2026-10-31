#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class KunlunMirrorCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineKunlunMirrorRuntime>> getCoreGameEngineKunlunMirrorRuntime) : IKunlunMirrorHost
{
    public void Resolve(ProgramSkillFrame frame) => getCoreGameEngineKunlunMirrorRuntime().Value.ResolveKunlunMirror(frame);
}
