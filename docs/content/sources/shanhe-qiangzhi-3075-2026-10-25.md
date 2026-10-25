# S5 玩家强识3075复用核对

核对日期：2026-10-25。来源为社区[技能表](https://s.xiaochao.org/data/skills.json)与[研习树](https://s.xiaochao.org/data/tactic-tree.json)，沿用[本批玩家构筑技能来源](shanhe-build-skills-2026-10-25.md)已经保存的来源快照。本次重新解析本地快照并核对当前源码，没有重新请求线上数据。

## 来源条目与研习位置

技能表的3075条目：名称“强识”，`spellId=484`，品质4，原价1000铜钱，关联技能号484。源文为：“出牌阶段开始时，你可以展示一名其他角色一张手牌，然后当你本阶段使用与展示牌类型相同的牌时，你可以摸一张牌。”

研习树精确筛选`seasonId=5 && rewardId=3075`只得到一项：节点103，第44层、第4列，研习消耗45，前置节点98，无附加条件，奖励类型为技能，品质4。其他赛期也有同名、同号条目；接线应使用这项S5节点，不把它列入无固定节点的默认开放库存。

技能表SHA-256：`9FCAD12C69394382ED903986C7BFCC03FA13B0949173A3ED12B4AA008CB0B53E`。研习树SHA-256：`A754CA2F4AC25F0F140C8DF2D97693B5BB84EC5EDF55A52AC59F8CD511A18625`。

## 现有规则与运行时

| 内容 | 实际规则 | S5适用性 |
| --- | --- | --- |
| `classic:qiangzhi` | 出牌阶段开始随机展示其他角色一张手牌，三个类别状态使用`game`作用域；摸牌只要求用牌者是当前回合角色，至`turnEnding`清理 | 整个本人回合有效，与3075的本阶段范围不同 |
| `boundary:qiangzhi` | 先观看其他角色手牌并自选一张展示，再按阶段范围摸牌 | 展示方式不同，对应源表另一条3352，不能混用 |
| `mobile:qiangzhi` | 出牌阶段开始选择一名有手牌的其他存活角色，随机展示其一张手牌；同类别真实用牌提交后可摸1，三个类别状态均使用`playPhase`作用域 | 与3075完整规则一致，可直接复用 |

移动版规则位于`src/Characters/zhang-song/mobile/Skills/mobile-zhang-song.rules.json`，展示及内容登记分别位于同目录的presentation文件和`../Content/mobile-zhang-song.character.json`。它已归属`standard-classic-generals`，由`CardGame.Content.Standard.csproj`的Characters通配资源、`CharacterContentCatalog`和`StandardClassicGeneralPackage`进入正常showcase；山河现有战斗注册表已经包含该技能，无需独立山河技能包或新规则副本。

运行时核对了以下边界：

- `GameEngine.ProgramReveals.cs`使用原技能帧解析展示目标，从目标的现有手牌随机选牌；展示绑定留在该帧，随机模式不会打开观看全部手牌的私密选择。
- `GameEngine.ProgramLifecycle.cs`在`CompletePlayPhaseAfterProgramWindow`完成原出牌阶段结算后调用`ResetPlayPhaseProgramStates`，在进入弃牌阶段或恢复额外阶段之前清零；该方法按技能实例声明的`playPhase`状态还原初始值。因此这里的`cardActionActorIsCurrentTurn`虽与经典版同名，实际收益仍被类别状态限定于本次出牌阶段。
- 真实`cardUseCommitted`按`CardActionContext.EffectiveKind`归类，持有者关系为`actor`，不是每个目标各触发一次；摸牌沿现有可选触发及原生摸牌指令执行，没有新增付款或待处理状态。
- 规则中没有身份模式或限定普通张松本人才能发动的条件；绑定为山河额外技能后沿用原生的阶段、目标、技能实例与牌移动约束。

## 库存接线

`StandardShanheCampaignCatalog.BuildSkills.cs`的`CurrentBuildSkills`应登记：源ID3075、名称“强识”、技能ID`mobile:qiangzhi`、价格1000、品质4、S5节点103，奖励说明沿用3075源文。现有投影会生成`build-skill-3075`奖励、商店与战斗奖励池，并绑定研习节点103；占用一个额外技能槽，保持现有购买、替换和章节奖励流程。

本项是既有完整规则的内容接线，没有修改经典或移动张松，也没有增加操作码、状态结构、规则格式或恢复路径。不需要重复人物定义快照或新增机制夹具；构建与本批例行验证由共享集成串行执行。
