# Card development guidance

Card is a .NET rules engine with a WPF desktop client. Use the SDK selected by
[global.json](global.json); setup and project entry points are in
[docs/GETTING_STARTED.md](docs/GETTING_STARTED.md).

## Working boundaries

- Use PowerShell. Before any recursive delete or move, resolve and verify the absolute target, then use PowerShell cmdlets with `-LiteralPath`.
- Inspect Git status before editing or committing. Preserve concurrent changes and stage only the task's intended files or hunks.
- Routine design decisions and format migrations within the requested task are authorized; document material rule and compatibility decisions.
- Publishing, installers and distribution packages require an explicit publishing or packaging request.
- Reuse compatible temporary outputs and clean up this task's disposable files after inspection. Respect ownership, links and locks; never bypass a policy rejection or clear another task's outputs. Follow the exact cleanup procedure in [development gates](docs/DEVELOPMENT_GATES.md#temporary-files-and-cleanup) when creating or cleaning artifacts.

## Architecture essentials

- Skills compose rule concepts; each concept has one owner and one answer point. The goal is to express new skills entirely in JSON. Ordinary generals using existing capabilities change content and registration only.
- Shared capabilities must not depend on characters, and characters must not depend on other characters. Passing checks prove behavior, not architecture; assess the design using the capability map's criteria. Moving code or adding adapters is not evidence of simplification.
- A character-specific/compound operation, duplicate rule answer or character branch in a shared query is a gap signal. Follow [the capability method](docs/content/skill-composition/CAPABILITY_MAP.md), complete the concept with all equivalent consumers, and record remaining differences there.
- Before adding an operation or condition, check whether existing operations plus an effect-level `{"kind":"expression"}` condition express the rule. Add a missing read-only fact once in the shared owner; see [runtime expressions](docs/content/skill-composition/RUNTIME_EXPRESSIONS.md).
- Validate at command, loader, restore and executor boundaries. Keep pending state, accepted facts, payments and typed child returns on the original owning frame; do not rebuild them with character proof events, history scans or sidecars.
- Preserve input validation, seat-specific privacy, card movement, checkpoint, replay and content fingerprints. Keep source loss, owner death, ongoing eligibility and already-issued permission distinct.
- Do not add a global gameplay rules epoch or checkpoint `RulesVersion`, or bump formats/package versions per character. If loading or replay fails, preserve the source save while starting a fresh game. Format changes follow [development gates](docs/DEVELOPMENT_GATES.md#formats-and-content).

## Verification

- During development use relevant existing name filters; after a code/content batch run the routine scope once. Pure documentation changes need link/content review and `git diff --check`, not a game build.
- Add focused checks for uncovered behavior or regressions, WPF checks for UI behavior, and shared checks for changed capabilities. Pure configuration reuse needs no duplicate character fixture or fixed test count.
- **Full verification is manual only:** `-Full` and unfiltered test assemblies require an explicit user request for a manual full run. A routine failure, milestone or follow-up cycle never lifts this restriction.
- Preserve intentional test pruning when merging older work. Do not prune checks in the same batch as a large refactor or remove compatibility boundaries to reduce test count.
- Report the scope actually run and separate build/dependency/runner time from test-process time. The default smoke target is about one second.

Run from the repository root (Windows, PowerShell 7 and Python 3.10+):

```powershell
pwsh -File .\tools\Test-Changed.ps1 -CoreFilter 'program interaction '
pwsh -File .\tools\Test-Changed.ps1 -WpfFilter 'selection, target toggle'
pwsh -File .\tools\Test-Changed.ps1 # Routine scope, not the full suite.
git diff --check
```

Choose filters for the behavior being changed; examples are not a mandatory checklist.
The scope definition and full workflow are in [development gates](docs/DEVELOPMENT_GATES.md).

## Read when relevant

Read the applicable document before changing its boundary; do not load the entire documentation set for every task.

| Task | Guidance |
| --- | --- |
| Add or change a general, skill or shared rule | [Capability map](docs/content/skill-composition/CAPABILITY_MAP.md), [character registration](docs/CHARACTER_MODULES.md), [rule syntax](docs/content/skill-composition/RUNTIME.md) |
| Change a rule mechanism or any consumer of it | [Contract index](docs/content/skill-composition/contracts/README.md): select the affected topics and follow their owning-frame and lifetime contracts |
| Change formats, tests, verification or temporary outputs | [Development gates](docs/DEVELOPMENT_GATES.md) |
| Change desktop saves or recovery | [Save and resume](docs/SAVE_AND_RESUME.md) |
| Find UI, mode, content or architecture guidance | [Documentation index](docs/README.md) |

## Maintaining guidance

- Keep this root file short: repository-wide constraints, useful commands and task-based links.
- Maintain detailed mechanism contracts in the relevant [contract topic](docs/content/skill-composition/contracts/README.md); keep ownership, completion, semantics and remaining gaps in `CAPABILITY_MAP.md`. Do not append a new root bullet for every completed concept.
- Use a nested `AGENTS.md` only for instructions unique to that directory. Cross-cutting mechanisms belong in linked documents so Core, characters, activities and tests share one contract.
- Update the owning document instead of copying its rules into multiple guides. Put batch timings, size statistics and detailed verification results in commit messages.
