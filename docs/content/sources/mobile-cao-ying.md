# 曹婴（普通移动版）来源与实现

核对日期：2026-10-26。身份 `mobile:cao-ying`，人物 `character:cao-ying`，魏、女、4体力，SP。凌人、伏间注册83600、83601，武将及当前将池83699，肖像键 `mobile-cao-ying`。经典图为[官网46100.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/46100.jpg)。

## 现行正式来源

| 来源 | 本次确认 |
| --- | --- |
| [移动官网曹婴288](https://www.sanguosha.cn/index.php/pc/hero-detail-288.html) | 实际成功读取的是带 `index.php` 的官方同页路径；普通路径当次超时。明确身份版、魏、曹操孙女及完整凌人/伏间，派生奸雄没有摸1。 |
| [移动凌人](https://wiki.biligame.com/msgs/凌人)（[固定修订38768](https://wiki.biligame.com/msgs/index.php?title=凌人&oldid=38768)） | 2025-12-09正文：出牌阶段限一次，指定真实杀或伤害类锦囊目标后猜一名目标手牌三类；至少1项强化此牌对该目标的伤害，至少2项摸2，3项暂授奸雄、行殇至下回合开始。人物技能展示为4体力。 |
| [移动伏间](https://wiki.biligame.com/msgs/伏间)（[固定修订34990](https://wiki.biligame.com/msgs/index.php?title=伏间&oldid=34990)） | 2025-12-08正文：锁定，只有结束阶段；随机观看一名其他角色的X张手牌，X为全场最少手牌数。 |
| [移动奸雄/曹婴](https://wiki.biligame.com/msgs/奸雄/曹婴)（[固定修订34991](https://wiki.biligame.com/msgs/index.php?title=奸雄/曹婴&oldid=34991)） | 受到伤害后可获得造成伤害的牌，与官网派生正文一致，没有OL的额外摸1。 |
| [移动行殇](https://wiki.biligame.com/msgs/行殇)（[固定修订8341](https://wiki.biligame.com/msgs/index.php?title=行殇&oldid=8341)） | 曹婴衍生栏：其他角色死亡时，可获得其所有牌。 |

官网伏间多写“若全场角色均有手牌”；当前移动专页写为X公式。两者在X=0时不观看任何实体。本实现此时不生成无牌的观看窗、不消耗无意义随机数。

移动人物主页面本次多次超时，因此没有编造人物修订号。以上当前移动技能页及官方人物正文已实际读取；没有用另一个产品的 `/sgs/` 同名页代替 `/msgs/` 移动资料。

## 相对于已有OL的实质差异

本库OL凌人按实际回合限一次，至少猜对1项的许可作用于本回合下次对该角色的伤害，至少2项还返还原牌使用次数，3项授予 `ol:jianxiong`。OL伏间在准备与结束均发动，排除唯一手牌最多者并看完整手牌。这些都不是普通移动现行规则，因此不直接引用OL技能ID。

两项移动独立JSON继续使用 `guessTargetCategoriesAndArmTurnDamage` 和 `randomEligibleOtherHandInspection`，显式配置 `policy:"mobileOrdinary"`。策略领域枚举 `CaoYingRulePolicy.MobileOrdinary=0` 位于专属合同文件；省略策略保留OL默认。没有新增操作码、全局规则版本、包版本或中央人物分支。

移动凌人的触发资格是拥有者正在进行的真实出牌阶段，次数使用既有阶段账本；不额外要求实际回合归属者也是技能拥有者。原猜测帧冻结来源、内容指纹、真实UseId/ActionId、目标、手牌类别和阶段身份，实际回合归属单独取自 `ActualTurnProgression.OwnerSeat`，不从用牌者或阶段操作者猜测。伤害许可同时核对原生用牌、行动身份和指定目标；原牌被闪避或伤害被防止后，不会将加伤转移到另一张牌或技能伤害。摸2不会返还原牌次数。

三项猜对先冻结原猜测结果，先原生摸2并等所有摸牌、获得牌及其衍生子结算完成，再在同一原帧的授技阶段调用 `GameEngine.ProgramSkillChanges.cs` 的 `CommitProgramParticipantSkillGrants`。阶段游标先推进，再提交授技，授技子窗恢复不会重新摸2或再次授予。奖励技能直接引用等价的 `classic:jianxiong`、`classic:xingshang`；每个授予来源采用共享合同要求的 `turn:` 前缀，并带真实创建回合及实际回合归属；每个来源有独立的 `NextActualTurnStartExpiry`，由既有原生回合开始收回。结束当前回合、插入另一出牌阶段或失去凌人都不提前撤销已签发来源。已付猜测在失源后沿原帧继续；若拥有者已在摸牌子结算中死亡，则不向死亡受益者授予技能。没有新增平行pending，旧OL先授技再摸牌的时序保持默认。

移动伏间在结束时冻结全体存活角色中的最少手牌数，再从全部其他存活角色随机选一人，并从其真实手牌无放回随机选X张。原帧保存已冻结且只读的实际 `CardSnapshot` 集合；只有拥有者视图能看到这些实体，完成观看后沿原窗口返回。没有把“随机选一个角色”误实现成“查看其全部牌”，也不排除唯一手牌最多者。

## 定向行为覆盖

`MobileCaoYingChecks.PhaseQuotaOriginalCardOnlyNoRefundAndNativeTemporarySkills`：真实摸牌子窗中已有两张收益牌但尚未授技，冷恢复并返回后才获得技能；真实首张杀受到原生防伤，下一张杀不继承原牌加伤。同阶段不能再猜，不退还用牌次数；真实额外出牌阶段返回同回合正常出牌阶段可再次猜测，获得准确的普通奸雄/行殇，并在拥有者下次真实回合开始到期。

`MobileCaoYingChecks.EndingMinimumRandomSubsetPrivateCold`：通过真实交牌建立手牌最少值，准备阶段没有伏间；结束时只私看随机他者的X张真实实体，在私看窗口冷恢复保持目标、实体、随机状态和原帧一致。

已有OL `CaoYingChecks.GrantedCurrentJianxiongDrawChildThenClaimsNativeDamageCard` 保留旧派生技能摸1再得原伤害牌的回归。暂授技能的共享变更、独立来源及到期边界沿用现有Skill changes覆盖，不复制整套矩阵。本文描述实现与定向覆盖，实际通过情况以批次统一验证结果为准。
