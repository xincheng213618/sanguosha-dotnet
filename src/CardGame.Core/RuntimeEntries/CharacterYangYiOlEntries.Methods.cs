#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangYiOlEntries
{
    internal void BeginJuanxiaFireAttackReveal(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.JuanxiaLaunchState state) => _resolve().BeginJuanxiaFireAttackReveal(frame, state);
    internal void BeginJuanxiaRetaliationSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginJuanxiaRetaliationSlash(frame);
    internal void BeginJuanxiaVirtualDuel(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.JuanxiaLaunchState state) => _resolve().BeginJuanxiaVirtualDuel(frame, state);
    internal bool CanUseJuanxiaRetaliationSlash(int actorSeat, int targetSeat) => _resolve().CanUseJuanxiaRetaliationSlash(actorSeat, targetSeat);
    internal void CancelJuanxiaTrick(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.JuanxiaLaunchState state, global::CardGame.Core.CardKind kind, string reason) => _resolve().CancelJuanxiaTrick(frame, state, kind, reason);
    internal void FinishJuanxiaLaunch(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishJuanxiaLaunch(frame);
    internal void FinishJuanxiaRetaliation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishJuanxiaRetaliation(frame);
    internal bool IsJuanxiaDuelOrigin(global::CardGame.Core.ProgramSkillFrame frame, scoped out int targetSeat) => _resolve().IsJuanxiaDuelOrigin(frame, out targetSeat);
    internal bool IsJuanxiaTrickCandidate(int ownerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<string> usedKinds) => _resolve().IsJuanxiaTrickCandidate(ownerSeat, targetSeat, kind, usedKinds);
    internal global::CardGame.Core.SkillProgramStepOutcome JuanxiaProgramDeclareTricks(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().JuanxiaProgramDeclareTricks(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome JuanxiaProgramRetaliation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().JuanxiaProgramRetaliation(frame, effect);
    internal global::System.Collections.Generic.IReadOnlyList<(long LaunchFrameId, int Count)> OutstandingJuanxiaDebts(int ownerSeat, int debtorSeat) => _resolve().OutstandingJuanxiaDebts(ownerSeat, debtorSeat);
    internal void PresentJuanxiaFirePaymentPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card revealed) => _resolve().PresentJuanxiaFirePaymentPrompt(frame, revealed);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentJuanxiaRetaliationPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentJuanxiaRetaliationPrompt(frame);
    internal void PresentJuanxiaTargetCardPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardKind kind) => _resolve().PresentJuanxiaTargetCardPrompt(frame, kind);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentJuanxiaTrickPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentJuanxiaTrickPrompt(frame);
    internal void ResolveJuanxiaFirePaymentChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveJuanxiaFirePaymentChoice(selected);
    internal void ResolveJuanxiaRetaliateChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveJuanxiaRetaliateChoice(selected);
    internal void ResolveJuanxiaTargetCardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveJuanxiaTargetCardChoice(selected);
    internal void ResolveJuanxiaTrickChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveJuanxiaTrickChoice(selected);
    internal bool ResumeJuanxia(long frameId) => _resolve().ResumeJuanxia(frameId);
    internal void ReturnJuanxiaPublicHand(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnJuanxiaPublicHand(frame);
}
