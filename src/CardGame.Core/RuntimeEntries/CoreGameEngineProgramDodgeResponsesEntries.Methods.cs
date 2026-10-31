#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDodgeResponsesEntries
{
    internal void BeginProgramDodgeResponse(global::CardGame.Core.PromptChoice choice, bool advanceToHumanBoundary) => _resolve().BeginProgramDodgeResponse(choice, advanceToHumanBoundary);
    internal void CompleteProgramMaximumHandDodge(global::CardGame.Core.ProgramSkillFrame frame, bool succeeded) => _resolve().CompleteProgramMaximumHandDodge(frame, succeeded);
    internal bool HasProgramDodgeResponseContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().HasProgramDodgeResponseContinuation(attack);
    internal bool IsWithinAttackRange(int actorSeat, int targetSeat) => _resolve().IsWithinAttackRange(actorSeat, targetSeat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ProgramDodgeResponseChoices(global::CardGame.Core.CharacterState owner) => _resolve().ProgramDodgeResponseChoices(owner);
}
