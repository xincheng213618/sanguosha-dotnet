#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime))]
internal sealed partial class CharacterMaChaoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime> _resolve;
    private CharacterMaChaoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime> resolve) => new(resolve);
}
