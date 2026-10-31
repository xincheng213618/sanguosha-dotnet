#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineVirtualOrdinaryTrickUsesEntries
{
    internal SkillProgramStepOutcome BeginProgramVirtualOrdinaryTrick(ProgramSkillFrame input, SkillProgramEffect supplied) => _resolve().BeginProgramVirtualOrdinaryTrick(input, supplied);
    internal bool FinishProgramVirtualOrdinaryAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishProgramVirtualOrdinaryAttack(attack, out paused);
    internal bool IsExactOwnPlayEndingProgram(ProgramSkillFrame frame) => _resolve().IsExactOwnPlayEndingProgram(frame);
    internal bool IsExactOwnVirtualOrdinaryTrickProgram(ProgramSkillFrame frame) => _resolve().IsExactOwnVirtualOrdinaryTrickProgram(frame);
    internal bool IsExactOwnVirtualTrickTurnBoundary(ProgramSkillFrame frame) => _resolve().IsExactOwnVirtualTrickTurnBoundary(frame);
    internal bool IsProgramVirtualOrdinaryTrickUse(long frameId) => _resolve().IsProgramVirtualOrdinaryTrickUse(frameId);
    internal void IssueProgramVirtualOrdinaryTrick(ProgramSkillFrame frame, SkillProgramEffect effect, GameEngine.ProgramOrdinaryTrickUseOption option) => _resolve().IssueProgramVirtualOrdinaryTrick(frame, effect, option);
    internal bool MatchesVirtualOrdinaryTrickUseAction(long frameId, CardActionContext action, int cardId) => _resolve().MatchesVirtualOrdinaryTrickUseAction(frameId, action, cardId);
    internal Card ReadVirtualOrdinaryTrickUseAppearance(long ownerId, CardAppearanceReference appearance) => _resolve().ReadVirtualOrdinaryTrickUseAppearance(ownerId, appearance);
    internal void ResolveProgramVirtualOrdinaryTrickChoice(PromptChoice selected) => _resolve().ResolveProgramVirtualOrdinaryTrickChoice(selected);
    internal bool ResumeProgramVirtualOrdinaryAttack() => _resolve().ResumeProgramVirtualOrdinaryAttack();
    internal void ReturnProgramVirtualOrdinaryTrick(CardUseFrame use) => _resolve().ReturnProgramVirtualOrdinaryTrick(use);
    internal bool ValidProgramVirtualOrdinaryTrickUse(CardUseFrame use) => _resolve().ValidProgramVirtualOrdinaryTrickUse(use);
    internal SkillProgramEffect ValidateVirtualOrdinaryTrickInstruction(ProgramSkillFrame frame) => _resolve().ValidateVirtualOrdinaryTrickInstruction(frame);
    internal void ValidateVirtualOrdinaryTrickReturn(CardUseFrame use) => _resolve().ValidateVirtualOrdinaryTrickReturn(use);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> VirtualOrdinaryTrickOptions(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().VirtualOrdinaryTrickOptions(frame, effect);
}
