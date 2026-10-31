#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActiveNativeAttackQueriesEntries
{
    internal NativeOwners.CardAttackHandle? ActiveCardAttack { get => _resolve().ActiveCardAttack; set => _resolve().ActiveCardAttack = value; }
}
