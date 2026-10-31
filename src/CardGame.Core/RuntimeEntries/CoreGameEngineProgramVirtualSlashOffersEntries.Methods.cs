#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramVirtualSlashOffersEntries
{
    internal void BeginOfferedProgramVirtualSlash(ProgramSkillFrame frame, int actorSeat, int targetSeat) => _resolve().BeginOfferedProgramVirtualSlash(frame, actorSeat, targetSeat);
    internal int[] GetProgramVirtualSlashOfferTargets(int actorSeat) => _resolve().GetProgramVirtualSlashOfferTargets(actorSeat);
    internal void GrantProgramTurnCardEffectImmunity(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<CardKind> cardKinds) => _resolve().GrantProgramTurnCardEffectImmunity(frame, targetSeat, cardKinds);
    internal SkillProgramStepOutcome OfferProgramVirtualSlashOrDraw(ProgramSkillFrame frame, int actorSeat) => _resolve().OfferProgramVirtualSlashOrDraw(frame, actorSeat);
    internal void ResolveProgramVirtualSlashOfferChoice(PromptChoice selected) => _resolve().ResolveProgramVirtualSlashOfferChoice(selected);
    internal void SetProgramVirtualSlashOfferPrompt(ProgramSkillFrame frame, int actorSeat, bool selectingTarget) => _resolve().SetProgramVirtualSlashOfferPrompt(frame, actorSeat, selectingTarget);
}
