#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime))]
internal sealed partial class CoreShanheBossProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime> _resolve;
    private CoreShanheBossProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShanheBossProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreShanheBossProgramsRuntime> resolve) => new(resolve);
}
