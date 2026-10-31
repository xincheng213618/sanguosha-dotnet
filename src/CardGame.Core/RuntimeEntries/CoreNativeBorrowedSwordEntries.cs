#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed partial class CoreNativeBorrowedSwordEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _resolve;
    private CoreNativeBorrowedSwordEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeBorrowedSwordEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> resolve) => new(resolve);
}
