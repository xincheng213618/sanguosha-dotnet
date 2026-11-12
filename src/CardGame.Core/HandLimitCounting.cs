namespace CardGame.Core;

// A hand-limit obligation and a legal self-discard payment are different sets.
internal interface IHandLimitCounting : IProgramSelectionQuery
{
    IReadOnlyList<Card> GetCountedHand(CharacterState owner);
}

internal sealed partial class TurnCardUseEffectStore
{
    internal long AllocateHandLimitGrantSequence() => ++_grantSequence;
}
