#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed partial class CharacterLuXunWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterLuXunWumiaoRuntime> _resolve;
    private CharacterLuXunWumiaoEntries(Func<EngineOwners.CharacterLuXunWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuXunWumiaoEntries Create(Func<EngineOwners.CharacterLuXunWumiaoRuntime> resolve) => new(resolve);
}
