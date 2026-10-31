#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramVirtualSlashOffersEntries
{
    internal void BeginOfferedProgramVirtualSlash(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int targetSeat) => _resolve().BeginOfferedProgramVirtualSlash(frame, actorSeat, targetSeat);
    internal int[] GetProgramVirtualSlashOfferTargets(int actorSeat) => _resolve().GetProgramVirtualSlashOfferTargets(actorSeat);
    internal void GrantProgramTurnCardEffectImmunity(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds) => _resolve().GrantProgramTurnCardEffectImmunity(frame, targetSeat, cardKinds);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferProgramVirtualSlashOrDraw(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat) => _resolve().OfferProgramVirtualSlashOrDraw(frame, actorSeat);
    internal void ResolveProgramVirtualSlashOfferChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramVirtualSlashOfferChoice(selected);
    internal void SetProgramVirtualSlashOfferPrompt(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, bool selectingTarget) => _resolve().SetProgramVirtualSlashOfferPrompt(frame, actorSeat, selectingTarget);
}
