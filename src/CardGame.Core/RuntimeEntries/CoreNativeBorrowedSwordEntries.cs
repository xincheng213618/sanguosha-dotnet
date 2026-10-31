#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed partial class CoreNativeBorrowedSwordEntries
{
    private readonly Func<EngineOwners.CoreNativeBorrowedSwordRuntime> _resolve;
    private CoreNativeBorrowedSwordEntries(Func<EngineOwners.CoreNativeBorrowedSwordRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeBorrowedSwordEntries Create(Func<EngineOwners.CoreNativeBorrowedSwordRuntime> resolve) => new(resolve);
}
