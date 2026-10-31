#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileChenQunHost(Func<RuntimeEntries.CharacterChenQunMobileEntries> getCharacterChenQunMobileRuntime) : IMobileChenQunHost, IMobileChenQunSelection
{
    public bool CardAllowed(int owner, Card card) => getCharacterChenQunMobileRuntime().MobileDingpinCardAllowed(owner, card);
    public bool TargetAllowed(int owner, int target) => getCharacterChenQunMobileRuntime().MobileDingpinTargetAllowed(owner, target);
    public void Run(ProgramSkillFrame frame, SkillProgramEffectOp operation)
    {
        if (operation == SkillProgramEffectOp.PrepareMobileChenQunDingpin)
            getCharacterChenQunMobileRuntime().PrepareMobileDingpin(frame);
        else
            getCharacterChenQunMobileRuntime().CompleteMobileDingpin(frame);
    }
}
