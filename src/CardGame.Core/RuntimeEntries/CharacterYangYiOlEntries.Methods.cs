#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangYiOlEntries
{
    internal void BeginJuanxiaFireAttackReveal(ProgramSkillFrame frame, JuanxiaLaunchState state) => _resolve().BeginJuanxiaFireAttackReveal(frame, state);
    internal void BeginJuanxiaRetaliationSlash(ProgramSkillFrame frame) => _resolve().BeginJuanxiaRetaliationSlash(frame);
    internal void BeginJuanxiaVirtualDuel(ProgramSkillFrame frame, JuanxiaLaunchState state) => _resolve().BeginJuanxiaVirtualDuel(frame, state);
    internal bool CanUseJuanxiaRetaliationSlash(int actorSeat, int targetSeat) => _resolve().CanUseJuanxiaRetaliationSlash(actorSeat, targetSeat);
    internal void CancelJuanxiaTrick(ProgramSkillFrame frame, JuanxiaLaunchState state, CardKind kind, string reason) => _resolve().CancelJuanxiaTrick(frame, state, kind, reason);
    internal void FinishJuanxiaLaunch(ProgramSkillFrame frame) => _resolve().FinishJuanxiaLaunch(frame);
    internal void FinishJuanxiaRetaliation(ProgramSkillFrame frame) => _resolve().FinishJuanxiaRetaliation(frame);
    internal bool IsJuanxiaDuelOrigin(ProgramSkillFrame frame, scoped out int targetSeat) => _resolve().IsJuanxiaDuelOrigin(frame, out targetSeat);
    internal bool IsJuanxiaTrickCandidate(int ownerSeat, int targetSeat, CardKind kind, IReadOnlyList<string> usedKinds) => _resolve().IsJuanxiaTrickCandidate(ownerSeat, targetSeat, kind, usedKinds);
    internal SkillProgramStepOutcome JuanxiaProgramDeclareTricks(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().JuanxiaProgramDeclareTricks(frame, effect);
    internal SkillProgramStepOutcome JuanxiaProgramRetaliation(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().JuanxiaProgramRetaliation(frame, effect);
    internal IReadOnlyList<(long LaunchFrameId, int Count)> OutstandingJuanxiaDebts(int ownerSeat, int debtorSeat) => _resolve().OutstandingJuanxiaDebts(ownerSeat, debtorSeat);
    internal void PresentJuanxiaFirePaymentPrompt(ProgramSkillFrame frame, Card revealed) => _resolve().PresentJuanxiaFirePaymentPrompt(frame, revealed);
    internal SkillProgramStepOutcome PresentJuanxiaRetaliationPrompt(ProgramSkillFrame frame) => _resolve().PresentJuanxiaRetaliationPrompt(frame);
    internal void PresentJuanxiaTargetCardPrompt(ProgramSkillFrame frame, CardKind kind) => _resolve().PresentJuanxiaTargetCardPrompt(frame, kind);
    internal SkillProgramStepOutcome PresentJuanxiaTrickPrompt(ProgramSkillFrame frame) => _resolve().PresentJuanxiaTrickPrompt(frame);
    internal void ResolveJuanxiaFirePaymentChoice(PromptChoice selected) => _resolve().ResolveJuanxiaFirePaymentChoice(selected);
    internal void ResolveJuanxiaRetaliateChoice(PromptChoice selected) => _resolve().ResolveJuanxiaRetaliateChoice(selected);
    internal void ResolveJuanxiaTargetCardChoice(PromptChoice selected) => _resolve().ResolveJuanxiaTargetCardChoice(selected);
    internal void ResolveJuanxiaTrickChoice(PromptChoice selected) => _resolve().ResolveJuanxiaTrickChoice(selected);
    internal bool ResumeJuanxia(long frameId) => _resolve().ResumeJuanxia(frameId);
    internal void ReturnJuanxiaPublicHand(ProgramSkillFrame frame) => _resolve().ReturnJuanxiaPublicHand(frame);
}
