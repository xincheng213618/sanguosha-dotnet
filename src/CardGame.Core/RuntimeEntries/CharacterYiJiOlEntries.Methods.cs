#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYiJiOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGiveDrawPileBottomCard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginGiveDrawPileBottomCard(frame);
    internal bool IsYiJiBottomGiftMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paidEffect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsYiJiBottomGiftMovement(frame, paidEffect, pending);
}
