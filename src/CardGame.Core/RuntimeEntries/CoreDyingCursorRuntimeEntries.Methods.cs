#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDyingCursorRuntimeEntries
{
    internal DyingFrame? ActiveDying { get => _resolve().ActiveDying; }
}
