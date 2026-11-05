using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeCursorSuspensionChecks
{
    public static void OriginalOwnersKeepIndependentIssuedIntervals()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var runtime = capabilities.Get<INativeCursorSuspensions>();
        var source = capabilities.Get<IMatchParticipantStateAccess>().Players
            .First(player => player.SkillGrants.Grants.Any(grant => !grant.IsModeRule));
        var grant = source.SkillGrants.Grants.First(grant => !grant.IsModeRule);
        var priorTop = frames.Top;
        var priorCount = frames.OfType<ResolutionFrame>().Count;
        const NativeCursorScope attack = NativeCursorScope.AttackAndContinuations;
        const NativeCursorScope dying = NativeCursorScope.Dying;
        const NativeCursorScope counterspell = NativeCursorScope.Nullification;
        var use = new CardUseFrame(frames.NextId(), 0, 0, CardKind.Slash, [1], PhysicalCardIds: []);
        var nullification = new NullificationWindowFrame(frames.NextId(), use.Id, 0, [1], 0,
            CardKind.Duel, LegalActionKind.Duel, null, null, [0, 1]);
        var originalDying = new DyingFrame(frames.NextId(), nullification.Id, 1, 0, [0, 1], 0, DyingContinuationKind.Damage);
        var first = Issuer("first");
        var second = Issuer("second");
        frames.Push(use);
        frames.Push(nullification);
        frames.Push(originalDying);
        frames.Push(first);
        frames.Push(second);
        try
        {
            var unchanged = frames.Find(use.Id);
            Reject(() => runtime.Suspend(use.Id, long.MaxValue, attack));
            Reject(() => runtime.Suspend(long.MaxValue, second.Id, attack));
            Reject(() => runtime.Suspend(second.Id, second.Id, attack));
            Reject(() => runtime.Suspend(second.Id, first.Id, attack));
            Reject(() => runtime.Suspend(use.Id, second.Id, dying));
            Reject(() => runtime.Suspend(use.Id, second.Id, NativeCursorScope.None));
            Reject(() => runtime.Suspend(use.Id, second.Id, (NativeCursorScope)8));
            Reject(() => runtime.Release(second.Id, (NativeCursorScope)8));
            Reject(() => runtime.IsSuspended(use.Id, (NativeCursorScope)8));
            Need(ReferenceEquals(unchanged, frames.Find(use.Id)),
                "A suspension rejects forged, self, reversed, incompatible and unknown-scope identities before changing its actual native owner.");

            runtime.Suspend(use.Id, first.Id, attack);
            runtime.Suspend(nullification.Id, first.Id, counterspell);
            runtime.Suspend(originalDying.Id, first.Id, dying);
            runtime.Suspend(use.Id, second.Id, attack);
            runtime.Suspend(originalDying.Id, second.Id, dying);
            var declared = frames.Find(originalDying.Id)!;
            runtime.Suspend(originalDying.Id, second.Id, dying);
            Need(ReferenceEquals(declared, frames.Find(originalDying.Id)) &&
                declared.NativeCursorSuspensions.Count == 2 &&
                declared.NativeCursorSuspensions is System.Collections.IList { IsReadOnly: true } &&
                runtime.OwnsSuspension(originalDying.Id, first.Id, dying) &&
                runtime.OwnsSuspension(originalDying.Id, second.Id, dying),
                "Independent issuers keep frozen original-frame declarations and identical issuance remains idempotent.");
            frames.Replace(originalDying.Id, JsonSerializer.Deserialize<DyingFrame>(JsonSerializer.Serialize((DyingFrame)declared))!);
            Need(runtime.IsSuspended(use.Id, attack) && runtime.IsSuspended(nullification.Id, counterspell) &&
                runtime.IsSuspended(originalDying.Id, dying) &&
                runtime.OwnsSuspension(originalDying.Id, second.Id, dying),
                "Original attack, counterspell and dying scopes retain their exact issuers through ordinary frame JSON recovery.");

            runtime.Release(first.Id, attack);
            Need(!runtime.OwnsSuspension(use.Id, first.Id, attack) && runtime.IsSuspended(use.Id, attack) &&
                !runtime.HasIssuedSuspension(use.Id, attack, originalDying.Id) &&
                runtime.HasIssuedSuspension(use.Id, attack, first.Id) &&
                runtime.OwnsSuspension(use.Id, second.Id, attack) &&
                runtime.OwnsSuspension(originalDying.Id, first.Id, dying) &&
                runtime.OwnsSuspension(nullification.Id, first.Id, counterspell),
                "Releasing one issuer's attack interval neither withdraws another issuer nor that issuer's distinct dying and counterspell intervals.");
            runtime.Release(second.Id, attack);
            Need(!runtime.IsSuspended(use.Id, attack) && runtime.IsSuspended(originalDying.Id, dying),
                "The original attack becomes visible only after both issued attack intervals end.");
            Need(source.SkillGrants.RemoveGrant(grant.GrantId), "The fixture removes its actual non-mode source grant.");
            source.IsAlive = false;
            Need(runtime.IsSuspended(originalDying.Id, dying) && runtime.IsSuspended(nullification.Id, counterspell),
                "An already-issued cursor interval does not requalify the caster's skill possession or survival.");

            var nestedDying = new DyingFrame(frames.NextId(), second.Id, 0, 1, [0, 1], 0, DyingContinuationKind.ProgramSkill);
            frames.Push(nestedDying);
            Need(runtime.IsSuspended(originalDying.Id, dying) && !runtime.IsSuspended(nestedDying.Id, dying),
                "Suspending the original dying cursor cannot hide a different newly issued nested dying frame.");
            frames.Pop(nestedDying.Id, nestedDying.Kind);
            frames.Pop(second.Id, second.Kind);
            Need(!runtime.OwnsSuspension(originalDying.Id, second.Id, dying) &&
                runtime.OwnsSuspension(originalDying.Id, first.Id, dying),
                "A completed issuer stops contributing without deleting another live issuer's interval.");
            runtime.Release(first.Id, dying);
            Need(!runtime.IsSuspended(originalDying.Id, dying) && runtime.IsSuspended(nullification.Id, counterspell),
                "Releasing the remaining dying interval exposes its original cursor and preserves an independently active counterspell interval.");
            runtime.Release(first.Id);
            Need(!runtime.IsSuspended(use.Id, attack) && !runtime.IsSuspended(nullification.Id, counterspell) &&
                !runtime.IsSuspended(originalDying.Id, dying),
                "The default release removes only the selected issuer's remaining scopes across its actual original owners.");

            var reveal = new PublicHandRevealWindowFrame(frames.NextId(), first.Id, PublicHandRevealReturnKind.Program);
            frames.Push(reveal);
            try
            {
                Need(runtime.IsSuspended(use.Id, attack) && runtime.IsSuspended(nullification.Id, counterspell) &&
                    runtime.IsSuspended(originalDying.Id, dying) &&
                    !runtime.HasIssuedSuspension(originalDying.Id, dying) &&
                    !runtime.OwnsSuspension(originalDying.Id, first.Id, dying),
                    "The shared aggregate preserves public-Hand reveal suspension while issuer-specific continuation rights read only their own declared interval.");
            }
            finally { frames.Pop(reveal.Id, reveal.Kind); }
            Need(!runtime.IsSuspended(originalDying.Id, dying),
                "Returning the public reveal removes its aggregate suspension without inventing a producer declaration.");
        }
        finally
        {
            while (frames.OfType<ResolutionFrame>().Count > priorCount)
            {
                var top = frames.Top!;
                frames.Pop(top.Id, top.Kind);
            }
        }
        Need(ReferenceEquals(priorTop, frames.Top), "Suspension checks restore the original native stack.");

        ProgramSkillFrame Issuer(string binding) => new(frames.NextId(), source.Seat, grant.SkillId, binding, "fixture:cursor", 0, [], [])
            { SkillInstanceId = grant.SkillInstanceId };
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A cursor suspension escaped its real ancestor, issuer or declared scope.");
    }
}
