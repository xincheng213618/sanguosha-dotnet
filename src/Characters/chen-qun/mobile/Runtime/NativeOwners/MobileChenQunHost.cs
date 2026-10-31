#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileChenQunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChenQunMobileRuntime>> getCharacterChenQunMobileRuntime) : IMobileChenQunHost, IMobileChenQunSelection
{
    public bool CardAllowed(int owner, Card card) => getCharacterChenQunMobileRuntime().Value.MobileDingpinCardAllowed(owner, card);
    public bool TargetAllowed(int owner, int target) => getCharacterChenQunMobileRuntime().Value.MobileDingpinTargetAllowed(owner, target);
    public void Run(ProgramSkillFrame frame, SkillProgramEffectOp operation)
    {
        if (operation == SkillProgramEffectOp.PrepareMobileChenQunDingpin)
            getCharacterChenQunMobileRuntime().Value.PrepareMobileDingpin(frame);
        else
            getCharacterChenQunMobileRuntime().Value.CompleteMobileDingpin(frame);
    }
}
