#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDamageCursorRuntimeEntries
{
    internal DamageTriggerWindowFrame? ActiveDamageTrigger { get => _resolve().ActiveDamageTrigger; }
}
