#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDodgeResponsesEntries
{
    internal void BeginProgramDodgeResponse(PromptChoice choice, bool advanceToHumanBoundary) => _resolve().BeginProgramDodgeResponse(choice, advanceToHumanBoundary);
    internal void CompleteProgramMaximumHandDodge(ProgramSkillFrame frame, bool succeeded) => _resolve().CompleteProgramMaximumHandDodge(frame, succeeded);
    internal bool IsWithinAttackRange(int actorSeat, int targetSeat) => _resolve().IsWithinAttackRange(actorSeat, targetSeat);
    internal IEnumerable<PromptChoice> ProgramDodgeResponseChoices(CharacterState owner) => _resolve().ProgramDodgeResponseChoices(owner);
}
