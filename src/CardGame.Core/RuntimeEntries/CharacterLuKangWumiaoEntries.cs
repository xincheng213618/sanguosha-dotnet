#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed partial class CharacterLuKangWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterLuKangWumiaoRuntime> _resolve;
    private CharacterLuKangWumiaoEntries(Func<EngineOwners.CharacterLuKangWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuKangWumiaoEntries Create(Func<EngineOwners.CharacterLuKangWumiaoRuntime> resolve) => new(resolve);
}
