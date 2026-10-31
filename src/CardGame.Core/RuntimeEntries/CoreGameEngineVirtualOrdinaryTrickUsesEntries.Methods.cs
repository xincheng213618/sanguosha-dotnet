#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineVirtualOrdinaryTrickUsesEntries
{
    internal void AssertProgramVirtualOrdinaryTricks() => _resolve().AssertProgramVirtualOrdinaryTricks();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramVirtualOrdinaryTrick(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect supplied) => _resolve().BeginProgramVirtualOrdinaryTrick(input, supplied);
    internal bool FinishProgramVirtualOrdinaryAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishProgramVirtualOrdinaryAttack(attack, out paused);
    internal bool IsExactOwnPlayEndingProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactOwnPlayEndingProgram(frame);
    internal bool IsExactOwnVirtualOrdinaryTrickProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactOwnVirtualOrdinaryTrickProgram(frame);
    internal bool IsExactOwnVirtualTrickTurnBoundary(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactOwnVirtualTrickTurnBoundary(frame);
    internal bool IsProgramVirtualAreaAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsProgramVirtualAreaAttackConsistent(attack, processing);
    internal bool IsProgramVirtualOrdinaryTrickUse(long frameId) => _resolve().IsProgramVirtualOrdinaryTrickUse(frameId);
    internal bool IsProgramVirtualSingleAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsProgramVirtualSingleAttackConsistent(attack, processing);
    internal void IssueProgramVirtualOrdinaryTrick(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption option) => _resolve().IssueProgramVirtualOrdinaryTrick(frame, effect, option);
    internal bool MatchesVirtualOrdinaryTrickUseAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesVirtualOrdinaryTrickUseAction(frameId, action, cardId);
    internal global::CardGame.Core.Card ReadVirtualOrdinaryTrickUseAppearance(long ownerId, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadVirtualOrdinaryTrickUseAppearance(ownerId, appearance);
    internal void ResolveProgramVirtualOrdinaryTrickChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramVirtualOrdinaryTrickChoice(selected);
    internal bool ResumeProgramVirtualOrdinaryAttack() => _resolve().ResumeProgramVirtualOrdinaryAttack();
    internal void ReturnProgramVirtualOrdinaryTrick(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnProgramVirtualOrdinaryTrick(use);
    internal bool ValidProgramVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidProgramVirtualOrdinaryTrickUse(use);
    internal global::CardGame.Core.SkillProgramEffect ValidateVirtualOrdinaryTrickInstruction(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateVirtualOrdinaryTrickInstruction(frame);
    internal void ValidateVirtualOrdinaryTrickReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateVirtualOrdinaryTrickReturn(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> VirtualOrdinaryTrickOptions(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().VirtualOrdinaryTrickOptions(frame, effect);
    internal bool VirtualTrickDamageTargetMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int original) => _resolve().VirtualTrickDamageTargetMatches(use, attack, original);
}
