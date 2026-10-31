#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class KunlunMirrorCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineKunlunMirrorEntries> getCoreGameEngineKunlunMirrorRuntime) : IKunlunMirrorHost
{
    public void Resolve(ProgramSkillFrame frame) => getCoreGameEngineKunlunMirrorRuntime().ResolveKunlunMirror(frame);
}
