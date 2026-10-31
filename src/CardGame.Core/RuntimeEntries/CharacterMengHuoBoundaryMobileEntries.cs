#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime))]
internal sealed partial class CharacterMengHuoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> _resolve;
    private CharacterMengHuoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve) => new(resolve);
}
