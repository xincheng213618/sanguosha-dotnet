#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class KunlunMirrorCapability(Func<RuntimeEntries.CoreGameEngineKunlunMirrorEntries> getCoreGameEngineKunlunMirrorRuntime) : IKunlunMirrorHost
{
    public void Resolve(ProgramSkillFrame frame) => getCoreGameEngineKunlunMirrorRuntime().ResolveKunlunMirror(frame);
}
