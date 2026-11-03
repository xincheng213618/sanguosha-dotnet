using CardGame.Core.EngineOwners;

namespace CardGame.Core;

internal interface IPublicHandRevealNativeReturns : IProgramRuntimeOperations
{
    void Return(PublicHandRevealWindowFrame window);
}

// Window completion returns to its original producer. The producer's own
// continuation retains ownership of its remaining native resolution.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class PublicHandRevealNativeReturnsRuntime(IProgramRuntimeFrames frames,
    ICardZoneQueries zones, RuntimeEntries.CoreNativeFireAttackEntries fireAttack,
    IKernelTryResumeOperations programReturns, IKernelPublicHandRevealReturnOperations registeredReturns)
    : IPublicHandRevealNativeReturns
{
    public void Return(PublicHandRevealWindowFrame window)
    {
        if (window.ReturnKind == PublicHandRevealReturnKind.Program)
        {
            if (!programReturns.TryResume(
                EngineOwners.CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value,
                window.ParentFrameId))
                frames.AdvanceProgram(window.ParentFrameId);
        }
        else if (window.ReturnKind == PublicHandRevealReturnKind.FireAttack)
        {
            var use = frames.Find(window.ParentFrameId) as CardUseFrame ??
                throw new InvalidOperationException("Public reveal lost its actual FireAttack.");
            var cardId = use.FireAttackSelection?.RevealedCardId ??
                throw new InvalidOperationException("FireAttack cannot replace its frozen revealed entity.");
            fireAttack.BeginFireAttackDiscard(use,
                zones.CardsAt(zones.GetLocation(cardId)).Single(card => card.Id == cardId));
        }
        else if (window.ReturnKind == PublicHandRevealReturnKind.Registered)
        {
            var parent = frames.Find(window.ParentFrameId) ??
                throw new InvalidOperationException("Public Hand reveal lost its registered native producer.");
            registeredReturns.Return(PublicHandRevealReturns.Default, window.ReturnKey!, parent);
        }
        else
            frames.Advance(window.ParentFrameId);
    }
}
