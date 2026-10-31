#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileEntries
{
    internal void CompleteMobileDingpin(ProgramSkillFrame supplied) => _resolve().CompleteMobileDingpin(supplied);
    internal bool KeepMobileDingpinInitialSelection(ProgramSkillFrame frame) => _resolve().KeepMobileDingpinInitialSelection(frame);
    internal bool KeepMobileDingpinPaid(ProgramSkillFrame frame) => _resolve().KeepMobileDingpinPaid(frame);
    internal bool MobileDingpinCardAllowed(int owner, Card card) => _resolve().MobileDingpinCardAllowed(owner, card);
    internal bool MobileDingpinCurrentTurn(int turn, int owner) => _resolve().MobileDingpinCurrentTurn(turn, owner);
    internal bool MobileDingpinPaid(ProgramSkillFrame frame) => _resolve().MobileDingpinPaid(frame);
    internal bool MobileDingpinTargetAllowed(int owner, int target) => _resolve().MobileDingpinTargetAllowed(owner, target);
    internal void PrepareMobileDingpin(ProgramSkillFrame supplied) => _resolve().PrepareMobileDingpin(supplied);
    internal bool QualifyMobileDingpin(ProgramActivationLegalityContext context) => _resolve().QualifyMobileDingpin(context);
}
